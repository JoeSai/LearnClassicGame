using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static Loader _instance;

    public static Loader GetInstance()
    {
        return _instance;
    }
    public enum Scene
    {
        GameScene,
        Main,
    }

    [SerializeField] private Animator _transition;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Load(Scene scene)
    {
        StartCoroutine(LoadLevel(scene));
    }
    

    IEnumerator LoadLevel(Scene scene)
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.3f);         //play button sound
        AsyncOperation temp = SceneManager.LoadSceneAsync(scene.ToString());
        yield return temp;
        _transition.SetTrigger("End");
    }
}
