using System.Collections;
using System.Collections.Generic;
// using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;
    public SceneFader sceneFaderPrefab;

    // 以上两个 Prefab 需要在 Inspector 窗口中去进行 Prefab 赋值。

    bool ifGameEnd;
    GameObject player;

    NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this); // 切换场景时不要销毁这个组件
    }

    void Start()
    {
        GameManager.Instance.AddObserver(this);
        ifGameEnd = false;
    }

    public void TransitionToDestination(TeleportDeparture teleportDeparture)
    {
        switch (teleportDeparture.teleportType)

        // switch 语句，通过判断给定的 （ ）里的变量，来执行一个 其中一个 case - break  方法。
        {
            case TeleportDeparture.TeleportType.SameScene:
                StartCoroutine(Teleport(SceneManager.GetActiveScene().name, teleportDeparture.destinationTag)); // 确认\传入  去的点的 Tag  *********
                break;
            case TeleportDeparture.TeleportType.DifferentScene:
                StartCoroutine(Teleport(teleportDeparture.sceneName, teleportDeparture.destinationTag));
                break;
        }
    }

IEnumerator Teleport (string sceneName,TeleportDestination.DestinationTag destinationTag)  // 传入 的 去的点的 Tag *********
    {
        // 保存数据
        SaveManager.Instance.SavePlayerData();


        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneFader fade = Instantiate(sceneFaderPrefab);
            yield return StartCoroutine(fade.FadeOut(1.5f));

            yield return SceneManager.LoadSceneAsync(sceneName);    // yield return 是让程序运行等待
            /*
                        GameObject existingPlayer = GameObject.FindWithTag("Player");

                        if (existingPlayer != null)
                        {
                            Destroy(existingPlayer);
                        }
            */
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

            SaveManager.Instance.LoadPlayerData();

            yield return StartCoroutine(fade.FadeIn(1.5f));

            yield break;    // 执行完上变得指令即从协程中跳出

        }

        else
        {
            SceneFader fade = Instantiate(sceneFaderPrefab);
            yield return StartCoroutine(fade.FadeOut(1.5f));

            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;

            yield return StartCoroutine(fade.FadeIn(1.5f));

            yield return null;
        }
    }


    private TeleportDestination GetDestination(TeleportDestination.DestinationTag destinationTag)  // 传入 Departure 点 挂的 （要去的点（也就是终点的）） Tag ********
    {
        var arraynumber = FindObjectsOfType<TeleportDestination>();  // 找场景内所有挂了 TeleportDestination 组件 的元素，并返回一个数组 Array，里面包含了场景中所有类型为 TeleportDestination 的对象
        // FindObjectsOfType<TeleportDestination>() 返回的是一个数组 Array，里面包含了场景中所有类型为 TeleportDestination 的对象

        for (int i = 0; i < arraynumber.Length; i++)  // entrances.Length 的意思就是数组 entrances 的元素总数 .Length 是数组的一个属性，表示这个数组中有多少个元素
        {
            if (arraynumber[i].destinationTag == destinationTag)
                return arraynumber[i];
        }

        return null;
    }
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("GrassLand"));
    }


    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(1.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            // 保存数据
            SaveManager.Instance.SavePlayerData();

            yield return StartCoroutine(fade.FadeIn(1.5f));
            yield break;
        }

    }
    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(1.5f));

        yield return SceneManager.LoadSceneAsync("Main");

        yield return StartCoroutine(fade.FadeIn(1.5f));

        yield break;
    }

    public void EndNotify()
    {
        ifGameEnd = true;
        if(ifGameEnd)
        {
            ifGameEnd = false;
            StartCoroutine(LoadMain());
        }
    
    }
}
