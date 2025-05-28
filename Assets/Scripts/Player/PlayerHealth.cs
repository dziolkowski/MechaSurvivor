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
        UpdateHealthUI(); // Ustawienie suwaka na poczatkowe wartosci

        // Subskrybujemy event upgradu
        PlayerUpgradeTracker.OnUpgradeApplied += OnUpgradeApplied;
    }

    private void OnDestroy()
    {
        // Odsubskrybujemy event zeby uniknac bledu po zniszczeniu obiektu
        PlayerUpgradeTracker.OnUpgradeApplied -= OnUpgradeApplied;
    }

    private void OnUpgradeApplied(StatUpgradeData upgrade)
    {
        // Sprawdzamy czy upgrade dotyczy MaxHealth
        if (upgrade.isPlayerUpgrade && upgrade.statType == StatType.MaxHealth)
        {
            int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
            float value = upgrade.GetValueAtLevel(level - 1); // Pobieramy wartosc z poziomu upgradu
            IncreaseMaxHealth(value);
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        // Zwiekszamy maxHealth o wartosc upgradu
        maxHealth += Mathf.RoundToInt(amount);
        currentHealth += Mathf.RoundToInt(amount); // Opcjonalnie dodajemy tez do aktualnego zdrowia

        Debug.Log($"Zwiekszono max health o {amount}. Nowe max: {maxHealth}");
        UpdateHealthUI(); // Aktualizujemy UI po zmianie zdrowia
    }

    public void TakeDamage(int damage)
    {
        PlayerShield shield = GetComponent<PlayerShield>();
        if (shield != null && shield.AbsorbDamage(damage))
        {
            return;
        }

        currentHealth -= damage;
        //Debug.Log("Player takes damage. Current health: " + currentHealth);

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
