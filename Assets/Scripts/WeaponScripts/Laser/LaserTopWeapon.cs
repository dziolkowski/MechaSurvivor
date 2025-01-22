using System.Collections;
using UnityEngine;

public class LaserTopWeapon : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt pocz¹tkowy lasera
    public float laserRange = 100f; // Maksymalny zasiêg lasera
    public float fireRate = 0.1f; // Czêstotliwoœæ strzelania
    public int laserDamage = 10; // Obra¿enia zadawane przez laser
    public LayerMask hitLayers; // Warstwy, które mog¹ byæ trafione

    private float nextFireTime;
    private GameObject currentLaser; // Instancja aktualnie u¿ywanego lasera

    void Update()
    {
        RotateWeaponTowardsCursor();

        if (Time.time >= nextFireTime) // Automatyczne strzelanie co okreœlony czas
        {
            nextFireTime = Time.time + fireRate;
            ShootLaser();
        }
    }

    void RotateWeaponTowardsCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Ustawienie tej samej wysokoœci co broñ

            // Obracanie broni w kierunku kursora
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }

    void ShootLaser()
    {
        // Tworzenie instancji lasera
        if (currentLaser == null)
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na pocz¹tku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, transform.forward); // Strza³ zgodny z ustawieniem broni
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, hitLayers);

        // Szukanie najbli¿szego przeciwnika
        RaycastHit? closestHit = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(laserOrigin.position, hit.point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
            }
        }

        if (closestHit.HasValue)
        {
            DealDamage(closestHit.Value.collider);

            // Ustawianie lasera na trafiony punkt
            AdjustLaser(closestHit.Value.point);
        }
        else
        {
            // Maksymalny zasiêg, gdy laser w nic nie trafi
            AdjustLaser(laserOrigin.position + transform.forward * laserRange);
        }

        StartCoroutine(LaserEffect());
    }

    void AdjustLaser(Vector3 targetPoint)
    {
        // Obliczanie kierunku i odleg³oœci miêdzy pocz¹tkiem a koñcem lasera
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        currentLaser.transform.position = laserOrigin.position + direction / 2f; // Laser na œrodku
        currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, currentLaser.transform.localScale.y, distance); // Dopasowanie d³ugoœci

        // Ustawianie rotacji lasera w kierunku celu
        currentLaser.transform.rotation = Quaternion.LookRotation(direction);
    }

    void DealDamage(Collider target)
    {
        // Sprawdzanie, czy obiekt implementuje interfejs IDamageable
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(laserDamage);
        }
    }

    IEnumerator LaserEffect()
    {
        currentLaser.SetActive(true);
        yield return new WaitForSeconds(0.05f); // Czas widocznoœci lasera
        currentLaser.SetActive(false);
    }
}

public interface IDamageable
{
    void TakeDamage(int damage);
}

