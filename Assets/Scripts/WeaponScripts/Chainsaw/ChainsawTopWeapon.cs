using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawTopWeapon : MonoBehaviour
{
    [SerializeField] private int chainsawDamage = 10; // Ilosc zadawanych obrazen
    [SerializeField] private float timeToAttack = 1f; // Czas miedzy atakami
    [SerializeField] private float damageRadius = 2f; // Promien obszaru zadawania obrazen

    private float attackCooldown = 0f; // Licznik czasu od ostatniego ataku

    void Update()
    {
        RotateTowardsMouse(); // Obracanie broni w kierunku kursora
        attackCooldown += Time.deltaTime; // Odliczanie czasu

        // Jezeli minelo wystarczajaco czasu od ostatniego ataku
        if (attackCooldown >= timeToAttack)
        {
            AttackEnemiesInRange(); // Zadaj obrazenia przeciwnikom w poblizu
            attackCooldown = 0f; // Zresetuj licznik
        }
    }

    // Metoda atakujaca przeciwnikow w poblizu
    void AttackEnemiesInRange()
    {
        // Znajdz wszystkie obiekty w promieniu damageRadius
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                DealDamage(hit.gameObject); // Zadaj obrazenia przeciwnikowi
            }
        }
    }

    // Metoda do zadawania obrazen przeciwnikowi
    void DealDamage(GameObject target)
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(chainsawDamage);
        }
    }

    // Obracanie broni w kierunku kursora myszy
    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Utrzymanie tej samej wysokosci

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }
        }
    }

    // Rysowanie promienia obrazen w edytorze Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
