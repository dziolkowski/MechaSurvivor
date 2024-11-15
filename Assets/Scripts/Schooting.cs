using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z kt�rego b�d� wystrzeliwane pociski
    public float bulletSpeed = 10f; // Pr�dko�� pocisku
    public float bulletLifetime = 5f; // Czas �ycia pocisku

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Lewy przycisk myszy
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Tworzenie pocisku w punkcie wystrza�u
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Obliczanie kierunku strza�u w kierunku kursora myszy
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Ustawienie celu na tej samej wysoko�ci co posta�
            Vector3 targetPosition = new Vector3(hitInfo.point.x, firePoint.position.y, hitInfo.point.z);
            Vector3 direction = (targetPosition - firePoint.position).normalized;

            // Nadanie pociskowi pr�dko�ci w kierunku kursora
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }

        // Automatyczne zniszczenie pocisku po okre�lonym czasie
        Destroy(bullet, bulletLifetime);
    }
}

