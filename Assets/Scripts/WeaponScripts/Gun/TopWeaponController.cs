using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f; // Czestotliwosc strzalow 

    private float nextFireTime;

    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        RotateTowardsMouse();

        // Automatyczne strzelanie
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - transform.position;
            // Utrzymanie pocisku na tej samej wysokosci
            direction.y = 0; 
            transform.forward = direction;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        // Zniszczenie pocisku po 3 sekundach, jesli nie trafi w cel
        Destroy(bullet, 3f);
    }
}