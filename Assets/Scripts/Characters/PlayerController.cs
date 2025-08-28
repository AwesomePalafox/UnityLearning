using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private Animator anim;

    private GameObject attackAimedTarget;

    private float lastAttackTime;

    private CharacterStats characterStats;

    private bool isDeath;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;

        GameManager.Instance.RigistterPlayer(characterStats);
    }


    void Update()
    {
        isDeath = characterStats.CurrentHealth == 0; // 布尔值判断  == 0  等于 返回 true, 不等于返回 false.
        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDeath);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackAimedTarget = target;
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget()); 
        }
    }

    IEnumerator MoveToAttackTarget() // 应用于 EventAttack → void Start MouseManager
    {
        agent.isStopped = false;

        transform.LookAt(attackAimedTarget.transform);

        while (Vector3.Distance(attackAimedTarget.transform.position, transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackAimedTarget.transform.position;
            yield return null; // yield return null;：这是协程的关键部分，表示“等待一帧”，然后继续执行循环。这样可以在每一帧都重新评估角色与目标之间的距离，直到角色进入攻击范围
        } // 这段代码的作用是：让角色不断向攻击目标移动，直到进入攻击范围为止。

        agent.isStopped = true;
        // Attack

        if (lastAttackTime < 0)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Chritical", characterStats.isCritical);

            // 重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }

    }

    // Animation Event

    void Hit()
    {
        var targetStats = attackAimedTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
    }



}
