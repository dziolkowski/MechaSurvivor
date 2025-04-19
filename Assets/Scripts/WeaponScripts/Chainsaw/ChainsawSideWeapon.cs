using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSideWeapon : MonoBehaviour
{
    [SerializeField] private int chainsawDamage = 10; // Ilosc zadawanych obrazen
    [SerializeField] private float timeToAttack = 1f; // Odstep czasu miedzy atakami
    [SerializeField] private float damageRadius = 2f; // Promien obszaru dzialania pily

    private float attackCooldown = 0f; // Licznik odstepu czasu miedzy atakami

    void Update()
    {
        attackCooldown += Time.deltaTime; // Odliczanie czasu

        // Jezeli minelo wystarczajaco czasu od ostatniego ataku
        if (attackCooldown >= timeToAttack)
        {
            AttackEnemiesInRange(); // Wykonaj atak
            attackCooldown = 0f; // Zresetuj licznik
        }
    }

    // Metoda odpowiadajaca za atakowanie przeciwnikow w zasiegu
    void AttackEnemiesInRange()
    {
        // Znajdz wszystkie obiekty w promieniu damageRadius
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider hit in hits)
        {
            // Sprawdz, czy trafiony obiekt ma tag "Enemy"
            if (hit.CompareTag("Enemy"))
            {
                DealDamage(hit.gameObject); // Zadaj obrazenia
            }
        }
    }

    // Metoda do zadawania obrazen przeciwnikowi
    void DealDamage(GameObject target)
    {
        IDamageable enemyHealth = target.GetComponent<IDamageable>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(chainsawDamage); // Wywolaj obrazenia
        }
    }

    // Rysowanie zasiegu obrazen w edytorze Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;                            
        Gizmos.DrawWireSphere(transform.position, damageRadius); 
    }
}
