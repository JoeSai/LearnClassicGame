using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _birdDiedEvent = default;
    [SerializeField] private VoidEventChannelSO _startGameEvent = default;
    
    private Text highScoreText;
    private Text scoreText;
    // Start is called before the first frame update
    private void Awake()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        highScoreText = transform.Find("highScoreText").GetComponent<Text>();
        
                
        _birdDiedEvent.OnEventRaised += Hide;
        _startGameEvent.OnEventRaised += Show;
        Hide();
    }

    void Start()
    {
        highScoreText.text = "HIGHSCORE:" + Score.GetHighScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Level.GetInstance().GetPipesPassed().ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    
    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        _birdDiedEvent.OnEventRaised -= Hide;
        _startGameEvent.OnEventRaised -= Show;
    }
}
