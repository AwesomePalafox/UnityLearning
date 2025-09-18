using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Golem : EnemyController
{

    [Header("Skill")]
    public float kickForce = 25;

    public GameObject rockPrefab;
    public Transform handPos;

    // Animation Event
    public void KickOff()
    {
        if (EnemyAttackTarget != null && transform.IsFacingTarget(EnemyAttackTarget.transform))
        {
            var targetStats = EnemyAttackTarget.GetComponent<CharacterStats>();

            Vector3 direction = (EnemyAttackTarget.transform.position - transform.position).normalized;

            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            // 根据个人喜好添加
            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    // Animation Event
    public void ThrowRock()
    {
        if (EnemyAttackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            // Quaternion.identity 为旋转， 意为 Prefab 原始的旋转
            rock.GetComponent<Rock>().target = EnemyAttackTarget;

        }
    }
}
