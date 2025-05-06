using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UıController : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}