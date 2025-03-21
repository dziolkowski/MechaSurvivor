using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeAI : MonoBehaviour, IDamageable
{
    public Transform player;
    private NavMeshAgent agent;

    [SerializeField] private float detectionRadius = 10f; // Promien wykrywania gracza
    [SerializeField] private float explosionRadius = 5f; // Promien eksplozji
    [SerializeField] private int damageDealt = 10; // Obrazenia eksplozji
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionEffectDuration = 2f;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= explosionRadius)
            {
                Explode();
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
            Explode();
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
}

