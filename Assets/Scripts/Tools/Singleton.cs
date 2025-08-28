using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> // 通常的 泛型单例的写法
                                                                 // T 是类型参数：它代表一个占位符类型，在使用时由具体类型替代
                                                                 // <> 是泛型类型参数的语法容器
                                                                 //在定义时写 <T>，在使用时写 <int>、<string> 等具体类型。
                                                                 // Singleton<T> 是一个泛型类，T 是你希望单例管理的类型。
                                                                 // where T : Singleton<T> 这是一个 泛型约束，意思是：类型参数 T 必须继承自 Singleton<T>。这个约束的作用是确保类型 T 与泛型类 Singleton<T> 
                                                                 // 之间形成一种“自引用”的关系，也叫 Curiously Recurring Template Pattern（CRTP），在 C# 中用于实现类型安全的泛型继承。
                                                                 // 这种写法的目的是为了让每个继承 Singleton<T> 的类都能拥有自己的单例实例，并且类型安全。

{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

                                                                    // 允许外部访问此 Private 单例模式  Get： 读入  （Set 写入）

    protected virtual void Awake()                                  // protected 表示这个 方法 只能在当前类或其子类中访问   virtual 允许子类重写这个方法
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;                                        // (T)this 是类型转换，把当前对象转换为泛型类型 T。
    }

                                                                // 泛型方式可以复用这个单例逻辑到多个类，比如 GameManager、AudioManager 等。

    public static bool IsInitialized
    {
        get { return instance != null; }
    }
                                                                    // 这样可以避免访问 Instance 时触发自动创建，或者避免访问一个尚未初始化的单例。


    protected virtual void OnDestroy()                              // 在对象销毁时清理单例引用，防止内存泄漏或错误引用。
    {

        if (instance == this)
        {
            instance = null;
        }

                                                                    // OnDestroy() 是 Unity 生命周期方法之一 当挂载该脚本的 GameObject 被销毁时（比如场景切换或手动销毁），Unity 会自动调用这个方法
                                                                    // if (instance == this)  检查当前对象是否就是单例实例。 
                                                                    // 如果不清空 instance，即使对象已经销毁，Instance 仍然指向一个无效对象，可能导致运行时错误。
    }

}
