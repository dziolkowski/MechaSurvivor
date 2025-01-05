using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSideWeapon : MonoBehaviour
{
    public Transform laserObject; // Obiekt lasera 
    public Transform laserOrigin; // Punkt poczatkowy lasera 
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czêstotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask enemyLayer; // Warstwa przeciwnikow

    private float nextFireTime;

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
        // Strzelanie przed siebie
        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;

        // Sprawdz, czy trafiono przeciwnika
        if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out RaycastHit hit, laserRange, enemyLayer))
        {
            targetPoint = hit.point; // Ustawienie celu na punkt trafienia
            DealDamage(hit.collider); // Zastosowanie obrazen
        }

        AdjustLaser(targetPoint); // Dopasowanie lasera do celu
        StartCoroutine(LaserEffect());
    }

    void AdjustLaser(Vector3 targetPoint)
    {
        // Obliczanie kierunku i odleg³osci miedzy poczatkiem a koncem lasera
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        laserObject.position = laserOrigin.position + direction / 2f; // Srodek miedzy poczatkiem a koncem
        laserObject.localScale = new Vector3(laserObject.localScale.x, laserObject.localScale.y, distance); // Dopasowanie dlugosci

        // Ustawianie rotacji lasera w kierunku celu
        laserObject.rotation = Quaternion.LookRotation(direction);
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
        laserObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f); // Czas widocznosci lasera
        laserObject.gameObject.SetActive(false);
    }
}


