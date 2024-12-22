using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSideWepon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
<<<<<<< HEAD
    public Transform firePoint; // Punkt, z ktorego strzela shotgun
    public float fireRate = 1f; // Czas miedzy kolejnymi strzalami
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float bulletLifetime = 3f; // Czas zycia pocisku
    public int pelletCount = 4; // Liczba pociskow wystrzelonych na raz
    public float spreadAngle = 10f; // Kat rozrzutu pociskow
=======
    public Transform firePoint; // Punkt, z kt�rego strzela shotgun
    public float fireRate = 1f; // Czas mi�dzy kolejnymi strza�ami
    public float bulletSpeed = 20f; // Pr�dko�� pocisku
    public float bulletLifetime = 3f; // Czas �ycia pocisku
    public int pelletCount = 4; // Liczba pocisk�w wystrzelonych na raz
    public float spreadAngle = 10f; // K�t rozrzutu pocisk�w
>>>>>>> origin/master

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
<<<<<<< HEAD
            // Generowanie losowego kata w zakresie rozrzutu
            float angle = Random.Range(-spreadAngle, spreadAngle);

            // Obrot na podstawie kata rozrzutu
=======
            // Generowanie losowego k�ta w zakresie rozrzutu
            float angle = Random.Range(-spreadAngle, spreadAngle);

            // Obr�t na podstawie k�ta rozrzutu
>>>>>>> origin/master
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;

            // Tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

<<<<<<< HEAD
            // Ustawianie pr�dkosci pocisku
            rb.velocity = bullet.transform.forward * bulletSpeed;

            // Niszczenie pocisku po okreslonym czasie
=======
            // Ustawianie pr�dko�ci pocisku
            rb.velocity = bullet.transform.forward * bulletSpeed;

            // Niszczenie pocisku po okre�lonym czasie
>>>>>>> origin/master
            Destroy(bullet, bulletLifetime);
        }
    }
}

