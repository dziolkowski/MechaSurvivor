using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Latwy dostep do punktacji
    public TextMeshProUGUI scoreText; // Punkty wyswietlane na UI
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void AddPoints(int points) // Dodaje punkty i aktualizuje UI
    {
        score += points;
        UpdateScoreUI();
    }


    void UpdateScoreUI() // Aktualizuje tekst na ekranie
    {
        if (scoreText != null)
        {
            scoreText.text = "Points: " + score.ToString();
        }
    }
}

