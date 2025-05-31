using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverlordAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    
    [SerializeField] private float movementSpeed = 3.5f; // Predkosc poruszania sie
    [SerializeField] private int amountOfPoolsSpawned = 5; // Ilosc stref
    [SerializeField] private float explosionDelayBetweenPools = 0.5f; // Czas miedzy eksplozjami
    [SerializeField] private float explosionInitialRadius = 1f; // Poczatkowy promien
    [SerializeField] private float explosionIncrementalRadius = 0.5f; // Przyrost promienia
    [SerializeField] private int explosionDamage = 10; // Obrazenia od eksplozji
    [SerializeField] private float attackTriggerDistance = 10f; // Obaszar, w ktorym uruchamia sie atak
    [SerializeField] private GameObject explosionPoolPrefab; // Prefab strefy eksplozji

    private bool isAttacking = false; // Czy w trakcie ataku

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed; // Ustaw predkosc agenta
    }

    void Update()
    {
        // Znajdz gracza jesli jeszcze nie znaleziony
        if (player == null)
        {
            GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");
            if (findPlayer != null) player = findPlayer.transform;
        }

        if (player != null && !isAttacking)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackTriggerDistance)
            {
                // Atak jesli gracz jest w obszarze
                StartCoroutine(ExplosionAttack());
            }
            else
            {
                // Idz do gracza jesli jest daleko
                agent.SetDestination(player.position);
            }
        }
    }

    private IEnumerator ExplosionAttack()
    {
        isAttacking = true;
        agent.ResetPath(); // Zatrzymaj ruch

        Vector3 direction = (player.position - transform.position).normalized;
        float spacing = Vector3.Distance(transform.position, player.position) / amountOfPoolsSpawned;

        List<ExplosionPool> allPools = new List<ExplosionPool>();

        // Tworzenie basenow po kolei
        for (int i = 0; i < amountOfPoolsSpawned; i++)
        {
            Vector3 spawnPos = transform.position + direction * spacing * (i + 1);
            GameObject pool = Instantiate(explosionPoolPrefab, spawnPos, Quaternion.identity);

            float radius = explosionInitialRadius + i * explosionIncrementalRadius;

            ExplosionPool explosionScript = pool.GetComponent<ExplosionPool>();
            explosionScript.Prepare(explosionDamage, radius);
            allPools.Add(explosionScript);

            yield return new WaitForSeconds(0.2f); // Odstep miedzy pojawianiem sie basenow
        }

        yield return new WaitForSeconds(0.5f); // Pauza po ostatnim basenie

        // Eksplozje po kolei
        foreach (ExplosionPool pool in allPools)
        {
            pool.Explode();
            yield return new WaitForSeconds(explosionDelayBetweenPools); // ODSTEP MIEDZY EKSPLOZJAMI
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}
