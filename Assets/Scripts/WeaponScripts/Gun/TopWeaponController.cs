using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt spawnu pociskow
    public float bulletSpeed = 20f; // Predkosc pociskow
    public float fireRate = 0.2f; // Czestotliwosc strzalow w sekundach
    public int bulletDamage = 10; // Obrazenia zadawane przez pocisk
    private float nextFireTime = 0f; // Czas, po ktorym mozna wystrzelic kolejny pocisk

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania

        RotateTowardsMouse();

        // Automatyczne strzelanie po ustalonym czasie
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Ustawienie czasu kolejnrgo strzalu
            Shoot();
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Brakuje prefabu pocisku lub punktu spawnu!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>(); // Ustawienie predkosci pocisku 

        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        bullet.AddComponent<InternalBulletHandler>().Initialize(bulletDamage);
        Destroy(bullet, 3f); // Usuwanie pocisku po 3 sekundach gdy w nic nie trafi
    }

}