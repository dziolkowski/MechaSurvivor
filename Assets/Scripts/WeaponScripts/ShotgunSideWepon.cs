using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSideWepon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z którego strzela shotgun
    public float fireRate = 1f; // Czas miêdzy kolejnymi strza³ami
    public float bulletSpeed = 20f; // Prêdkoœæ pocisku
    public float bulletLifetime = 3f; // Czas ¿ycia pocisku
    public int pelletCount = 4; // Liczba pocisków wystrzelonych na raz
    public float spreadAngle = 10f; // K¹t rozrzutu pocisków

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // Generowanie losowego k¹ta w zakresie rozrzutu
            float angle = Random.Range(-spreadAngle, spreadAngle);

            // Obrót na podstawie k¹ta rozrzutu
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;

            // Tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Ustawianie prêdkoœci pocisku
            rb.velocity = bullet.transform.forward * bulletSpeed;

            // Niszczenie pocisku po okreœlonym czasie
            Destroy(bullet, bulletLifetime);
        }
    }
}

