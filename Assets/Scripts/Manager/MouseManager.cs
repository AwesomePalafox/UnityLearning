using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.Events;


// [System.Serializable]
// public class EventVector3 : UnityEvent<Vector3> { }
public class MouseManager : Singleton<MouseManager>
{

    public Texture2D pointto, doorway, attack, target, arrownormal;
    
    RaycastHit hitInfo;

    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked;

    protected override void Awake()
    {
        base.Awake();                       // 基于原有父类的函数方法 额外运行 即保留 Singleton 中的 Awake 里的所有运行
        // DontDestroyOnLoad(this);
    }

    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            // 切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack,new Vector2(0,0),CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
        }
    }



}
