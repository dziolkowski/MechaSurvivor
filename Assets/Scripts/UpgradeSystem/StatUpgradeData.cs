using UnityEngine;

public enum WeaponType
{
    Gun, Laser, Shotgun, Chainsaw, RocketLauncher, Player
}

public enum StatType
{
    Damage, FireRate, ProjectileSize, ProjectileAmount, BulletSpeed,
    LaserWidth, RocketSpeed, ExplosionRadius, AreaSize, TimeToAttack,
    MaxHealth, ShieldRegen, BulletDamage
}

[CreateAssetMenu(fileName = "New Stat Upgrade", menuName = "Upgrades/StatUpgrade")]
public class StatUpgradeData : ScriptableObject
{
    public string upgradeTitle;
    public string description;
    public Sprite icon;

    public WeaponType weaponType;     // Dla broni lub gracza (Player)
    public StatType statType;         // Konkretny stat, np. Damage, MaxHealth itp.

    public float[] upgradeValues = new float[5]; // Poziomy 0–4

    public bool isPlayerUpgrade;      // true = dotyczy gracza, false = broni

    public float GetValueAtLevel(int level)
    {
        if (level >= 0 && level < upgradeValues.Length)
            return upgradeValues[level];
        return 0f;
    }
}
