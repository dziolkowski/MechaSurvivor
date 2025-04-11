using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; // Obrazenia przeciwnika

    

    private void OnTriggerEnter(Collider other) {
        // Sprawdzenie, czy pocisk trafil w obiekt z tagiem "Enemy" lub "Wall"
        if (other.gameObject.CompareTag("Enemy")) {
            print("trafilem " + other);
            // Zadawanie obrazen
            IDamageable enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w przeciwnika
        }
        else if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w sciane
        }
        
    }

}
