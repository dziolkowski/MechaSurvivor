using UnityEngine;

public class BehemothCharge : MonoBehaviour
{
    public float chargeSpeed = 10f; // Predkosc szarzy
    public float chargeDistance = 10f; // Maksymalny dystans szarzy
    public float chargeCooldown = 2f; // Czas przerwy miedzy szarzami
    public int damage = 10; // Ilosc obrazen zadawanych graczowi
    public Transform playerTransform; // Transform gracza 

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 chargeDirection;
    private bool isCharging = false;
    private float distanceTraveled = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Blokujemy ruch w osi Y, zeby Behemoth nie unosil sie w powietrze
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

            StopCharge();
        }
    }

    void StartCharge()
    {
        startPosition = transform.position;
        distanceTraveled = 0f;
        isCharging = true;
    }

    void StopCharge()
    {
        isCharging = false;
        cooldownTimer = chargeCooldown;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Stabilizacja Behemotha po szarzy
        RaycastHit hit;
        float rayHeight = 2f;
        float maxRayDistance = 10f;

        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, maxRayDistance))
        {
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;
            rb.MovePosition(newPos); // zamiast transform.position
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

        // Kierunek do gracza w plaszczyznie poziomej
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f; // ignorujemy wysokosc
        chargeDirection = direction.normalized;

        transform.forward = chargeDirection;
    }
}
