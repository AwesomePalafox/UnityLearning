using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public float fadeInDuration;
    public float fadeOutDuration;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject); // 服务于第一次之后的，后续的场景转换。
    }


    public IEnumerator FadeOutIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }
    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1) // 只要当前透明度还没达到 1，就持续执行循环。
        {
            canvasGroup.alpha += Time.deltaTime / time; // 把每帧增加量设为“总路程 1 / 总时长 time”
            // 在约 time 秒内，把 alpha 从当前值（通常是 0）增至 1，实现淡入效果

            yield return null;
        }
    }

        public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0) // 只要当前透明度还没达到 1，就持续执行循环。
        {
            canvasGroup.alpha -= Time.deltaTime / time; // 把每帧增加量设为“总路程 1 / 总时长 time”
                                                        // 在约 time 秒内，把 alpha 从当前值（通常是 0）增至 1，实现淡入效果

            yield return null;
        }
        Destroy(gameObject);
    }
}
