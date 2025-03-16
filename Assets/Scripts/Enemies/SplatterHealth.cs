using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] private GameObject damageZonePrefab; // Prefab plamy
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
        print("Enemy dead!");

        if (damageZonePrefab != null)
        {
            Vector3 spawnPosition = transform.position; // Domyœlna pozycja (w razie braku pod³ogi)

            // Raycast w dó³, aby znaleŸæ rzeczywist¹ pod³ogê
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPosition = hit.point; // Ustawiamy pozycjê na pod³ogê
            }

            Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
        }

        ScoreManager.Instance.AddPoints(scoreValue);
        Destroy(gameObject);
    }
}
