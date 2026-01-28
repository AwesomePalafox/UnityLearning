using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainManue : MonoBehaviour
{
    Button newGameBtn;

    Button continueBtn;

    Button quitBtn;

    PlayableDirector director;

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();


        newGameBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);

        // onClick.AddListener 构成了 点击 Button 后的事件监听，后挂函数，即可触发函数。
        director = FindObjectOfType<PlayableDirector>();

        /* 错误案例
        director.stopped += NewGame; 


        你把方法 NewGame 赋给了一个需要 Action<PlayableDirector> 类型的方法指针（委托），但 NewGame 的签名不符合 这个委托的要求，因此无法绑定。
        在 Unity 里，PlayableDirector 的事件（如 played, paused, stopped）的类型就是 Action<PlayableDirector>，也就是需要一个参数（当前触发事件的 PlayableDirector 实例）并且无返回值的函数：

public event Action<PlayableDirector> played;
public event Action<PlayableDirector> paused;
public event Action<PlayableDirector> stopped;

        因此，能直接绑定到这些事件的方法必须满足这个签名： oid SomeHandler(PlayableDirector director) { ... }

        
        */
        director.stopped += NewGame; // 此处 stopped 为 PlayableDirector 类中的事件。指“动画播放完”的这一事件
        // 此段含义为： 当事件 stopped 达成后，执行 NewGame 参数。
    }

    void PlayTimeline()
    {
        director.Play(); // 此 Play 为 PlayableDirector 类中的函数。
    }

    void NewGame(PlayableDirector point)
    {
        // 删除所有已有数据
        PlayerPrefs.DeleteAll();

        //转换场景， 在 scene controller 中完成
        SceneController.Instance.TransitionToFirstLevel();
    }
        
    void ContinueGame()
    {
        // 转换场景，读取进度
        SceneController.Instance.TransitionToLoadGame();
    }


    void QuitGame()
    {
        Application.Quit();
        Debug.Log("000exit000");
    }


}
