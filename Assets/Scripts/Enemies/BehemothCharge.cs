using System.Collections.Generic;
using UnityEngine;

public class BehemothCharge : MonoBehaviour
{
    [Header("Charge")]
    public float chargeSpeed = 10f; // Predkosc szarzy
    public float chargeDistance = 10f; // Maksymalny dystans szarzy
    public float chargeCooldown = 2f; // Czas przerwy miedzy szarzami
    public int damage = 10; // Obrazenia zadawane graczowi
    public Transform playerTransform; // Transform gracza

    [Header("Stomp")]
    public GameObject stompAOEPrefab; // Prefab strefy obrazen po tupnieciu
    public Transform stompSpawnPoint; // Punkt gdzie pojawi sie AOE
    public float stompDelay = 0.3f; // Opoznienie przed pojawieniem sie AOE (po tupnieciu)

    [Header("Enemies Spawn")]
    public List<GameObject> enemiesToSpawn; // Lista prefabow przeciwnikow do spawnu
    public int spawnCount = 2; // Ilosc przeciwnikow do spawnu po uderzeniu w przeszkode
    public float spawnRadius = 3f; // Promien spawnu wokol Behemotha

    private Rigidbody rb;
    private Vector3 chargeDirection;
    private bool isCharging = false;
    private float distanceTraveled = 0f;
    private float cooldownTimer = 0f;
    private bool hasHitPlayer = false;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Blokujemy rotacje i ruch w osi Y
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (!isCharging)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                StartChargeToPlayer();
            }
        }
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            rb.velocity = chargeDirection * chargeSpeed;

            distanceTraveled += (chargeDirection * chargeSpeed * Time.fixedDeltaTime).magnitude;

            if (distanceTraveled >= chargeDistance)
            {
                StopCharge();
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        // Zabezpieczenie przed zapadaniem sie pod ziemie
        if (transform.position.y < 0.5f)
        {
            Vector3 pos = transform.position;
            pos.y = 0.5f;
            transform.position = pos;
        }
    }

    void StartChargeToPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Nie przypisano transform gracza!");
            return;
        }

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        StartCharge(direction);
    }

    void StartCharge(Vector3 direction)
    {
        chargeDirection = direction;
        distanceTraveled = 0f;
        isCharging = true;
        hasHitPlayer = false;

        Debug.Log("Behemoth zaczyna szarze!");
    }

    void StopCharge()
    {
        isCharging = false;
        cooldownTimer = chargeCooldown;

        // Zatrzymujemy ruch
        rb.velocity = Vector3.zero;


        if (!hasHitPlayer)
        {
            if (animator != null)
            {
                animator.SetTrigger("Stomp");
            }

            Invoke(nameof(SpawnStompAOE), stompDelay);
        }
    }

    void SpawnStompAOE()
    {
        if (stompAOEPrefab != null && stompSpawnPoint != null)
        {
            Instantiate(stompAOEPrefab, stompSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Brakuje prefab stompAOEPrefab lub stompSpawnPoint!");
        }
    }

    void SpawnEnemiesOnCollision()
    {
        if (enemiesToSpawn.Count == 0) return;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];

            Vector3 spawnPos = transform.position + (Random.insideUnitSphere * spawnRadius);
            spawnPos.y = transform.position.y;

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isCharging) return;

        // Jesli trafilismy gracza
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            hasHitPlayer = true;
            StopCharge();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            SpawnEnemiesOnCollision();
            StopCharge();
        }
    }
}
