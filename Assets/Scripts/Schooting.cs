using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float bulletSpeed = 20f;
    public float bulletLifetime = 2f; // Czas ¿ycia pocisku

    void Start()
    {
        // Sprawdzanie, czy bulletPrefab jest przypisany
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
        }
    }

    void Update()
    {
        // Strza³ lewym przyciskiem myszy
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // Obliczanie kierunku strza³u
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 shootDirection;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    shootDirection = (hit.point - transform.position).normalized;
                }
                else
                {
                    shootDirection = (ray.GetPoint(100) - transform.position).normalized;
                }

                // Ustawianie rotacji i prêdkoœci pocisku
                bullet.transform.rotation = Quaternion.LookRotation(shootDirection);
                bulletRb.velocity = shootDirection * bulletSpeed;

                // Zniszczenie pocisku po okreœlonym czasie
                Destroy(bullet, bulletLifetime);
            }
        }
    }
}

