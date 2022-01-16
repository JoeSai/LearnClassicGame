using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单例模式基类模块
//作用 ： 减少单例模式重复代码的书写

// 1. C# 泛型 、 泛型约束
// 2. 设计模式 单例模式
public class BaseManager<T> where T : new()
{
    private static T instance;

    public static T GetInstance()
    {
        //多线程要加双锁
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}


//public class GameManager
//{
//    private static GameManager instance;

//    public static GameManager GetInstance()
//    {
//        if(instance == null)
//        {
//            instance = new GameManager();
//        }
//        return instance;
//    }
//}

//public class GameManager : BaseManager<GameManager>
//{

//}
