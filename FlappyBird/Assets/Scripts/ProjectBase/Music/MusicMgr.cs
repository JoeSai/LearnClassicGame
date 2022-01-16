using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 2D音效管理 ，没有远小近大的效果
/// </summary>
/// 


public class MusicMgr : BaseManager<MusicMgr>
{
    private const string MusicPath = "Audio/Music/";
    private const string SoundPath = "Audio/Sound/";

    private AudioSource bgMusic = null;
    private float bgVolume = 1;

    private float soundVolue = 1;
    private GameObject soundObj = null;
    private List<AudioSource> soundList = new List<AudioSource>();

    public MusicMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        for(int i = soundList.Count - 1; i >= 0; i--)
        {
            if (soundList[i] == null)
            {
                soundList.RemoveAt(i);
                return;
            }
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBgMusic(string name)
    {
        if(bgMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BgMusic";
            bgMusic = obj.AddComponent<AudioSource>();
        }

        // 异步加载背景音乐 然后播放
        ResMgr.GetInstance().LoadAsync<AudioClip>(MusicPath + name, (clip) => {
            bgMusic.clip = clip;
            bgMusic.volume = bgVolume;
            bgMusic.loop = true;
            bgMusic.Play();
        });
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBgMusic()
    {
        if (bgMusic == null)
        {
            return;
        }
        bgMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBgMusic()
    {
        if(bgMusic == null)
        {
            return;
        }
        bgMusic.Stop();
    }
    /// <summary>
    /// 改变背景音乐音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBgMusicVolum(float v)
    {
        bgVolume = v;
        if(bgMusic == null)
        {
            return;
        }
        else
        {
            bgMusic.volume = bgVolume;
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name, bool isLoop = false, UnityAction<AudioSource> callback = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        AudioSource source = soundObj.AddComponent<AudioSource>();

        ResMgr.GetInstance().LoadAsync<AudioClip>(SoundPath + name, (clip) => {
            source.clip = clip;
            source.volume = soundVolue;
            source.loop = isLoop;
            source.Play();

            soundList.Add(source);

            //如果需要手动控制 ，使用回调
            if (callback != null)
            {
                callback(source);
            }
        });
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="source"></param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 改变所有音效的音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundVolum(float v)
    {
        soundVolue = v;
        foreach (var sound in soundList)
        {
            sound.volume = soundVolue;
        }
    }


}
