using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float explosionTime = 1f; // Czas trwania efektu eksplozji
    [SerializeField] private float rocketLifeTime = 3f; // Czas ¿ycia rakiety
    private bool hasExploded = false;
    private float explosionRadius;
    private float damage;
    public GameObject explosionEffect; // Prefab efektu eksplozji


    void Start()
    {
        Invoke(nameof(ExplodeByTimeout), rocketLifeTime);    
    }

    void ExplodeByTimeout() 
    {
        Explode(transform.position);
    }
    public void SetParameters(float radius, float dmg)
    {
        explosionRadius = radius;
        damage = dmg;
    }

    void OnTriggerEnter(Collider collision)
    {
        // Sprawdzenie, czy trafiono przeciwnika
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            // Eksplozja w punkcie kolizji
            Explode(transform.position);
        }
    }

    void Explode(Vector3 explosionPoint)
    {
        if (hasExploded) return;
        hasExploded = true;


        // Efekt eksplozji
        if (explosionEffect != null)
        {
            Quaternion randomRotation = Random.rotation;
            GameObject explosion = Instantiate(explosionEffect, explosionPoint, randomRotation);
            Destroy(explosion, explosionTime); // Usuniecie efektu eksplozji po X sekundach
        }

        // Znalezienie przeciwnikow w promieniu eksplozji
        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);

        //Zapamietanie przeciwnikow, ktorym zadano obrazenia
        HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();


        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                GameObject enemy = nearbyObject.gameObject;

                //Jezeli przeciwnik nie otrzymal obrazen wczesniej, to je otrzymuje
                if (!damagedEnemies.Contains(enemy))
                {
                    IDamageable enemyHealth = enemy.GetComponent<IDamageable>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
                        damagedEnemies.Add(enemy); // Przeciwnik dodany do listy trafionych
                    }
                }
            }
        }

        Destroy(gameObject); // Zniszczenie rakiety 
    }
}


