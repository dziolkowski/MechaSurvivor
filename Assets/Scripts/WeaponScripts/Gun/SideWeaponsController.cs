using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt, z ktorego beda wystrzeliwane pociski
    public int projectileAmount = 1; // Ilosc pociskow
    public float delayBetweenProjectiles = 0.05f; // Odstep miedzy wytrzalami
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float bulletLifetime = 3f; // Czas zycia pocisku 
    public float fireRate = 0.2f; // Czestotliwosc strzalow w sekundach
    public int bulletDamage = 10; // Ilosc obrazen zadawanych przez pocisk
    public float projectileSize = 0.25f; // Wielkosc pocisku
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
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Prefab pocisku lub punkt startowy nie s¹ przypisane!");
            return;
        }

        StartCoroutine(ShootBurst());
    }

    IEnumerator ShootBurst ()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.layer = LayerMask.NameToLayer("Bullet");
            bullet.transform.localScale = Vector3.one * projectileSize; 

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





