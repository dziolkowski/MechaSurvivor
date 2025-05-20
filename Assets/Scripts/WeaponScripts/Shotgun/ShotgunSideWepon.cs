using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSideWepon : BaseWeapon
{
    [Header("Shotgun Settings")]
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform firePoint; // Punkt, z ktorego strzela shotgun
    public float fireRate = 1f; // Czas miedzy kolejnymi strzalami
    public float bulletSpeed = 20f; // Predkosc pocisku
    public float bulletLifetime = 3f; // Czas zycia pocisku
    public int projectileAmount = 4; // Liczba pociskow wystrzelonych na raz
    public float spreadAngle = 10f; // Kat rozrzutu pociskow
    public int bulletDamage = 25; // Obrazenia zadawane przez pociski
    public float projectileSize = 0.25f; // Wielkosc pocisku
    private float nextFireTime = 0f; // Czas, po ktorym mozna wystrzelic kolejne pociski



    protected override void Start()
    {
        weaponType = WeaponType.Shotgun; // Ustaw typ broni tutaj
        base.Start();
    }


    void Update()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie strzelania
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            // Generowanie losowego kata w zakresie rozrzutu
            float angle = Random.Range(-spreadAngle, spreadAngle);

            // Obrot na podstawie kata rozrzutu
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;

            // Tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.layer = LayerMask.NameToLayer("Bullet");
            bullet.transform.localScale = Vector3.one * projectileSize;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Ustawianie predkosci pocisku
            rb.velocity = bullet.transform.forward * bulletSpeed;

            bullet.AddComponent<InternalBulletHandler>().Initialize(bulletDamage);
            // Niszczenie pocisku po okreslonym czasie
            Destroy(bullet, bulletLifetime);
        }
    }

    public override void ApplyUpgrade(StatUpgradeData upgrade)
    {
        int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
        float value = upgrade.GetValueAtLevel(level - 1);

        switch (upgrade.statType)
        {
            case StatType.Damage:
            case StatType.BulletDamage:
                bulletDamage += Mathf.RoundToInt(value);
                break;
            case StatType.FireRate:
                fireRate -= value;
                if (fireRate < 0.05f) fireRate = 0.05f;
                break;
            case StatType.ProjectileSize:
                projectileSize += value;
                break;
            case StatType.ProjectileAmount:
                projectileAmount += Mathf.RoundToInt(value);
                break;
            case StatType.BulletSpeed:
                bulletSpeed += value;
                break;
        }

        Debug.Log($"{weaponType} upgraded: {upgrade.statType} +{value}");
    }
}

