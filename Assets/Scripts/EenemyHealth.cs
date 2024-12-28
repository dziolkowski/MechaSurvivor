using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;

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
        // Niszczenie przeciwnika po œmierci
        Destroy(gameObject); 
    }
}
