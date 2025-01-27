using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float explosionTime = 1f;
    private float explosionRadius;
    private float damage;
    public GameObject explosionEffect; // Prefab efektu eksplozji

    public void SetParameters(float radius, float dmg)
    {
        explosionRadius = radius;
        damage = dmg;
    }

    void OnTriggerEnter(Collider collision)
    {
        print("test explode");
        // Sprawdzenie, czy trafiono przeciwnika
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            // Eksplozja w punkcie kolizji
            Vector3 rocketPosition = transform.position;
            Explode(rocketPosition);
        }
    }

    void Explode(Vector3 explosionPoint)
    {
        // Efekt eksplozji
        if (explosionEffect != null)
        {
            Quaternion randomRotation = Random.rotation;
            GameObject explosion = Instantiate(explosionEffect, explosionPoint, randomRotation);
            Destroy(explosion, explosionTime); // Usuniêcie efektu eksplozji po X sekundach
        }

        // Znalezienie przeciwników w promieniu eksplozji
        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
               EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
               if (enemyHealth != null)
               {
                   enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
               }
            }
        }

        Destroy(gameObject); // Zniszczenie rakiety
    }
}

