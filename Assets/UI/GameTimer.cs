using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float gameLength = 60f; // Czas startowy w sekundach
    private float currentTime; // Aktualny czas
    public TextMeshProUGUI timerText; // Tekst na UI
    public GameOverManager gameOverManager; // Referencja do GameOverManager
    public GameObject playerWin;

    private void Start()
    {
        currentTime = gameLength; // Poczatkowe ustawienie czasu
        UpdateTimerUI();
    }


    private void Update()
    {
            if (currentTime > 0) 
        { 
            currentTime -= Time.deltaTime; // Odliczanie czasu
            currentTime = Mathf.Clamp(currentTime, 0, gameLength);

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
        gameOverManager.ShowGameOver(); // Pojawienie si� ekranu Game Over
        playerWin.SetActive(true);
    }
}
