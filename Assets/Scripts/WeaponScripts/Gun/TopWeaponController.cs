using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWeaponController : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt spawnu pociskow
    public int projectileAmount = 1; // Ilosc pociskow
    public float delayBetweenProjectiles = 0.05f; // Odstep miedzy wytrzalami
    public float bulletSpeed = 20f; // Predkosc pociskow
    public float bulletLifetime = 3f; // Czas zycia pocisku
    public float fireRate = 0.2f; // Czestotliwosc strzalow w sekundach
    public int bulletDamage = 10; // Obrazenia zadawane przez pocisk
    public Vector3 projectileSize = Vector3.one; // Wielkosc pocisku
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
            Debug.LogError("Prefab pocisku lub punkt startowy nie s¹ przypisane!");
            return;
        }

        StartCoroutine(ShootBurst());
    }

    IEnumerator ShootBurst()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.transform.localScale = projectileSize;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }

            bullet.AddComponent<InternalBulletHandler>().Initialize(bulletDamage);
            Destroy(bullet, bulletLifetime);

            if (i < projectileAmount - 1)
                yield return new WaitForSeconds(delayBetweenProjectiles);
        }
    }

}