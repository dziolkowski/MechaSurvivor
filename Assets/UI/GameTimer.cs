using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float startTime = 60f; // Czas startowy w sekundach
    private float currentTime; // Aktualny czas
    public TextMeshProUGUI timerText; // Tekst na UI
    public GameOverManager gameOverManager; // Referencja do GameOverManager

    private void Start()
    {
        currentTime = startTime; // Poczatkowe ustawienie czasu
        UpdateTimerUI();
    }


    private void Update()
    {
            if (currentTime > 0) 
        { 
            currentTime -= Time.deltaTime; // Odliczanie czasu
            currentTime = Mathf.Clamp(currentTime, 0, startTime);

            UpdateTimerUI(); // Aktualizacja tekstu na UI

            if(currentTime <= 0) // Sprawdzanie czy czas sie skonczyl 
            {
                TimerEnded();
            }
        }        
    }


    private void UpdateTimerUI() 
    { 
        int minutes = Mathf.FloorToInt(currentTime / 60); // Formatowanie czasu na minuty i sekundy
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = $"{minutes:D2}:{seconds:D2}"; // Aktualizacja tekstu
    }


    private void TimerEnded() 
    {
        Debug.Log("Game Over!");
        gameOverManager.ShowGameOver(); // Pojawienie siê ekranu Game Over
    }
}
