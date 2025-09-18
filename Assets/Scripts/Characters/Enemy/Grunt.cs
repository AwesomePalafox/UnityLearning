using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10;

    public void KickOff()
    {
        if (EnemyAttackTarget != null && transform.IsFacingTarget(EnemyAttackTarget.transform))
        {
            transform.LookAt(EnemyAttackTarget.transform);

            UnityEngine.Vector3 direction = EnemyAttackTarget.transform.position - transform.position;
            direction.Normalize();   //Normalize() 是 Vector3 的一个方法，用于将向量标准化（归一化）。标准化后的向量长度为 1，但方向不变。这样做的目的是为了只保留方向信息，而不考虑距离大小，常用于移动、旋转、射线等操作。

            EnemyAttackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            EnemyAttackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            EnemyAttackTarget.GetComponent<Animator>().SetTrigger("Dizzy");



        }
    }
}
