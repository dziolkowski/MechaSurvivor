using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SplatterHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private string enemyType; // The type of the enemy
    [SerializeField] private int maxHealth = 100; // Max health of the enemy
    [SerializeField] public int currentHealth; // Current health of the enemy
    [SerializeField] private bool hasDeathAnimation; // Toggle for death animation
    public int scoreValue = 10; // Points awarded for killing the enemy

    private Animator animator;
    private EnemyManager enemyManager;

    [Header("Splatter Settings")]
    [SerializeField] private GameObject damageZonePrefab; // Prefab plamy
    [SerializeField] private float stainLifetime = 10f; // Czas, po ktorym plama znika
    [SerializeField] private float moveSpeedMultiplier = 0.5f; // Spowolnienie ruchu 
    [SerializeField] private float rotationSpeedMultiplier = 0.5f; // Spowolnienie oborotu

    private void Start()
    {
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
        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        // Disable collider and stop movement
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().isStopped = true;

        // Always spawn splatter effect and award points, regardless of animation
        PerformSplatterEffect();
        ScoreManager.Instance.AddPoints(scoreValue);

        // Trigger animation if applicable, then destroy the enemy
        if (hasDeathAnimation)
        {
            animator.SetTrigger("Death");
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PerformSplatterEffect()
    {
        if (damageZonePrefab != null)
        {
            Vector3 spawnPosition = transform.position;

            // Adjust the spawn position to the ground
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPosition = hit.point;
            }

            GameObject splatter = Instantiate(damageZonePrefab, spawnPosition, Quaternion.identity);
            splatter.AddComponent<DamageZoneHandler>().Initialize(stainLifetime, moveSpeedMultiplier, rotationSpeedMultiplier);
        }

        Debug.Log("Splatter effect applied.");
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the animation to finish before destroying the object
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}