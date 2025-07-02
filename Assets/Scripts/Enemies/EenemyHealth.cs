using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private string enemyType;
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private bool hasDeathAnimation;

    public int scoreValue = 10;
    public int expValue = 10;

    private Animator animator;
    private EnemyManager enemyManager;
    private bool isDead = false;
    private NavMeshAgent navAgent;
    
    //flash sprite on damage
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = FindAnyObjectByType<EnemyManager>();
        navAgent = GetComponent<NavMeshAgent>();

        SetEnemyType(enemyType);
    }

    public void SetEnemyType(string type)
    {
        enemyType = type;

        if (enemyManager != null)
        {
            maxHealth = enemyManager.GetEnemyHealth(enemyType);
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        if(spriteRenderer != null)
        {
            StartCoroutine(ChangeColorToRedAndBack());
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;

            // Wylaczamy collider i ewentualnego NavMeshAgenta
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            if (navAgent != null)
            {
                navAgent.isStopped = true;
            }

            if (hasDeathAnimation && animator != null)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                Die();
            }
        }
    }

    private IEnumerator  ChangeColorToRedAndBack()
    {
        // Change the color to red
        spriteRenderer.color = Color.red;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.1f);

        // Change the color back to the original color
        spriteRenderer.color = Color.white;;
    }

    public void ForceDie()
    {
        if (isDead) return;
        isDead = true;

        Die();
    }

    // zmienione na public, problem z niszczeniem sie Overlorda
    public void Die()
    {
        // Jesli przeciwnik ma OverlordAI, wykonaj dodatkowe czyszczenie
        OverlordAI overlordAI = GetComponent<OverlordAI>();
        if (overlordAI != null)
        {
            overlordAI.OnDeath(); // Wyczysci baseny i zatrzyma coroutine
            return; // OverlordAI niszczy obiekt
        }

        // Domyslna logika smierci dla innych przeciwnikow
        ScoreManager.Instance.AddPoints(scoreValue);
        FindObjectOfType<PlayerExperience>()?.AddExperience(expValue);
        Destroy(gameObject);
    }
}