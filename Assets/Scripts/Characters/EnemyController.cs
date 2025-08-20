using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour


{

    private EnemyStates enemyStates;
    private NavMeshAgent agent;

    private Animator anim;

    private Vector3 wayPoint;

    [Header("Basic Settings")]
    public float sightRadius;
    [SerializeField] EnemyStates States;
    [SerializeField] private float moveSpeedTest;
    // 上变这个是自己测试用的，这样写可以在inspector 中显示出来这个参数

    public float lookAtTime;

    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private float EnemySpeed;
    private GameObject EnemyAttackTarget;

    // bool 值配合动画转换
    bool isGuard;
    bool isWalk;
    bool isChase;
    bool isFollow;

    private Vector3 guardPosition;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        EnemySpeed = agent.speed;

        guardPosition = transform.position;

        remainLookAtTime = lookAtTime;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
            
        
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }


    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
    }

    void SwitchStates()
    {

        // 如果发现player, 切换到chase 模式

        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            Debug.Log("找到player");
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;

            case EnemyStates.PATROL:

                isChase = false;

                agent.speed = EnemySpeed * 0.5f;

                // 判断是否到了随机巡逻点。

                if (Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                    GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }

            break;


            case EnemyStates.CHASE:

                isWalk = false;
                isChase = true;

                agent.speed = EnemySpeed;

                // 可能会执行的事件： 
                // 1：追Player； 
                ChaseActions();
                // 2：拉脱范围回到上一个状态
                // 3:在攻击范围内则攻击
                // 4：适配动画

                break;


            case EnemyStates.DEAD:
                break;

        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var element in colliders)
        {
            if (element.CompareTag("Player"))
            {

                EnemyAttackTarget = element.gameObject;
                return true;

            }
        }

        EnemyAttackTarget = null;
        return false;

    }

    void ChaseActions()
    {



        if (!FoundPlayer())
        {
            //2.拉脱范围回到上一个状态
            isFollow = false;
            
            if (remainLookAtTime > 0)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
                enemyStates = EnemyStates.GUARD;
            else
                enemyStates = EnemyStates.PATROL;
            
        }
        else
        {
            isFollow = true;
            agent.destination = EnemyAttackTarget.transform.position;
        }
    }

    void GetNewWayPoint()
    {

        remainLookAtTime = lookAtTime - Random.Range(0f, 5f);;


        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPosition.x + randomX, transform.position.y, guardPosition.z + randomZ);

        NavMeshHit hit;

        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
        
        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);

    }
}
 