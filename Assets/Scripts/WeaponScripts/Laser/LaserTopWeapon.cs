using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTopWeapon : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
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
            targetPosition.y = transform.position.y; // Ustawienie tej samej wysokosci co bron
            
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
        if (currentLaser == null)
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na poczatku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, hitLayers);

        Vector3 finalHitPoint = laserOrigin.position + transform.forward * laserRange;
        List<IDamageable> damagedTargets = new List<IDamageable>();

        foreach (var hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & hitLayers) == 0)
                continue; // Ignorowanie obiektow spoza hitLayers

            // Jesli trafiony obiekt to sciana, ustawiamy punkt koncowy lasera i przerywamy
            if (hit.collider.CompareTag("Wall"))
            {
                finalHitPoint = hit.point;
                break;
            }

            // Trafiony obiekt mo¿e otrzymac obrazenia
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null && !damagedTargets.Contains(damageable))
            {
                damagedTargets.Add(damageable);
            }
        }

        // Zadanie obrazen wszystkim przeciwnikom trafionym przez laser
        foreach (var target in damagedTargets)
        {
            target.TakeDamage(laserDamage);
        }

        AdjustLaser(finalHitPoint);
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

