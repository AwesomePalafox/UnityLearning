using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;

    public Transform barPoint;

    public bool isAlwaysVisible;

    public float visiableTime;

    private float timeLeft;
    Image healthSlider;

    Transform UIbar;

    Transform cam;

    CharacterStats currentStats;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }



    void OnEnable() // 人物启动时调用
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>()) //注意此处是 FindObjectsOfType Objects 的 s 要注意！
        {
            if (canvas.renderMode == RenderMode.WorldSpace) // 此寻找方法有弊端，可选择另外的方法，如寻找 tag 等来进行筛选
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;  // Instantiate 生成出来一份副本 让UIbar 变量拿到 healthUIPrefab 的 transform 参数。
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(isAlwaysVisible);
            }
        }
    }


    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);

        UIbar.gameObject.SetActive(true); // Transform 是一个组件（Component），它附加在某个 GameObject 上。所有组件都有一个 .gameObject 属性，指向它所属的 GameObject。所以你可以通过 UIbar.gameObject 获取到这个 Transform 所在的 GameObject
        timeLeft = visiableTime;

        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void LateUpdate() // 在update之后一帧进行的 update
    {
        if (UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if (timeLeft <= 0 && !isAlwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;

        }
    }




}
