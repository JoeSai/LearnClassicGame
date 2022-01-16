using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托 、lambda
/// 3.协程
/// 4.泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载
    public T Load<T>(string name)where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是GameObject类型， 实例化后再返回出去 ，外部直接使用
        if(res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else //TestAsset 、AudioClip ...
        {
            return res;
        }
    }


    //异步加载
    public void LoadAsync<T>(string name , UnityAction<T> callback) where T : Object
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name , callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
        
        if(r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
