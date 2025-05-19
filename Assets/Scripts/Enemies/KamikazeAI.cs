using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeAI : MonoBehaviour, IDamageable
{
    public Transform player;
    private NavMeshAgent agent;

    [SerializeField] private float movementSpeed = 3.5f; // Predkosc poruszania sie kamikadze
    [SerializeField] private float detectionRadius = 10f; // Promien wykrywania gracza
    [SerializeField] private float explosionRadius = 5f; // Promien eksplozji
    [SerializeField] private int damageDealt = 10; // Obrazenia eksplozji
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionEffectDuration = 2f;

    private bool isDead = false;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed; // Ustaw predkosc kamikaze
        FindPlayer();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= explosionRadius) {
                GetComponent<CapsuleCollider>().enabled = false; // wylaczenie collidera aby nie zadawac graczowi obrazen /L
                GetComponent<NavMeshAgent>().isStopped = true; // zatrzymanie przeciwnika w momencie kiedy ma 0 HP /L
                animator.SetTrigger("Death");
            }
        }
    }

    private void FindPlayer()
    {
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");
        if (findPlayer != null)
        {
            player = findPlayer.transform;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false; // wylaczenie collidera aby nie zadawac graczowi obrazen /L
            GetComponent<NavMeshAgent>().isStopped = true; // zatrzymanie przeciwnika w momencie kiedy ma 0 HP /L
            animator.SetTrigger("Death");
            //Explode();
        }
    }

    private void Explode()
    {
        // Tworzenie efektu eksplozji
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionInstance, explosionEffectDuration);
        }

        // Zadawanie obrazen wszystkim w zasiegu
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        HashSet<GameObject> damagedEntities = new HashSet<GameObject>();

        foreach (Collider hit in hitColliders)
        {
            if (!damagedEntities.Contains(hit.gameObject))
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageDealt);
                    }
                }
                else if (hit.CompareTag("Enemy"))
                {
                    IDamageable enemy = hit.GetComponent<IDamageable>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damageDealt);
                    }
                }
                damagedEntities.Add(hit.gameObject);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

