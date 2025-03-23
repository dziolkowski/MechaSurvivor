using System.Collections;
using UnityEngine;

public class LaserSideWeapon : MonoBehaviour
{
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask enemyLayer; // Warstwa przeciwnikow
    public LayerMask obstacleLayer; // Warstwa przeszkód (sciany)

    private float nextFireTime;
    private GameObject currentLaser; // Instancja aktualnie uzywanego lasera

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootLaser();
        }
    }

    void ShootLaser()
    {        
        if (currentLaser == null)// Tworzenie instancji lasera, jesli jeszcze nie istnieje
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na poczatku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, enemyLayer | obstacleLayer);

        // Sortowanie trafien od najblizszego do najdalszego
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;

        foreach (var hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0) // Trafienie w przeszkode
            {
                targetPoint = hit.point; // Skrocenie lasera do sciany
                break; // Sciana zatrzymuje promien
            }

            if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0) // Trafienie przeciwnika
            {
                DealDamage(hit.collider);
            }
        }

        AdjustLaser(targetPoint);
        StartCoroutine(LaserEffect());
    }

    void AdjustLaser(Vector3 targetPoint)
    {        
        Vector3 direction = targetPoint - laserOrigin.position; // Obliczanie kierunku i odleglosci miedzy poczatkiem a koncem lasera
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        currentLaser.transform.position = laserOrigin.position + direction / 2f; // Srodek miedzy poczatkiem a koncem
        currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, currentLaser.transform.localScale.y, distance); // Dopasowanie dlugosci

        // Ustawianie rotacji lasera w kierunku celu
        currentLaser.transform.rotation = Quaternion.LookRotation(direction);
    }

    void DealDamage(Collider target)
    {
        // Sprawdzanie, czy trafiony obiekt ma komponent EnemyHealth
        IDamageable enemyHealth = target.GetComponent<IDamageable>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(laserDamage);
        }
    }

    IEnumerator LaserEffect()
    {
        currentLaser.SetActive(true); // Wlaczenie widocznosci lasera
        yield return new WaitForSeconds(0.05f); // Czas widocznosci lasera
        currentLaser.SetActive(false); // Wylaczenie widocznosci lasera
    }
}


