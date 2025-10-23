using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void Save(Object data, string key)  // object 类是所有unity可引用的类的基类 【 unity 所有类的基类 】
    {

        // ToJson 函数 让变量变成 string 类型的字符串，存储在系统当中
        // JsonUtility 是用来在对象和字符串之间进行转换的工具，方便你把复杂的数据结构变成字符串保存，或从字符串恢复成对象。 *****
        // Json 用来变字符串， PlayerPrefs 用来写入硬盘
        var jsonData = JsonUtility.ToJson(data, true); // 后面那个 true 是 prettyprint, 可要可不要，久石让本地可视的数据文件更美观


        // PlayerPrefs  Unity 自带的存储数据的方法，在硬盘上产生文件数据
        // PlayerPrefs is a class that stores Player preferences between game sessions. It can store string, float and integer values into the user’s platform registry.
        PlayerPrefs.SetString(key, jsonData); // SetString	Sets a single string value for the preference identified by the given key. You can use PlayerPrefs.GetString to retrieve this value.
        PlayerPrefs.Save(); // Saves all modified preferences.
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);

        }
    }
/*
对象 → JSON 字符串：用 JsonUtility.ToJson()。
JSON 字符串 → 本地硬盘：用 PlayerPrefs.SetString()。
本地硬盘 → JSON 字符串：用 PlayerPrefs.GetString()。
JSON 字符串 → 对象：用 JsonUtility.FromJsonOverwrite()。
*/


}


/*
public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlyerData();
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlyerData();
        }
    }
    
    public void SavePlyerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void LoadPlyerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void Save(Object data, string key)  // object 类是所有unity可引用的类的基类 【 unity 所有类的基类 】
    {

        var jsonData = JsonUtility.ToJson(data, true); // 后面那个 true 是 prettyprint, 可要可不要，久石让本地可视的数据文件更美观
        PlayerPrefs.SetString(key, jsonData); // SetString	Sets a single string value for the preference identified by the given key. You can use PlayerPrefs.GetString to retrieve this value.
        PlayerPrefs.Save(); // Saves all modified preferences.
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))    
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        
        }
    }
    */