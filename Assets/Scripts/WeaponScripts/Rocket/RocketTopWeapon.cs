using System.Collections;
using UnityEngine;

public class RocketTopWeapon : MonoBehaviour
{
    public GameObject rocketPrefab; // Prefab rakiety
    public Transform firePoint; // Punkt wystrzalu rakiety
    public float fireRate = 2f; // Czas pomiedzy kolejnymi strzalami
    public float rocketSpeed = 20f; // Predkosc rakiety
    public float explosionRadius = 5f; // Promien eksplozji
    public float damage = 50f; // Obrazenia eksplozji
    private float nextFireTime = 0f;

    void Update()
    {
        RotateTowardsMouse();

        if (Time.time >= nextFireTime)
        {
            FireRocket();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }

    void FireRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
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



