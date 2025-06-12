using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SplatterHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private string enemyType;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] bool HasDeathAnimation;
    public int scoreValue = 10;
    public int expValue = 10;
    private Animator animator;

    [Header("Splatter Settings")]
    [SerializeField] private GameObject damageZonePrefab;
    [SerializeField] private float stainLifetime = 10f;
    [SerializeField] private float moveSpeedMultiplier = 0.5f;
    [SerializeField] private float rotationSpeedMultiplier = 0.5f;

    private EnemyManager enemyManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = FindAnyObjectByType<EnemyManager>();

        if (enemyManager == null)
        {
            Debug.LogWarning("EnemyManager not found! Defaulting maxHealth to 100.");
        }

        SetEnemyType(enemyType);
    }

    // Ustawia typ przeciwnika i jego bazowe zdrowie
    public void SetEnemyType(string type)
    {
        enemyType = type;

        if (enemyManager != null)
        {
            maxHealth = enemyManager.GetEnemyHealth(enemyType);
        }

        currentHealth = maxHealth;
    }

    // Otrzymywanie obrazen
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " received damage: " + damage);

        if (currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().isStopped = true;

            if (HasDeathAnimation)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                Die();
            }
        }
    }

    // Smierc przeciwnika
    private void Die()
    {
        Debug.Log("Splatter dead!");

        if (damageZonePrefab != null)
        {
            Vector3 spawnPosition = transform.position;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPosition = hit.point;
            }

            GameObject splatter = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
            splatter.AddComponent<DamageZoneHandler>().Initialize(stainLifetime, moveSpeedMultiplier, rotationSpeedMultiplier);
        }

        ScoreManager.Instance.AddPoints(scoreValue);
        FindObjectOfType<PlayerExperience>()?.AddExperience(expValue);
        Destroy(gameObject);
    }
}