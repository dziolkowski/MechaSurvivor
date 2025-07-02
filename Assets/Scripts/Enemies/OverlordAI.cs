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
    [SerializeField] private float explosionIncrementalRadius = 0.5f; // Maksymalny przyrost promienia
    [SerializeField] private int explosionDamage = 10; // Obrazenia od eksplozji
    [SerializeField] private float attackTriggerDistance = 10f; // Obszar, w ktorym uruchamia sie atak
    [SerializeField] private GameObject explosionPoolPrefab; // Prefab strefy eksplozji

    private bool isAttacking = false; // Czy w trakcie ataku
    private Coroutine attackRoutine; // Referencja do aktywnego coroutina
    private List<ExplosionPool> activePools = new List<ExplosionPool>(); // Lista aktywnych basenow
    
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed; // Ustaw predkosc agenta
        animator = GetComponent<Animator>();

        // Znajdz gracza jesli jeszcze nie znaleziony
        if (player == null)
        {
            GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");
            if (findPlayer != null) player = findPlayer.transform;
        }
    }

    void Update()
    {
        if (player != null && !isAttacking)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackTriggerDistance)
            {
                // Sprawdzenie widocznosci gracza
                Ray ray = new Ray(transform.position + Vector3.up, (player.position - transform.position).normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, attackTriggerDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        attackRoutine = StartCoroutine(ExplosionAttack());
                    }
                }
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
        activePools.Clear();
        animator.SetTrigger("Attack");

        Vector3 direction = (player.position - transform.position).normalized;
        float totalDistance = Vector3.Distance(transform.position, player.position);
        float spacing = totalDistance / amountOfPoolsSpawned;

        // Tworzenie basenow po kolei
        for (int i = 0; i < amountOfPoolsSpawned; i++)
        {
            Vector3 spawnPos = transform.position + direction * spacing * (i + 1);
            GameObject pool = Instantiate(explosionPoolPrefab, spawnPos, Quaternion.identity);

            float distanceToPlayer = Vector3.Distance(spawnPos, player.position);
            float t = 1.5f - (distanceToPlayer / totalDistance); // Im blizej gracza, tym wieksze t

            float radius = explosionInitialRadius + t * explosionIncrementalRadius;

            // Skala w poziomie
            float visualScale = 1f + t;
            float originalY = pool.transform.localScale.y;
            pool.transform.localScale = new Vector3(visualScale, originalY, visualScale);

            ExplosionPool explosionScript = pool.GetComponent<ExplosionPool>();
            explosionScript.Prepare(explosionDamage, radius);
            activePools.Add(explosionScript);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f); // Pauza po ostatnim basenie

        // Eksplozje po kolei
        foreach (ExplosionPool pool in activePools)
        {
            if (pool != null) pool.Explode();
            yield return new WaitForSeconds(explosionDelayBetweenPools); // Odstep miedzy eksplozjami
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    // Wywolywane przy smierci przeciwnika
    public void OnDeath()
    {
        if (isAttacking && attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            StartCoroutine(FinishExplosionSequenceAndDie());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FinishExplosionSequenceAndDie()
    {
        // Eksplozje po kolei (tak jak w normalnym ataku)
        foreach (ExplosionPool pool in activePools)
        {
            if (pool != null)
            {
                pool.Explode();
                yield return new WaitForSeconds(explosionDelayBetweenPools);
            }
        }

        yield return new WaitForSeconds(0.5f); // Pauza po ostatnim wybuchu

        // Usuwanie obiektow po eksplozji
        foreach (ExplosionPool pool in activePools)
        {
            if (pool != null)
            {
                Destroy(pool.gameObject);
            }
        }

        activePools.Clear();
        Destroy(gameObject);
    }
}
