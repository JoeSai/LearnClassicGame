using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//继承MonoBehaviour的单例模式基类
//需要保证场景上只挂在一个脚本

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //继承了Mono的基类 不能直接new
        //只能通过拖动到对象上 或者 AddComponent加脚本
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
