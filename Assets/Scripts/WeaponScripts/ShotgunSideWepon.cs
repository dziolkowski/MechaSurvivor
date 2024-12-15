using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSideWepon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z którego strzela shotgun
    public float fireRate = 1f; // Czas miêdzy kolejnymi strza³ami
    public float bulletSpeed = 20f; // Prêdkoœæ pocisku
    public float bulletLifetime = 3f; // Czas, po którym pocisk siê niszczy

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
        // Kierunek strza³u na podstawie aktualnej orientacji broni
        Vector3 direction = firePoint.forward; 

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Nadawanie prêdkoœci pociskowi
        rb.velocity = direction * bulletSpeed;

        // Niszczenie pocisku po okreœlonym czasie
        Destroy(bullet, bulletLifetime);
    }
}
