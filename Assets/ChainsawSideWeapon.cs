using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSideWeapon : MonoBehaviour
{
    [SerializeField] private int chainsawDamage = 10;
    [SerializeField] private float timeToAttack = 1f;

    private float attackCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        // Sprawdzenie, czy trafiono przeciwnika
            print("pop " + other);
        if (other.gameObject.CompareTag("Enemy") && attackCooldown >= timeToAttack) {
            DealDamage(other);
            attackCooldown = 0;
        }
    }

    void DealDamage(Collider target) {
        // Sprawdzanie, czy trafiony obiekt ma komponent EnemyHealth
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null) {
            enemyHealth.TakeDamage(chainsawDamage);
            print("pop");
        }
    }
}
