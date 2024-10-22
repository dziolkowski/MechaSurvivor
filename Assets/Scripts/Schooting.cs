using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public float bulletSpeed = 20f;

    void Update()
    {
        //Strza³ lewym przyciskiem myszy
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);

        // Obliczanie kierunek strza³u
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 shootDirection;
        if (Physics.Raycast(ray, out hit))
        {
            shootDirection = (hit.point - transform.position).normalized;
        }
        else
        {
            // Jeœli pocisk w nic nie trafi³, strza³ w kierunku kamery
            shootDirection = (ray.GetPoint(100) - transform.position).normalized;
        }

        // Ustawianie rotacji pocisku
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        // Przemieszczanie pocisku
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
    }
}

