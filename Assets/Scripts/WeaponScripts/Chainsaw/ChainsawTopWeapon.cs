using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawTopWeapon : MonoBehaviour
{
    [Header("Chainsaw Settings")]
    [SerializeField] private int chainsawDamage = 10; // Ilosc zadawanych obrazen
    [SerializeField] private float areaSize = 2f; // Rozmiar obszaru dzialania 
    [SerializeField] private float timeToAttack = 1f; // Odstep czasu miedzy kolejnymi atakami
    [SerializeField] private Transform attackPoint; // Punkt odniesienia, z ktorego liczymy przesuniecie obszaru

    private float attackCooldown = 0f; // Czas, jaki minal od ostatniego ataku

    void Update()
    {
        RotateTowardsMouse(); // Obracanie broni w kierunku kursora
        attackCooldown += Time.deltaTime; // Odliczanie czasu

        // Jezeli minelo wystarczajaco czasu od ostatniego ataku
        if (attackCooldown >= timeToAttack)
        {
            AttackEnemiesInRange(); 
            attackCooldown = 0f; 
        }
    }

    // Metoda sprawdzajaca, czy w zasiegu obszaru ataku sa przeciwnicy
    void AttackEnemiesInRange()
    {
        // Srodek obszaru ataku znajduje sie przed punktem odniesienia na odleglosc areaSize
        Vector3 attackPosition = attackPoint.position + attackPoint.forward * areaSize;

        // Wyszukiwanie obiektow w promieniu areaSize
        Collider[] hits = Physics.OverlapSphere(attackPosition, areaSize);

        foreach (Collider hit in hits)
        {
            // Jezeli obiekt ma tag "Enemy", zadaj obrazenia
            if (hit.CompareTag("Enemy"))
            {
                DealDamage(hit.gameObject);
            }
        }
    }

    // Zadaje obrazenia przeciwnikowi, jezeli ma on komponent IDamageable
    void DealDamage(GameObject target)
    {
        IDamageable enemyHealth = target.GetComponent<IDamageable>();
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

    // Wizualizacja zasiegu ataku w edytorze Unity
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        // Pokazujemy srodek obszaru przed bronia o wartosc areaSize
        Vector3 attackPosition = attackPoint.position + attackPoint.forward * areaSize;
        Gizmos.DrawWireSphere(attackPosition, areaSize);
    }
}
