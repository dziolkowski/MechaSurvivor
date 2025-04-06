using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public int damage; // Obrazenia przypisywane przez bron
    public MonoBehaviour weaponController; 

    void OnTriggerEnter(Collider other)
    {
        // Sprawdzanie czy obeikt implementuje interfejs IDamageable
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
