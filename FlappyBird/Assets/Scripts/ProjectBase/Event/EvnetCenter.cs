using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 事件中心模块
// 1.Dictionary
// 2.委托
// 3.观察者设计模式

// 里氏替换原则 : 子类可以替换父类
// 利用空接口包裹类
public interface IEventInfo
{
    
}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter : BaseManager<EventCenter>
{
    private Dictionary<string, IEventInfo> eventDir = new Dictionary<string, IEventInfo>();
    
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">用来处理事件的委托函数</param>
    public void AddEventListener<T>(string name , UnityAction<T> action)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDir.Add(name, new EventInfo<T>(action));
        }
    }
    /// <summary>
    /// 监听不需要传递参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo).actions += action;
        }
        else
        {
            eventDir.Add(name, new EventInfo(action));
        }
    }
    /// <summary>
    /// 事件移除 ， 监听者OnDestroy记得清除 ， 避免内存泄漏
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">事件之前添加的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// 移除不需要传递参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">触发的事件名</param>
    public void EventTrigger<T>(string name , T obj)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo<T>).actions?.Invoke(obj);
        }
        //eventDir[name]?.Invoke();
    }
    /// <summary>
    /// 事件触发（无参数）
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo).actions?.Invoke();
        }
        //eventDir[name]?.Invoke();
    }

    /// <summary>
    /// 清空事件中心
    /// 场景切换时调用 ，以防万一
    /// </summary>
    public void Clear()
    {
        eventDir.Clear();
    }


}
