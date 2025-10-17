using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Data", menuName = "Character States/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;


    [Header("Kill")]
    public int killpoint;


    [Header("Level")]
    public int currentLevel;

    public int maxLevel;

    public int levelUpExp;

    public int currentExp;

    public float levelBuff;

    public float LevelMultiplier
    {
        get{ return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;

        // 判断升级
        if (currentExp >= levelUpExp)
            LevelUp();
    }

    private void LevelUp()
    {
        // 所有想提升的属性数值都写在这里

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        levelUpExp += (int)(levelUpExp * LevelMultiplier);  // 设置逐级升级的经验值

        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;

        Debug.Log("Level Up!" + currentLevel + "Max Health:" + maxHealth);
    }
}

//ScriptableObject 是 Unity 中一个非常有用的类，用于创建轻量级、可序列化的数据容器，不依赖场景中的 GameObject。它非常适合存储配置数据、游戏设定、角色属性、技能信息等。