using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UıController : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject GameWinText;

    public void SetScore(int score,int goalScore)
    {
        scoreText.text = $"Score: {score}/{goalScore}";
    }
    
    public void GameOver()
    {
        GameOverText.SetActive(true);
    }
    public void GameWin()
    {
        GameWinText.SetActive(true);
    }
}