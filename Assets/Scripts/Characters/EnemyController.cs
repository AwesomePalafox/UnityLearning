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
    // [SerializeField] EnemyStates States;

     public   bool isGuard;
    [SerializeField] private float moveSpeedTest;
    // 上变这个是自己测试用的，这样写可以在inspector 中显示出来这个参数

    public float lookAtTime;

    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;

    private float EnemySpeed;
    private GameObject EnemyAttackTarget;

    // bool 值配合动画转换

    bool isWalk;
    bool isChase;
    bool isFollow;

    private Vector3 guardPosition;

    private CharacterStats characterStats;

    private float lastAttackTime;

    private Quaternion guardRotation;

    bool isDeath;

    private Collider coll;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        EnemySpeed = agent.speed;

        guardPosition = transform.position;

        guardRotation = transform.rotation;

        remainLookAtTime = lookAtTime;

        characterStats = GetComponent<CharacterStats>();

        coll = GetComponent<Collider>();
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
        lastAttackTime -= Time.deltaTime;

        if (characterStats.CurrentHealth == 0)
         isDeath = true; 
    }


    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDeath);
    }

    void SwitchStates()
    {
        if (isDeath)
            enemyStates = EnemyStates.DEAD;

        // 如果发现player, 切换到chase 模式

            else if (FoundPlayer()) // 用 else if 是设立前提： 非死亡状态下执行下列语句
            {
                enemyStates = EnemyStates.CHASE;
                Debug.Log("找到player");
            }


        switch (enemyStates)
        {
            case EnemyStates.GUARD:

                isChase = false;
                if (transform.position != guardPosition)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPosition;

                    if (Vector3.SqrMagnitude(guardPosition - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f); // Lear : 实现缓慢变化的 自有函数
                    }
                }
                break;

            case EnemyStates.PATROL:

                isChase = false;

                agent.speed = EnemySpeed * 0.5f;

                // 判断是否到了随机巡逻点。

                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
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

                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        // 用 random value 暴击判断 (与 CharacterState 中联动)
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;

                        // 如果 random value 生成的随机数小于暴击率 （criticalChance） 则 isCritical （CharacterState 中的 bool 值 返回 true）

                        //进行攻击
                        Attack();
                    }
                }

                break;


            case EnemyStates.DEAD:

                coll.enabled = false; // 将collider 关闭，因为 Mouse Manager 中会检测 Collider (Line 62) 此行命令会使死亡单位无法被点击从而导致“攻击尸体”现象。
                agent.enabled = false; // 将 agent 关闭，则 NavMeshAgent 功能无法启用，作用：停止导航行为、节省性能、防止冲突或错误如果角色死亡后仍然尝试移动，可能会导致动画或物理上的冲突。禁用 NavMeshAgent 可以确保角色保持静止状态。

                Destroy(gameObject, 2f);

                break;

        }
    }

    void Attack()
    {
        transform.LookAt(EnemyAttackTarget.transform);
        if (TargetInAttackRange())
        {
            // 近距离攻击动画
            anim.SetTrigger("Attack");
        }

        if (TargetInSkillRange())
        {
            // 远距离攻击动画
            anim.SetTrigger("Skill");
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
    bool TargetInAttackRange()
    {
        if (EnemyAttackTarget != null) return Vector3.Distance(EnemyAttackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else return false;
    }

    bool TargetInSkillRange()
    {
        if (EnemyAttackTarget != null) return Vector3.Distance(EnemyAttackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else return false;
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
            agent.isStopped = false;
            agent.destination = EnemyAttackTarget.transform.position;
        }
    }

    void GetNewWayPoint()
    {

        remainLookAtTime = lookAtTime - Random.Range(0f, 5f); ;


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


    // Animation Event

    void Hit()
    {
        if (EnemyAttackTarget != null)
        {
            var targetStats = EnemyAttackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
 