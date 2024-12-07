using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWeaponsController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefabrykat pocisku
    public Transform bulletSpawnPoint; // Punkt, z kt�rego b�d� wystrzeliwane pociski
    public float bulletSpeed = 20f; // Pr�dko�� pocisku

    void Update()
    {
        // Strza� po wci�ni�ciu LPM
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Sprawdzamy, czy prefabrykat i punkt strza�u zosta�y przypisane
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Prefabrykat pocisku lub punkt strza�u nie zosta�y przypisane!");
            return;
        }

        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Pr�do�� pocisku
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Usuwamy pocisk po 3 sekundach
        Destroy(bullet, 3f);
    }
}
