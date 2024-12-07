using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefabrykat pocisku
    public Transform bulletSpawnPoint; // Punkt, z którego bêd¹ wystrzeliwane pociski
    public float bulletSpeed = 20f; // Prêdkoœæ pocisku

    void Update()
    {
        // Strza³ po wciœniêciu LPM
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Sprawdzamy, czy prefabrykat i punkt strza³u zosta³y przypisane
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Prefabrykat pocisku lub punkt strza³u nie zosta³y przypisane!");
            return;
        }

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Prêdoœæ pocisku
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Usuwamy pocisk po 3 sekundach
        Destroy(bullet, 3f);
    }
}
