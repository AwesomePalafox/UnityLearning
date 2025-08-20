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



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }


    void Update()
    {
        SwitchAnimation();

        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
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
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;

        transform.LookAt(attackAimedTarget.transform);

        while (Vector3.Distance(attackAimedTarget.transform.position, transform.position) > 1)
        {
            agent.destination = attackAimedTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        // Attack

        if (lastAttackTime < 0)
        {
            anim.SetTrigger("Attack");
            
            // 重置冷却时间
            lastAttackTime = 0.5f;
        }

    }
}
