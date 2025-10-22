using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>
{
    GameObject player;

    NavMeshAgent playerAgent;
    public void TransitionToDestination(TeleportDeparture teleportDeparture)
    {
        switch (teleportDeparture.teleportType)

        // switch 语句，通过判断给定的 （ ）里的变量，来执行一个 其中一个 case - break  方法。
        {
            case TeleportDeparture.TeleportType.SameScene:
                StartCoroutine(Teleport(SceneManager.GetActiveScene().name, teleportDeparture.destinationTag)); // 确认\传入  去的点的 Tag  *********
                break;





            case TeleportDeparture.TeleportType.DifferentScene:

                break;
        }
    }

IEnumerator Teleport (string sceneName,TeleportDestination.DestinationTag destinationTag)  // 传入 的 去的点的 Tag *********
    {
        player = GameManager.Instance.playerStats.gameObject;
        playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;

        player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

        playerAgent.enabled = true;

        yield return null;
    }


private TeleportDestination GetDestination(TeleportDestination.DestinationTag destinationTag)  // 传入 Departure 点 挂的 （要去的点（也就是终点的）） Tag ********
    {
        var arraynumber = FindObjectsOfType<TeleportDestination>();  // 找场景内所有挂了 TeleportDestination 组件 的元素，并返回一个数组 Array，里面包含了场景中所有类型为 TeleportDestination 的对象
        // FindObjectsOfType<TeleportDestination>() 返回的是一个数组 Array，里面包含了场景中所有类型为 TeleportDestination 的对象

        for (int i =0; i < arraynumber.Length; i++)  // entrances.Length 的意思就是数组 entrances 的元素总数 .Length 是数组的一个属性，表示这个数组中有多少个元素
        {
            if (arraynumber[i].destinationTag == destinationTag)
                return arraynumber[i];
        }

        return null;
    }

}
