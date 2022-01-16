using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static GameAssets GetInstance()
    {
        return _instance;
    }
    
    public Sprite pipeHeaSprite;
    public Transform pipeBody;
    public Transform pipeHead;
    public Transform ground;
    public Transform[] clouds;

}
