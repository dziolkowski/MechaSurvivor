using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt, z ktorego beda wystrzeliwane pociski
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float fireRate = 0.2f; // Czestotliwosc strzalow w sekundach
    public int bulletDamage = 10; // Ilosc obrazen zadawanych przez pocisk
    private float nextFireTime = 0f; // Czas, po ktorym mozna wystrzelic kolejny pocisk

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania

        // Automatyczne strzelanie po ustalonym czasie
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Ustawienie czasu kolejnego strzalu
            Shoot();
        }
    }

    void Shoot()
    {

        Debug.Log("Próba strza³u!");

        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Prefab pocisku lub punkt startowy nie s¹ przypisane!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Debug.Log("Pocisk zosta³ stworzony!");

        // Ustawianie predkosci pocisku
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            Debug.Log("Pocisk ma nadan¹ prêdkoœæ!");
        }


        bullet.AddComponent<InternalBulletHandler>().Initialize(bulletDamage);
        Destroy(bullet, 3f); // Usuwanie pocisku po 3 sekundach
    }

    
}





