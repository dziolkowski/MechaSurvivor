using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSideWeapon : BaseWeapon
{
    [Header("Chainsaw Settings")]
    [SerializeField] private int chainsawDamage = 10; // Ilosc zadawanych obrazen
    [SerializeField] private float areaSize = 2f; // Rozmiar obszaru dzialania 
    [SerializeField] private float timeToAttack = 1f; // Odstep czasu miedzy kolejnymi atakami
    [SerializeField] private Transform attackPoint; // Punkt odniesienia, z ktorego liczymy przesuniecie obszaru

    private float attackCooldown = 0f; // Czas, jaki minal od ostatniego ataku

    private AudioPlaylistPlayer audioPlayer;

    protected override void Start()
    {
        audioPlayer = GetComponent<AudioPlaylistPlayer>();
        weaponType = WeaponType.Chainsaw; // Ustaw typ broni tutaj
        base.Start();
    }

    void Update()
    {
        attackCooldown += Time.deltaTime; // Odliczanie czasu

        // Jezeli minal odpowiedni czas - wykonaj atak
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

        audioPlayer.PlayAudio();
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

    public override void ApplyUpgrade(StatUpgradeData upgrade)
    {
        int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
        float value = upgrade.GetValueAtLevel(level - 1);

        switch (upgrade.statType)
        {
            case StatType.Damage:
            case StatType.ChainsawDamage:
                chainsawDamage += Mathf.RoundToInt(value);
                break;
            case StatType.AreaSize:
                areaSize += value;
                break;
            case StatType.TimeToAttack:
                timeToAttack -= value;
                if (timeToAttack < 0.1f) timeToAttack = 0.1f;
                break;
            }

        Debug.Log($"{weaponType} upgraded: {upgrade.statType} +{value}");
    }
}
