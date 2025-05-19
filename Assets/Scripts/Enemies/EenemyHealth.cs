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
        //Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false; // wylaczenie collidera aby nie zadawac graczowi obrazen /L
            GetComponent<NavMeshAgent>().isStopped = true; // zatrzymanie przeciwnika w momencie kiedy ma 0 HP /L
            // WORKAROUND - USUNAC POZNIEJ /L
            if (hasDeathAnimation) { // jesli ma anmacje smierci, to Die() zostanie wywolana po animacji
                animator.SetTrigger("Death");
                print("kaboom");
            }
            else {
                Debug.Log("skipping animation");
                Die(); // jesli nie ma animacji smierci to wywoluje Die()
            }
        }
    }

    private void Die() {
        // Add player score and destroy the game object
        ScoreManager.Instance.AddPoints(scoreValue);
        Destroy(gameObject);
        //Debug.Log($"{gameObject.name} has died and is destroyed.");
    }
}