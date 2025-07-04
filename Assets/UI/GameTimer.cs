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
    public BossEvents bossEvents; // Referencja do BossEvents
    private bool bossFightStarted = false; // Czy walka z bossem ju� si� rozpocz�a

    private void Start()
    {
        currentTime = gameLength; // Pocz�tkowe ustawienie czasu
        UpdateTimerUI(); // Aktualizacja UI na starcie
    }

    private void Update()
    {
        if (!bossFightStarted) // Je�li nie rozpocz�to walki z bossem
        {
            currentTime -= Time.deltaTime; // Odliczanie czasu
            currentTime = Mathf.Clamp(currentTime, 0, gameLength); // Upewnienie si�, �e czas nie spada poni�ej 0

            UpdateTimerUI(); // Aktualizacja tekstu na UI

            if (currentTime <= 0 && !bossFightStarted) // Sprawdzenie czy czas si� sko�czy� i walka z bossem si� nie rozpocz�a
            {
                TimerEnded(); // Rozpocz�cie walki z bossem
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
        currentTime = 0; // Ustawienie na r�wno 0
        UpdateTimerUI(); // Upewnienie si�, �e pokazuje 0:00

        Debug.Log("Czas min��! Pojawia si� Behemoth."); // Log na potrzeby debugowania
        bossEvents.StartEvents(); // Uruchomienie zdarze� Bossa
        bossFightStarted = true; // Ustawienie flagi, �e walka trwa
    }
}
