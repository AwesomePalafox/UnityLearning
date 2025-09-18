using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing }

    public RockStates rockStates;

    private Rigidbody rb;

    [Header("Basic Setting")]

    public float force;
    public GameObject target;
    // 于 Golem 脚本， ThrowRock 函数中 获取 target。

    public int damage;

    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;   // 用于避免 在 Rock 脚本 fixedupdate 生命周期中，判断 velocity < 1 而改变石头状态
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1)
        {
            rockStates = RockStates.HitNothing;
        }
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;

        direction = (target.transform.position - transform.position + Vector3.up * 2).normalized;
        // Vector3.up 为 (0,1,0)
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision theothercollider)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (theothercollider.gameObject.CompareTag("Player"))
                {
                    theothercollider.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                     theothercollider.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    // 这里没有成功，AI 说 velocity 是只读的，不能赋值  
                 /*    theothercollider.gameObject.GetComponent<Rigidbody>().AddForce(direction * 3, ForceMode.Impulse);    */


                    theothercollider.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

                    theothercollider.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, theothercollider.gameObject.GetComponent<CharacterStats>());

                    rockStates = RockStates.HitNothing;
                }
                break;



            case RockStates.HitEnemy:
                if (theothercollider.gameObject.GetComponent<Golem>()) // GetComponent 中自带有判断是否存在的 bool 值， 因此可被放到 if 语句中
                {
                    var theothercolliderStats = theothercollider.gameObject.GetComponent<CharacterStats>();
                    theothercolliderStats.TakeDamage(damage, theothercolliderStats);

                    Destroy(gameObject);
                }
                break;

        }
    }


}


//  