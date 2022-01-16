using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// 1.场景异步加载
/// 2.协程
/// 3.委托
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 同步切换场景
    /// </summary>
    /// <param name="name"></param>
    public void LoadSecne(string name , UnityAction fun)
    {
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成后，才会去执行
        //fun();
        if (fun != null)
        {
            fun();
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSecneAsync(string name , UnityAction fun)
    {
        // UIManager.GetInstance().ShowPanel<LevelLoaderPanel>("LevelLoaderPanel", E_UI_Layer.System, (LevelLoaderPanel) => {
        //     LevelLoaderPanel.transition.SetTrigger("Start");
        //
        //     MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsync(name, fun));
        // });
    }

    private IEnumerator ReallyLoadSceneAsync(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        // 0 - 1 , 场景加载的进度
        //ao.progress
        while (!ao.isDone)
        {
            //EventCenter
            //在这里更新进度条
            yield return ao.progress;
        }

        //加载完成后才去执行fun
        //fun();
        if (fun != null)
        {
            fun();
        }

        // UIManager.GetInstance().ShowPanel<LevelLoaderPanel>("LevelLoaderPanel", E_UI_Layer.System, (LevelLoaderPanel) => {
        //     LevelLoaderPanel.transition.SetTrigger("End");
        // });

    }
}
