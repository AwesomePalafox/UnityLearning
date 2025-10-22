using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDeparture : MonoBehaviour
{

    public enum TeleportType
    {
        SameScene, DifferentScene
    }
    [Header("Teleport Info")]
    public string sceneName;

    public TeleportType teleportType;

    public TeleportDestination.DestinationTag destinationTag; // SceneController 中有引用

    private bool canTrans;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && canTrans)
        {
            // TODO: SceneController 传送
            SceneController.Instance.TransitionToDestination(this);
    // this 表示的是当前挂载了这个脚本的 GameObject 上的 TeleportDeparture 组件实例。TeleportDeparture 类型的对象，不是 GameObject 本身，但你可以通过它访问 GameObject
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
            canTrans = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
            canTrans = false;
    }
}
