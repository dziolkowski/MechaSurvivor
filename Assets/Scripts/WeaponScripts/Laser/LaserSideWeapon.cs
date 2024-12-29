using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSideWeapon : MonoBehaviour
{
    public Transform laserObject; // Obiekt lasera 
    public Transform laserOrigin; // Punkt poczatkowy lasera 
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
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
        Transform closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            // Strzelaj w kierunku najblizszego przeciwnika
            Vector3 targetPoint = closestEnemy.position;
            AdjustLaser(targetPoint);
            DealDamage(closestEnemy);
        }
        else
        {
            // Jesli nie ma przeciwnikow, strzelaj przed siebie
            Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;
            AdjustLaser(targetPoint);
        }

        StartCoroutine(LaserEffect());
    }

    Transform FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(laserOrigin.position, laserRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(laserOrigin.position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hitCollider.transform;
            }
        }

        return closestEnemy;
    }

    void AdjustLaser(Vector3 targetPoint)
    {
        // Obliczanie kierunku i odleglosci miedzy poczatkiem a koncem lasera
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        // Ustawianie pozycji i skali lasera
        laserObject.position = laserOrigin.position + direction / 2f; // Ustawianie lasera na srodku miedzy poczatkiem a koncem
        laserObject.localScale = new Vector3(laserObject.localScale.x, laserObject.localScale.y, distance); // Dopasowanie dlugosci

        // Ustawianie rotacji lasera w kierunku celu
        laserObject.rotation = Quaternion.LookRotation(direction);
    }

    void DealDamage(Transform target)
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


