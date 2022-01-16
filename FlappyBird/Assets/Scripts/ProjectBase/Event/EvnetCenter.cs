using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// �¼�����ģ��
// 1.Dictionary
// 2.ί��
// 3.�۲������ģʽ

// �����滻ԭ�� : ��������滻����
// ���ÿսӿڰ�����
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
    /// ����¼�����
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">���������¼���ί�к���</param>
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
    /// ��������Ҫ���ݲ������¼�
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
    /// �¼��Ƴ� �� ������OnDestroy�ǵ���� �� �����ڴ�й©
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼�֮ǰ��ӵ�ί�к���</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// �Ƴ�����Ҫ���ݲ������¼�
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
    /// �¼�����
    /// </summary>
    /// <param name="name">�������¼���</param>
    public void EventTrigger<T>(string name , T obj)
    {
        if (eventDir.ContainsKey(name))
        {
            (eventDir[name] as EventInfo<T>).actions?.Invoke(obj);
        }
        //eventDir[name]?.Invoke();
    }
    /// <summary>
    /// �¼��������޲�����
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
    /// ����¼�����
    /// �����л�ʱ���� ���Է���һ
    /// </summary>
    public void Clear()
    {
        eventDir.Clear();
    }


}
