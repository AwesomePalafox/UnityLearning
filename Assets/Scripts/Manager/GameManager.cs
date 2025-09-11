using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;


    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    public void RigistterPlayer(CharacterStats player)
    {
        playerStats = player;
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

}
