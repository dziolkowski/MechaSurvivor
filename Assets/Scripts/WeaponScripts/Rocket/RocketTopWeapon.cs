using System.Collections;
using UnityEngine;

public class RocketTopWeapon : MonoBehaviour
{
    [Header("RocketLauncher Settings")]
    public GameObject rocketPrefab; // Prefab rakiety
    public Transform firePoint; // Punkt wystrzalu rakiety
    public float fireRate = 2f; // Czas pomiedzy kolejnymi strzalami
    public float rocketSpeed = 20f; // Predkosc rakiety
    public float explosionRadius = 5f; // Promien eksplozji
    public float damage = 50f; // Obrazenia eksplozji
    public float projectileSize = 1f; // Wielkosc pocisku
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        RotateTowardsMouse();

        if (Time.time >= nextFireTime)
        {
            FireRocket();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateTowardsMouse()
    {
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0)); // Tworzymy plaszczyzne na poziomie broni
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPosition = ray.GetPoint(enter); // Punkt przeciecia kursora z plaszczyzna
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }

    void FireRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
        rocket.transform.localScale = Vector3.one * projectileSize;
        Rigidbody rb = rocket.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * rocketSpeed;
        }

        Rocket rocketScript = rocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.SetParameters(explosionRadius, damage);
        }

        // Ignorowanie kolizji z graczem (zakladajac, ze gracz ma collider)
        Collider rocketCollider = rocket.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (rocketCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(rocketCollider, playerCollider);
        }
    }
}



