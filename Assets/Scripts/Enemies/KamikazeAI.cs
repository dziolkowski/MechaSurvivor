using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikazeAI : MonoBehaviour
{
    public Transform player; // Obiekt gracza
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private float explosionRadius = 3f; // Promien eksplozji
    [SerializeField] private int damageDealt = 10; // Obrazenia zadane przez eksplozje
    [SerializeField] private GameObject explosionEffect; // Prefab eksplozji
    [SerializeField] private float explosionEffectDuration = 2f; // Znikniecie eksplozji po 2. sekundach

    [Header("Debug - do not ship modified")]
    [SerializeField] private bool isMoving = true;

    void Start()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie ruchu
        if (!isMoving)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");
        if (findPlayer == null) return;
        player = findPlayer.transform;
        agent.SetDestination(player.position);

        // Sprawdzenie dystansu do gracza i aktywowanie eksplozji
        if (Vector3.Distance(transform.position, player.position) <= explosionRadius)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Stworzenie efektu eksplozji
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionInstance, explosionEffectDuration);
        }

        // Zadanie obra¿ez graczowi
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageDealt);
            }
        }

        // Zniszczenie Kamikaze
        Destroy(gameObject);
    }
}
