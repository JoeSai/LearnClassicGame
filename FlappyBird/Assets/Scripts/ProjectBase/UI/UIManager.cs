
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}
/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
     
    //暴露外部使用
    public RectTransform canvas;
    public UIManager()
    {
        GameObject obj = GameObject.Find("Canvas");
        if(obj == null)
        {
            obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        }
        // 创建Canvas 和 EventSystem ， 过场景不被移除
        canvas = obj.transform as RectTransform;
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");
        GameObject.DontDestroyOnLoad(obj);

        GameObject evs = GameObject.Find("EventSystem");
        if (evs == null)
        {
            evs = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        }
        GameObject.DontDestroyOnLoad(evs);

        Debug.Log(bot);
        Debug.Log(mid);
        Debug.Log(top);
        Debug.Log(system);


        GetDefaultPanel();
    }

    public void GetDefaultPanel()
    {
        for (int i = 0; i < system.childCount; i++)
        {
            panelDic.Add(system.GetChild(i).transform.name, system.GetChild(i).GetComponent<BasePanel>());
        }
   
        //ShowPanel<SingingTextPanel>("SingingTextPanel", E_UI_Layer.System, (panel) => { 
        //    panel.gameObject.SetActive(false); 
        //});
        //ShowPanel<StorylinePanel>("StorylinePanel", E_UI_Layer.System, (panel) => {
        //    panel.gameObject.SetActive(false);
        //});
        //ShowPanel<GetItemMessagePanel>("GetItemMessagePanel", E_UI_Layer.System, (panel) => { });
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示层级</param>
    /// <param name="callBack">面板创建成功后，想做的事</param>
    public void ShowPanel<T>(string panelName , E_UI_Layer layer = E_UI_Layer.Mid ,UnityAction<T> callBack = null) where T:BasePanel
    {
        //已经显示 直接回调
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            return;
        }


        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            // 作为Canvas的子对象
            // 设置相对位置？
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            obj.name = panelName;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            // 得到预制体本身上的面板脚本
            T panel = obj.GetComponent<T>();
            // 处理面板创建完成后的逻辑
            if(callBack != null)
            {
                callBack(panel);
            }
            panel.ShowMe();


            //把面板脚本存起来
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 隐藏面板 ，销毁
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板
    /// </summary>
    /// <param name=""></param>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }

    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件相应函数</param>
    public static void AddCustomeEventListener(UIBehaviour control , EventTriggerType type ,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
