using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Obra¿enie zadawane przez pocisk
    public int damage = 25; 

    void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            //Zadawanie obra¿eñ przeciwnikom
            enemyHealth.TakeDamage(damage); 

            //Niszczenie pocisku po trafieniu
            Destroy(gameObject); 
        }
    }
}
