using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeAI : MonoBehaviour, IDamageable
{
    private Transform player; // Gracz 

    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float detectionRadius = 5f;

    [SerializeField] private float explosionDelay = 0.5f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int damageDealt = 10;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionEffectDuration = 2f;

    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int expValue = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isExploding = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = movementSpeed;
    }

    void Update()
    {
        if (isExploding || isDead) return;

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        // Jesli gracz nie przypiety, sprobuj go znalezc
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else return;
        }

        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRadius)
        {
            StartExplosion(); // Wybuch, gdy gracz jest blisko
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        isDead = true;
        StartExplosion(); // Wybuch przy smierci
    }

    private void StartExplosion()
    {
        if (isExploding) return;

        isExploding = true;
        agent.isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, explosionEffectDuration);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        HashSet<GameObject> damaged = new HashSet<GameObject>();

        foreach (Collider hit in hits)
        {
            if (!damaged.Contains(hit.gameObject))
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                    if (ph != null)
                    {
                        ph.TakeDamage(damageDealt);
                    }
                }
                else if (hit.CompareTag("Enemy") && hit.gameObject != gameObject)
                {
                    IDamageable enemy = hit.GetComponent<IDamageable>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damageDealt);
                    }
                }
                damaged.Add(hit.gameObject);
            }
        }

        ScoreManager.Instance?.AddPoints(scoreValue);
        FindObjectOfType<PlayerExperience>()?.AddExperience(expValue);

        Destroy(gameObject); // Znikniecie po eksplozji
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

