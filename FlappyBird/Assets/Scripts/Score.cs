using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Score
{
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }

    public static bool TrySetNewHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("highScore" , score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highScore",0);
        PlayerPrefs.Save();
    }
}
