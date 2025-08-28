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

}

//ScriptableObject 是 Unity 中一个非常有用的类，用于创建轻量级、可序列化的数据容器，不依赖场景中的 GameObject。它非常适合存储配置数据、游戏设定、角色属性、技能信息等。