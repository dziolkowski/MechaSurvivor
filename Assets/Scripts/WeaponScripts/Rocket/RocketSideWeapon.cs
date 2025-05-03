using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSideWeapon : MonoBehaviour
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
        if (Time.time >= nextFireTime)
        {
            FireRocket();
            nextFireTime = Time.time + fireRate;
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
