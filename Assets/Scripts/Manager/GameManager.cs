using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    private CinemachineFreeLook followCamera;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this); // 切换场景时不要销毁这个GameManager
    }

    public void RigistterPlayer(CharacterStats player)
    {
        playerStats = player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if(followCamera != null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }
    }

    #region called in  [Enemy Controller]
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }
    #endregion

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();  // EndNotify() 在 EnemyController 中给出方法
        }

    }
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TeleportDestination>())
        {
            if (item.destinationTag == TeleportDestination.DestinationTag.ENTER)
                return item.transform;
        }
        return null;
    }
}
