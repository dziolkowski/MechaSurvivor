using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;

    public int scoreValue = 10;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        print(gameObject + " damage received!");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        print("dead");
        ScoreManager.Instance.AddPoints(scoreValue); // Dodaje punkty po smierci przeciwnika
        Destroy(gameObject); // Niszczenie przeciwnika po smierci
    }
}
