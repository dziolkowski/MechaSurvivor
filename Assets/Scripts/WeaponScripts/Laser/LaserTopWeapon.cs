using System.Collections;
using UnityEngine;

public class LaserTopWeapon : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czêstotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask hitLayers; // Warstwy, ktore moga byc trafione

    private float nextFireTime;
    private GameObject currentLaser; // Instancja aktualnie uzywanego lasera

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        RotateWeaponTowardsCursor();

        if (Time.time >= nextFireTime) // Automatyczne strzelanie co okreslony czas
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
            targetPosition.y = transform.position.y; // Ustawienie tej samej wysokosci co broñ
            
            Vector3 direction = (targetPosition - transform.position).normalized; // Obracanie broni w kierunku kursora
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
            currentLaser.SetActive(false); // Laser na poczatku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, transform.forward); // Strzal zgodny z ustawieniem broni
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, hitLayers);

        // Szukanie najblizszego przeciwnika
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
            // Maksymalny zasieg, gdy laser w nic nie trafi
            AdjustLaser(laserOrigin.position + transform.forward * laserRange);
        }

        StartCoroutine(LaserEffect());
    }

    void AdjustLaser(Vector3 targetPoint)
    {
        // Obliczanie kierunku i odleglosci miedzy poczatkiem a koncem lasera
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        currentLaser.transform.position = laserOrigin.position + direction / 2f; // Laser na srodku
        currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, currentLaser.transform.localScale.y, distance); // Dopasowanie dlugosci

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
        yield return new WaitForSeconds(0.05f); // Czas widocznosci lasera
        currentLaser.SetActive(false);
    }
}

public interface IDamageable
{
    void TakeDamage(int damage);
}

