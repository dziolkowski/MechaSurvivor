using UnityEngine;

public class BehemothCharge : MonoBehaviour
{
    public float chargeSpeed = 10f; // Predkosc szarzy
    public float chargeDistance = 10f; // Maksymalny dystans szarzy
    public float chargeCooldown = 2f; // Czas przerwy miedzy szarzami
    public int damage = 10; // Obrazenia od szarzy
    public Transform playerTransform; // Transform gracza

    public GameObject stompAOEPrefab; // Prefab strefy obrazen po tupnieciu
    public Transform stompSpawnPoint; // Punkt gdzie pojawi sie AOE
    public float stompDelay = 0.3f; // Opoznienie przed pojawieniem sie AOE 

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 chargeDirection;
    private bool isCharging = false;
    private float distanceTraveled = 0f;
    private float cooldownTimer = 0f;
    private bool hasHitPlayerDuringCharge = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (!isCharging)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                ChoosePlayerDirection();
                StartCharge();
            }
        }
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            Vector3 movement = chargeDirection * chargeSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            distanceTraveled += movement.magnitude;

            if (distanceTraveled >= chargeDistance)
            {
                StopCharge();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            hasHitPlayerDuringCharge = true;
            StopCharge();
        }
    }

    void StartCharge()
    {
        startPosition = transform.position;
        distanceTraveled = 0f;
        isCharging = true;
        hasHitPlayerDuringCharge = false; // Reset flagi
    }

    void StopCharge()
    {
        isCharging = false;
        cooldownTimer = chargeCooldown;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StabilizeToGround();

        // Jesli Behemoth NIE trafil gracza w czasie szarzy, wykonaj tupniecie
        if (!hasHitPlayerDuringCharge)
        {
            Animator animator = GetComponent<Animator>();
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
    }

    void StabilizeToGround()
    {
        RaycastHit hit;
        float rayHeight = 2f;
        float maxRayDistance = 10f;
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, maxRayDistance))
        {
            Vector3 newPos = transform.position;

            // Sprawdz pozycje pivota vs collider
            float yOffset = 0f;
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                float pivotToBottom = transform.position.y - col.bounds.min.y;

                // Dodaj tylko tyle, ile potrzeba, zeby collider dotykal ziemi
                yOffset = pivotToBottom;
            }

            newPos.y = hit.point.y + yOffset;
            transform.position = newPos;
        }
        else
        {
            Debug.LogWarning("Behemoth nie znalazl ziemi pod soba!");
        }
    }

    void ChoosePlayerDirection()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Behemoth nie ma przypisanego gracza!");
            chargeDirection = transform.forward;
            return;
        }

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;
        chargeDirection = direction.normalized;

        transform.forward = chargeDirection;
    }
}
