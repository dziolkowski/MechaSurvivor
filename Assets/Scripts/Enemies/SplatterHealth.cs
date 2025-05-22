using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SplatterHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private string enemyType; // The type of the enemy
    [SerializeField] private int maxHealth = 100; // Maksymalne zdrowie
    [SerializeField] public int currentHealth; // Aktualne zdrowie
    [SerializeField] bool HasDeathAnimation; // tymczasowe rozwiazanie dla przeciwnikow bez animacji smierci aby poprawnie umierali
    public int scoreValue = 10; // Punkty otrzymywane za zabicie przeciwnika
    public int expValue = 10; // Exp otrzymywany za zabicie przeciwnika
    Animator animator;

    [Header("Splatter Settings")]
    [SerializeField] private GameObject damageZonePrefab; // Prefab plamy
    [SerializeField] private float stainLifetime = 10f; // Czas, po ktorym plama znika
    [SerializeField] private float moveSpeedMultiplier = 0.5f; // Spowolnienie ruchu 
    [SerializeField] private float rotationSpeedMultiplier = 0.5f; // Spowolnienie oborotu

    private EnemyManager enemyManager;

    private void Start() {
        animator = GetComponent<Animator>();

        // Find the EnemyManager instance in the scene
        enemyManager = FindAnyObjectByType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogWarning("EnemyManager not found! Defaulting maxHealth to 100.");
        }

        // Initialize max health and current health based on the enemy type
        SetEnemyType(enemyType);
    }

    /// <summary>
    /// Sets the enemy's type and updates its max health based on EnemyManager's settings.
    /// </summary>
    /// <param name="type">The enemy type to set.</param>
    public void SetEnemyType(string type)
    {
        enemyType = type;

        // Get the corresponding max health value from the EnemyManager
        if (enemyManager != null)
        {
            maxHealth = enemyManager.GetEnemyHealth(enemyType);
        }

        currentHealth = maxHealth; // Initialize current health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject + " damage received!");
        if (currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false; // wylaczenie collidera aby nie zadawac graczowi obrazen /L
            gameObject.GetComponent<NavMeshAgent>().isStopped = true; // zatrzymanie przeciwnika w momencie kiedy ma 0 HP /L
            // WORKAROUND - USUNAC POZNIEJ /L
            if (HasDeathAnimation) { // jesli ma anmacje smierci, to Die() zostanie wywolana po animacji
                animator.SetTrigger("Death");
            }
            else Die(); // jesli nie ma animacji smierci to wywoluje Die()
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
            splatter.AddComponent<DamageZoneHandler>().Initialize(stainLifetime, moveSpeedMultiplier, rotationSpeedMultiplier);
        }

        ScoreManager.Instance.AddPoints(scoreValue); // Dodaje punkty po smierci przeciwnika
        FindObjectOfType<PlayerExperience>().AddExperience(expValue); // Dostajemy exp za punkty
        Destroy(gameObject);
    }
}