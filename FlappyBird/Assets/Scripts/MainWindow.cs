using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : MonoBehaviour
{
    private Button _playButton;
    private Button _quietButton;
    // Start is called before the first frame update
    private void Awake()
    {
        _playButton = transform.Find("playButton").GetComponent<Button>();
        _quietButton = transform.Find("quietButton").GetComponent<Button>();
        
        _playButton.onClick.AddListener(() =>
        {
            Loader.GetInstance().Load(Loader.Scene.GameScene);
        });
        _quietButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
