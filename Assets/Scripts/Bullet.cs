using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; // Obra¿enia przeciwnika

    void OnCollisionEnter(Collision collision)
    {
        // Sprawdzenie, czy pocisk trafi³ w obiekt z tagiem "Enemy" lub "Wall"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Zadawanie obra¿eñ
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w przeciwnika
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w œcianê
        }
        else
        {
            Destroy(gameObject); // Niszczenie pocisku, gdy trafi w inne obiekty
        }
    }
}
