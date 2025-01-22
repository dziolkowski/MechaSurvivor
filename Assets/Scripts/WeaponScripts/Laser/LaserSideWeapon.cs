using System.Collections;
using UnityEngine;

public class LaserSideWeapon : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt pocz¹tkowy lasera
    public float laserRange = 100f; // Maksymalny zasiêg lasera
    public float fireRate = 0.1f; // Czêstotliwoœæ strzelania
    public int laserDamage = 10; // Obra¿enia zadawane przez laser
    public LayerMask enemyLayer; // Warstwa przeciwników

    private float nextFireTime;
    private GameObject currentLaser; // Instancja aktualnie u¿ywanego lasera

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        // Tworzenie instancji lasera, jeœli jeszcze nie istnieje
        if (currentLaser == null)
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na pocz¹tku jest niewidoczny
        }

        // Strzelanie przed siebie
        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;

        // Sprawdzanie, czy trafiono przeciwnika
        if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out RaycastHit hit, laserRange, enemyLayer))
        {
            targetPoint = hit.point; // Ustawienie celu na punkt trafienia
            DealDamage(hit.collider); // Zastosowanie obra¿eñ
        }

        AdjustLaser(targetPoint); // Dopasowanie lasera do celu
        StartCoroutine(LaserEffect());
    }

    void AdjustLaser(Vector3 targetPoint)
    {
        // Obliczanie kierunku i odleg³oœci miêdzy pocz¹tkiem a koñcem lasera
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        currentLaser.transform.position = laserOrigin.position + direction / 2f; // Œrodek miêdzy pocz¹tkiem a koñcem
        currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, currentLaser.transform.localScale.y, distance); // Dopasowanie d³ugoœci

        // Ustawianie rotacji lasera w kierunku celu
        currentLaser.transform.rotation = Quaternion.LookRotation(direction);
    }

    void DealDamage(Collider target)
    {
        // Sprawdzanie, czy trafiony obiekt ma komponent EnemyHealth
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(laserDamage);
        }
    }

    IEnumerator LaserEffect()
    {
        currentLaser.SetActive(true); // W³¹czenie widocznoœci lasera
        yield return new WaitForSeconds(0.05f); // Czas widocznoœci lasera
        currentLaser.SetActive(false); // Wy³¹czenie widocznoœci lasera
    }
}


