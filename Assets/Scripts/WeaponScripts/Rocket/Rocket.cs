using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float explosionRadius;
    private float damage;
    public GameObject explosionEffect; // Prefab efektu eksplozji

    public void SetParameters(float radius, float dmg)
    {
        explosionRadius = radius;
        damage = dmg;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Sprawdzenie, czy trafiono przeciwnika
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Eksplozja w punkcie kolizji
            Vector3 collisionPoint = collision.contacts[0].point;
            Explode(collisionPoint);
        }
        else
        {
            // Opcjonalnie: Zniszczenie rakiety przy kolizji z innym obiektem
            Destroy(gameObject);
        }
    }

    void Explode(Vector3 explosionPoint)
    {
        // Efekt eksplozji
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, explosionPoint, Quaternion.identity);
            Destroy(explosion, 2f); // Usuniêcie efektu eksplozji po 2 sekundach
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

