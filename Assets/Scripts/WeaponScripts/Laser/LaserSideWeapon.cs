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
    public LayerMask obstacleLayer; // Warstwa przeszk�d (sciany)

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
        
        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;// Strzelanie przed siebie
        RaycastHit[] hits = Physics.RaycastAll(laserOrigin.position, laserOrigin.forward, laserRange, enemyLayer | obstacleLayer);

        foreach (var hit in hits) 
        { 
            if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0) // Trafienie przeciwnika
            { 
                DealDamage(hit.collider);
            }

            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0) // Trafienie przeszkody
            {
                targetPoint = hit.point; // Skrocenie lasera do sciany
                break; // Sciana zatrzymuje promien
            } 
                                                                             

        }

        AdjustLaser(targetPoint); // Dopasowanie lasera do celu
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
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
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


