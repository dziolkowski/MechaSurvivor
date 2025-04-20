using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth; // Aktualne zdrowie gracza
    public int maxHealth = 100; // Maksymalne zdrowie gracza
    public GameOverManager gameOverManager; // Referencja do GameOverManager
    public Slider healthSlider; // Suwak pokazujacy stan zdrowia
    public TextMeshProUGUI healthText; // Tekst pokazujacy wartosc zdrowia

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); // Ustawinie suwaka na poczatkowe wartosci
    }

    public void TakeDamage(int damage)
    {
        PlayerShield shield = GetComponent<PlayerShield>();
        if (shield != null && shield.AbsorbDamage(damage))
        {            
            return;
        }

        currentHealth -= damage;
        Debug.Log("Player takes damage. Current health: " + currentHealth);

        UpdateHealthUI(); // Aktualizacja po zadaniu obrazen

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead!");
        gameOverManager.ShowGameOver(); // Wywolaj Game Over
        gameObject.SetActive(false); // Ukryj gracza
    }


    private void UpdateHealthUI() 
    { 
        healthSlider.value = (float)currentHealth / maxHealth; // Ustawienie wartosci suwaka na procent zdrowia
        healthText.text = $"{currentHealth}/{maxHealth}"; // Aktualizacja tekstu pokazujacego stan zdrowia
    }
}
