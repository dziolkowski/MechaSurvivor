using UnityEngine.AI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private string enemyType;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] private bool hasDeathAnimation;

    public int scoreValue = 10;
    public int expValue = 10;
    private Animator animator;
    private EnemyManager enemyManager;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = FindAnyObjectByType<EnemyManager>();

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

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;

            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().isStopped = true;

            if (hasDeathAnimation)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                Die();
            }
        }
    }

    public void ForceDie()
    {
        if (isDead) return;
        isDead = true;

        Die(); // Wymuszenie natychmiastowej smierci
    }

    private void Die()
    {
        ScoreManager.Instance.AddPoints(scoreValue);
        FindObjectOfType<PlayerExperience>()?.AddExperience(expValue);
        Destroy(gameObject);
    }
}