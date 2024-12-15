using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSideWepon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z kt�rego strzela shotgun
    public float fireRate = 1f; // Czas mi�dzy kolejnymi strza�ami
    public float bulletSpeed = 20f; // Pr�dko�� pocisku
    public float bulletLifetime = 3f; // Czas, po kt�rym pocisk si� niszczy

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
        // Kierunek strza�u na podstawie aktualnej orientacji broni
        Vector3 direction = firePoint.forward; 

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Nadawanie pr�dko�ci pociskowi
        rb.velocity = direction * bulletSpeed;

        // Niszczenie pocisku po okre�lonym czasie
        Destroy(bullet, bulletLifetime);
    }
}
