using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    TextMeshProUGUI levelText;

    Image healthSlider;

    Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
       
            UpdateHealth();
            UpdateExp();

            levelText.text = "Level  " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
            // ToString 是 C# 中用于格式化数字为字符串的一种方式，常用于 Unity 中显示数字时保持统一的位数格式 "00" 表示：总共至少显示两位数，不够就补 0。
       
    }

void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.levelUpExp;
        expSlider.fillAmount = sliderPercent;
    }

}
