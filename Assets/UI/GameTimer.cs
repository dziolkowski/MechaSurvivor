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
    private bool bossFightStarted = false; // Czy walka z bossem ju¿ siê rozpoczê³a

    private void Start()
    {
        currentTime = gameLength; // Pocz¹tkowe ustawienie czasu
        UpdateTimerUI(); // Aktualizacja UI na starcie
    }

    private void Update()
    {
        if (!bossFightStarted) // Jeœli nie rozpoczêto walki z bossem
        {
            currentTime -= Time.deltaTime; // Odliczanie czasu
            currentTime = Mathf.Clamp(currentTime, 0, gameLength); // Upewnienie siê, ¿e czas nie spada poni¿ej 0

            UpdateTimerUI(); // Aktualizacja tekstu na UI

            if (currentTime <= 0 && !bossFightStarted) // Sprawdzenie czy czas siê skoñczy³ i walka z bossem siê nie rozpoczê³a
            {
                TimerEnded(); // Rozpoczêcie walki z bossem
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
        currentTime = 0; // Ustawienie na równo 0
        UpdateTimerUI(); // Upewnienie siê, ¿e pokazuje 0:00

        Debug.Log("Czas min¹³! Pojawia siê Behemoth."); // Log na potrzeby debugowania
        bossEvents.StartEvents(); // Uruchomienie zdarzeñ Bossa
        bossFightStarted = true; // Ustawienie flagi, ¿e walka trwa
    }
}
