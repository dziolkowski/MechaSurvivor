using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private string enemyType; // The type of the enemy
    [SerializeField] private int maxHealth = 100; // Max health of the enemy
    [SerializeField] public int currentHealth;
    [SerializeField] private bool hasDeathAnimation; // Toggle for death animation

    public int scoreValue = 10;
    private Animator animator;
    private EnemyManager enemyManager;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Find the EnemyManager instance in the scene
        enemyManager = FindObjectOfType<EnemyManager>();
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
        Debug.Log($"Enemy type set to {enemyType} with max health {maxHealth}.");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<CapsuleCollider>().enabled = false; // Disable collider to prevent additional damage
        GetComponent<NavMeshAgent>().isStopped = true; // Stop enemy movement

        if (hasDeathAnimation)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            PerformDeath();
        }
    }

    private void PerformDeath()
    {
        // Add player score and destroy the game object
        ScoreManager.Instance.AddPoints(scoreValue);
        Destroy(gameObject);
        Debug.Log($"{gameObject.name} has died and is destroyed.");
    }
}