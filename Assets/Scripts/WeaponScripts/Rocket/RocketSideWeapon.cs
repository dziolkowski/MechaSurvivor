using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSideWeapon : MonoBehaviour
{
    public GameObject rocketPrefab; // Prefab rakiety
    public Transform firePoint; // Punkt wystrza�u rakiety
    public float fireRate = 2f; // Czas pomi�dzy kolejnymi strza�ami
    public float rocketSpeed = 20f; // Pr�dko�� rakiety
    public float explosionRadius = 5f; // Promie� eksplozji
    public float damage = 50f; // Obra�enia eksplozji
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireRocket();
            nextFireTime = Time.time + fireRate;
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

        // Ignorowanie kolizji z graczem (zak�adaj�c, �e gracz ma collider)
        Collider rocketCollider = rocket.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (rocketCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(rocketCollider, playerCollider);
        }
    }
}
