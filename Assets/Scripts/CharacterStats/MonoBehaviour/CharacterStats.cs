using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;


    void Awake()
    {
        if (templateData != null) characterData = Instantiate(templateData);
        // 从 CharacterData_SO templateData 中 复制出一份 （Instantiate 一份）出来，赋给  characterData 
        // 即,使 characterData 变成 CharacterData_SO templateData 的一个副本，而不直接使用 CharacterData_SO
    }



    #region Read from Data_SO
    public int MaxHealth
    {
        // use {get; set;} to pull or set figure

        get { if (characterData != null) return characterData.maxHealth; else return 0; }

        set { characterData.maxHealth = value; }

    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStats attacker, CharacterStats defender) //新建的一个函数， 在 PlayerController 和 EnemyController 中有引用
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0); // 若防大于攻，确保掉血值不会小于0
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0); // Current Health 不会小于0

        if (attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit"); // 这里的 Hit 关键字，在 Player 和 Enemy 的动画机里，关键字均为“Hit”。所以在总控里可以一步解决。
        }
        // : Update UI : 
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);  // .? 判断不为空
        // : 经验 Update
        if (CurrentHealth <= 0)
            // attacker.characterData.UpdateExp(characterData.killpoint);
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killpoint);
    }

    public void TakeDamage(int damage, CharacterStats defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);

        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killpoint);
            // 确保石头击败石头人也能拿到经验值
    }

    private int CurrentDamage() // 由 TakeDamage 引用的一个计算性函数
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }
        return (int)coreDamage;
    }

    #endregion




}
