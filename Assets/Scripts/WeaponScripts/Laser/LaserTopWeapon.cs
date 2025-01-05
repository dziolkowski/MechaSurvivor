using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTopWeapon : MonoBehaviour
{
    public Transform laserObject; // Obiekt lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask hitLayers; // Warstwy, ktore moga byc trafione

    private float nextFireTime;

    void Update()
    {
        RotateWeaponTowardsCursor();

        if (Time.time >= nextFireTime)
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Poczatkowe ustawienie pozycji lasera
        laserObject.position = laserOrigin.position;

        if (Physics.Raycast(ray, out hit, laserRange, hitLayers))
        {
            // Dostosowanie lasera i zadawanie obrazen
            Vector3 targetPoint = hit.point;
            targetPoint.y = laserOrigin.position.y; // Wymuszenie tej samej wysokosci
            AdjustLaser(targetPoint);
            DealDamage(hit.collider);
        }
        else
        {
            // Maksymalna dlugosc gdy laser w nic nie trafi
            Vector3 targetPoint = ray.GetPoint(laserRange);
            targetPoint.y = laserOrigin.position.y; // Wymuszenie tej samej wysokosci
            AdjustLaser(targetPoint);
        }

        StartCoroutine(LaserEffect());
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
        laserObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f); // Czas widocznosci lasera
        laserObject.gameObject.SetActive(false);
    }
}

public interface IDamageable
{
    void TakeDamage(int damage);
}
