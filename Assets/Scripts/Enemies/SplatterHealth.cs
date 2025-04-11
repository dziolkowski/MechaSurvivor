using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Maksymalne zdrowie
    [SerializeField] public int currentHealth; // Aktualne zdrowie
    public int scoreValue = 10; // Punkty otrzymywane za zabicie przeciwnika

    [Header("Splatter Settings")]
    [SerializeField] private GameObject damageZonePrefab; // Prefab plamy
    [SerializeField] private float splatterLifetime = 10f; // Czas, po ktorym plama znika
    [SerializeField] private float moveSpeedMultiplier = 0.5f; // Spowolnienie ruchu 
    [SerializeField] private float rotationSpeedMultiplier = 0.5f; // Spowolnienie oborotu

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject + " damage received!");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy dead!");

        if (damageZonePrefab != null)
        {
            Vector3 spawnPosition = transform.position;

            // Plama plama pojawia sie w miejscu smierci Splattera ale na podlodze
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPosition = hit.point;
            }

            GameObject splatter = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
            splatter.AddComponent<DamageZoneHandler>().Initialize(splatterLifetime, moveSpeedMultiplier, rotationSpeedMultiplier);
        }

        ScoreManager.Instance.AddPoints(scoreValue); 
        Destroy(gameObject);
    }
}
