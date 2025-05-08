using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalBulletHandler : MonoBehaviour
{
    private int damage;

    public void Initialize(int dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall")) {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null) {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else return;
    }
} 
    

