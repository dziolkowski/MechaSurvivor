using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTopWeapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z którego strzela shotgun
    public float fireRate = 1f; // Czas miêdzy kolejnymi strza³ami
    public float bulletSpeed = 20f; // Prêdkoœæ pocisku
    public float bulletLifetime = 3f; // Czas, po którym pocisk siê niszczy

    private float nextFireTime = 0f;

    void Update()
    {
        RotateTowardsMouse(); // Obracanie shotgunem w kierunku kursora

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void RotateTowardsMouse()
    {
        // Obliczenie kierunku do kursora myszy
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Pociski na jednakowej wysokoœci

            // Obracanie broni w kierunku kursora myszki
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }

    void Fire()
    {
        // Obliczenie kierunku strza³u w kierunku kursora myszy
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = firePoint.position.y; // Ustawienie sta³ej wysokoœci pocisku

            Vector3 direction = (targetPosition - firePoint.position).normalized;

            // Tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = direction * bulletSpeed;

            // Niszczenie pocisku po okreœlonym czasie
            Destroy(bullet, bulletLifetime);
        }
    }
}


