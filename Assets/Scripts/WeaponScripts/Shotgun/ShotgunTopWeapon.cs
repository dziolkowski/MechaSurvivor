using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTopWeapon : MonoBehaviour
{
    [Header("Shotgun Setings")]
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z ktorego strzela shotgun
    public float fireRate = 1f; // Czas miedzy kolejnymi strzalami
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float bulletLifetime = 3f; // Czas zycia pocisku
    public int pelletCount = 4; // Liczba pociskow wystrzelonych na raz
    public float spreadAngle = 10f; // Kat rozrzutu pocisków
    public int bulletDamage = 25; // Obrazenia zadawane przez pocisk
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        RotateTowardsMouse();

        if (Time.time >= nextFireTime) 
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Ustawienie tej samej wysokosci dla kazdego pocisku

            // Obracanie broni w kierunku kursora
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // Generowanie losowego kata w zakresie rozrzutu
            float angle = Random.Range(-spreadAngle, spreadAngle);

            // Obrot na podstawie kata rozrzutu
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;

            // Tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Ustawianie predkosci pocisku
            rb.velocity = bullet.transform.forward * bulletSpeed;

            bullet.AddComponent<InternalBulletHandler>().Initialize(bulletDamage);
            // Niszczenie pocisku po okreslonym czasie
            Destroy(bullet, bulletLifetime);
        }
    }
}


