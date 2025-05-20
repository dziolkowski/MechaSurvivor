using System.Collections;
using UnityEngine;

public class LaserSideWeapon : BaseWeapon
{
    [Header("Laser Settings")]
    [SerializeField] private float laserWidth = 0.1f; // Szerokosc lasera 
    public GameObject laserPrefab; // Prefab lasera
    public Transform laserOrigin; // Punkt poczatkowy lasera
    public float laserRange = 100f; // Maksymalny zasieg lasera
    public float fireRate = 0.1f; // Czestotliwosc strzelania
    public int laserDamage = 10; // Obrazenia zadawane przez laser
    public LayerMask enemyLayer; // Warstwa przeciwnikow
    public LayerMask obstacleLayer; // Warstwa przeszkod (np. sciany)

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
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        if (currentLaser == null) // Tworzenie instancji lasera, jesli jeszcze nie istnieje
        {
            currentLaser = Instantiate(laserPrefab, laserOrigin.position, Quaternion.identity);
            currentLaser.SetActive(false); // Laser na poczatku jest niewidoczny
        }

        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserRange, enemyLayer | obstacleLayer);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance)); // Sortowanie trafien

        Vector3 targetPoint = laserOrigin.position + laserOrigin.forward * laserRange;

        foreach (var hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
            {
                targetPoint = hit.point;
                break;
            }

            if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
            {
                DealDamage(hit.collider);
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
        IDamageable enemyHealth = target.GetComponent<IDamageable>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(laserDamage);
        }
    }

    IEnumerator LaserEffect()
    {
        currentLaser.SetActive(true); // Pokaz laser
        yield return new WaitForSeconds(0.05f); // Czas widocznosci
        currentLaser.SetActive(false); // Ukryj laser
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


