using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt, z ktorego beda wystrzeliwane pociski
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float fireRate = 0.2f; // Czestotliwosc strzalow w sekundach
    private float nextFireTime = 0f; // Czas, w ktorym mozna wystrzelic kolejny pocisk

    void Update()
    {
        // Automatyczne strzelanie po ustalonym czasie
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Ustawienie czasu kolejnego strzalu
            Shoot();
        }
    }

    void Shoot()
    {
        // Sprawdzanie, czy prefab pocisku i punkt startowy sa przypisane
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Prefab pocisku lub punkt startowy nie sa przypisane!");
            return;
        }

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Ustawianie predkosci pocisku
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Usuwanie pocisku po 3 sekundach
        Destroy(bullet, 3f);
    }
}
