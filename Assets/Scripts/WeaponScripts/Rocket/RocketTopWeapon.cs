using System.Collections;
using System.Collections.Generic;
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
        // Pozycja kursora w swiecie
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        // Kierunek z broni do kursora
        Vector3 targetDirection = mousePosition - transform.position;
        targetDirection.y = 0; // Obrót tylko w osi poziomej

        // Jesli mamy kierunek do obrotu
        if (targetDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Plynne przejscie
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
    }
}

public class Rocket : MonoBehaviour
{
    private float explosionRadius;
    private float damage;
    public GameObject explosionEffect; // Prefab efektu eksplozji

    public void SetParameters(float radius, float dmg)
    {
        explosionRadius = radius;
        damage = dmg;
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        // Efekt eksplozji
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Znalezienie obiektow w promieniu eksplozji
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Zadawanie obrazen 
            EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

        Destroy(gameObject); // Zniszczenie rakiety
    }
}
