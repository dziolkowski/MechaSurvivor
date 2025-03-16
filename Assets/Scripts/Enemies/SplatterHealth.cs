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
            Vector3 spawnPosition = transform.position; // Domyslna pozycja (w razie braku podlogi)

            // Raycast w dol, aby znalezc rzeczywista podloge
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPosition = hit.point; // Ustawiamy pozycje na podloge
            }

            Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
        }

        ScoreManager.Instance.AddPoints(scoreValue);
        Destroy(gameObject);
    }
}
