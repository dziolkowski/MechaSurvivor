using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int damage = 5; // Iloœæ zadawanych obra¿eñ
    [SerializeField] private float lifetime = 10f; // Czas po jakim plama znika
    [SerializeField] private float damageCooldown = 0.1f; // Minimalny czas miêdzy zadaniem obra¿eñ

    private HashSet<Collider> playerColliders = new HashSet<Collider>(); // Przechowuje kolizje nóg
    private PlayerHealth playerHealth; // Referencja do zdrowia gracza

    private void Start()
    {
        Destroy(gameObject, lifetime); // Plama znika po okreœlonym czasie
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerColliders.Count == 0) // Tylko przy pierwszym kontakcie
            {
                playerHealth = other.GetComponentInParent<PlayerHealth>(); // Pobieramy zdrowie gracza
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }

            playerColliders.Add(other); // Dodajemy nogê do zbioru
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliders.Remove(other); // Usuwamy nogê ze zbioru

            if (playerColliders.Count == 0) // Gdy ¿adna noga nie dotyka plamy, resetujemy zdrowie
            {
                playerHealth = null;
            }
        }
    }
}
