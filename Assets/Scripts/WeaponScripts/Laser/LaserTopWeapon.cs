using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTopWeapon : BaseWeapon
{
    [Header("Laser Settings")]
    [SerializeField] private float laserWidth = 0.1f; // Szerokosc lasera 
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask hitLayers; // Warstwy, ktore moga byc trafione

    private float nextFireTime;
    private GameObject currentLaser; // Instancja aktualnie uzywanego lasera



    protected override void Start()
    {
        weaponType = WeaponType.Laser; // Ustaw typ broni tutaj
        base.Start();
    }


    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        RotateWeaponTowardsCursor();

        if (Time.time >= nextFireTime) // Automatyczne strzelanie co okreslony czas
        {
            nextFireTime = Time.time + fireRate;
            ShootLaser();
        }
    }

    void RotateWeaponTowardsCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Ustawienie tej samej wysokosci co bron

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void ShootLaser()
    {
        if (currentLaser == null)
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na poczatku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, hitLayers);

        // Sortowanie trafien od najblizszego do najdalszego
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;

        foreach (var hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & hitLayers) != 0) // Sprawdzenie, czy obiekt jest w warstwach hitLayers
            {
                // Trafienie przeciwnika
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    DealDamage(hit.collider); // Zadaj obrazenia
                    targetPoint = hit.point; // Laser zatrzymuje sie na przeciwniku
                    break; // Trafienie przeciwnika zatrzymuje promien
                }

                // Trafienie przeszkody 
                targetPoint = hit.point; // Skrocenie lasera do przeszkody
                break; // Przeszkoda zatrzymuje promien
            }
        }

        AdjustLaser(targetPoint);
        StartCoroutine(LaserEffect());
    }


    void AdjustLaser(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - laserOrigin.position;
        float distance = direction.magnitude;

        currentLaser.transform.position = laserOrigin.position + direction / 2f;
        currentLaser.transform.localScale = new Vector3(laserWidth, currentLaser.transform.localScale.y, distance);
        currentLaser.transform.rotation = Quaternion.LookRotation(direction);
    }

    void DealDamage(Collider target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(laserDamage);
        }
    }


    IEnumerator LaserEffect()
    {
        currentLaser.SetActive(true);
        yield return new WaitForSeconds(0.05f); // Czas widocznosci lasera
        currentLaser.SetActive(false);
    }

    public override void ApplyUpgrade(StatUpgradeData upgrade)
    {
        int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
        float value = upgrade.GetValueAtLevel(level - 1);

        switch (upgrade.statType)
        {
            case StatType.Damage:
            case StatType.LaserDamage:
                laserDamage += Mathf.RoundToInt(value);
                break;
            case StatType.FireRate:
                fireRate -= value;
                if (fireRate < 0.1f) fireRate = 0.1f;
                break;
            case StatType.LaserWidth:
                laserWidth += value;
                break;
        }

        Debug.Log($"{weaponType} upgraded: {upgrade.statType} +{value}");
    }
}

public interface IDamageable
{
    void TakeDamage(int damage);
}

