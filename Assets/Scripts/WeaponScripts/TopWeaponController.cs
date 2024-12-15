using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;

    void Update()
    {
        RotateTowardsMouse();

        // Strza³ za pomoc¹ LPM
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - transform.position;
            //Utrzymanie pocisku na tej samej wysokoœci
            direction.y = 0; 
            transform.forward = direction;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;

        //Pocisk zniszczony po 3 sekundach kiedy nie trafi w cel
        //Destroy(bullet, 3f); // duplikat, pocisk ma na sobie skrypt który odpowiada za jego zniszczenie
    }
}
