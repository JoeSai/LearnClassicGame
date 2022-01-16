using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _birdDiedEvent = default;
    
    private Text _scoreText;
    private Text _highScoreText;
    private Button _retryButton;
    private Button _mainButton;
    // Start is called before the first frame update
    private void Awake()
    {
        _scoreText = transform.Find("scoreText").GetComponent<Text>();
        _highScoreText = transform.Find("highScoreText").GetComponent<Text>();
        _retryButton = transform.Find("retryButton").GetComponent<Button>();
        _mainButton = transform.Find("mainButton").GetComponent<Button>();
        _retryButton.onClick.AddListener(() =>
        {
            Loader.GetInstance().Load(Loader.Scene.GameScene);
        });
        
        _mainButton.onClick.AddListener(() =>
        {
            Loader.GetInstance().Load(Loader.Scene.Main);
        });
    }

    void Start()
    {
        _birdDiedEvent.OnEventRaised += OnGameOver;
        Hide();
    }
    
    private void OnDestroy()
    {
        _birdDiedEvent.OnEventRaised -= OnGameOver;
    }

    private void OnGameOver()
    {
        _scoreText.text = Level.GetInstance().GetPipesPassed().ToString();
        _highScoreText.text = "HIGHSCORE:" + Score.GetHighScore().ToString();
        Show();
   
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
