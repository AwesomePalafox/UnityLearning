using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Golem : EnemyController
{

     [Header("Skill")]
    public float kickForce = 25;


    public void KickOff()
    {
        if (EnemyAttackTarget != null && transform.IsFacingTarget(EnemyAttackTarget.transform))
        {
            var targetStats = EnemyAttackTarget.GetComponent<CharacterStats>();

             UnityEngine.Vector3 direction = (EnemyAttackTarget.transform.position - transform.position).normalized;

            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
}
