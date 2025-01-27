using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public GameOverManager gameOverManager; // Referencja do GameOverManager

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player takes damage. Current health: " + health);

        if (health <= 0)
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
}
