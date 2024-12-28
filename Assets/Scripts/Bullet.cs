using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; // Obra�enia przeciwnika

    /*
    private void OnCollisionEnter(Collision collision)
    {
        // Sprawdzenie, czy pocisk trafi� w obiekt z tagiem "Enemy" lub "Wall"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("trafilem");
            // Zadawanie obra�e�
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w przeciwnika
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w �cian�
        }
        else
        {
            Destroy(gameObject); // Niszczenie pocisku, gdy trafi w inne obiekty
        }
    }
    */

    private void OnTriggerEnter(Collider other) {
        // Sprawdzenie, czy pocisk trafi� w obiekt z tagiem "Enemy" lub "Wall"
        if (other.gameObject.CompareTag("Enemy")) {
            print("trafilem " + other);
            // Zadawanie obra�e�
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w przeciwnika
        }
        else if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject); // Niszczenie pocisku po trafieniu w �cian�
        }
        // TBD wymaga poprawnego ustawienia colliderów w Unity
        //else {
        //    Destroy(gameObject); // Niszczenie pocisku, gdy trafi w inne obiekty
        //}
    }

}
