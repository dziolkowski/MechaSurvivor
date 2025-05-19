using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeTracker : MonoBehaviour
{
    public static PlayerUpgradeTracker Instance { get; private set; }

    private Dictionary<StatUpgradeData, int> upgradeLevels = new();

    public static event Action<StatUpgradeData> OnUpgradeApplied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ApplyUpgrade(StatUpgradeData upgrade)
    {
        if (!upgradeLevels.ContainsKey(upgrade))
            upgradeLevels[upgrade] = 1;
        else
            upgradeLevels[upgrade]++;

        Debug.Log("Applied upgrade: " + upgrade.upgradeTitle + " - New level: " + upgradeLevels[upgrade]);

        if (upgrade.isPlayerUpgrade)
        {
            int newLevel = upgradeLevels[upgrade];
            float value = upgrade.GetValueAtLevel(newLevel - 1);

            if (upgrade.statType == StatType.MaxShieldPoints)
            {
                PlayerShield.Instance.UpgradeShield((int)value);
            }
            else if (upgrade.statType == StatType.MoveSpeed)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                    player.moveSpeed += value;
            }
            else if (upgrade.statType == StatType.RotationSpeed)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                    player.rotationSpeed += value;
            }

            OnUpgradeApplied?.Invoke(upgrade);
        }
        else
        {
            var weapons = WeaponManager.Instance.GetWeaponsOfType(upgrade.weaponType);
            foreach (var weapon in weapons)
            {
                weapon.ApplyUpgrade(upgrade);
            }
        }
    }

    public int GetUpgradeLevel(StatUpgradeData upgrade)
    {
        if (upgradeLevels.TryGetValue(upgrade, out int level))
            return level;
        return 0;
    }

    public int GetUpgradeLevelForStat(StatType statType)
    {
        int level = 0;
        foreach (var kvp in upgradeLevels)
        {
            if (kvp.Key.statType == statType)
                level += kvp.Value;
        }
        return level;
    }

    public int GetUpgradeLevelForWeapon(WeaponType weaponType, StatType statType)
    {
        int level = 0;
        foreach (var kvp in upgradeLevels)
        {
            if (kvp.Key.weaponType == weaponType && kvp.Key.statType == statType)
                level += kvp.Value;
        }
        return level;
    }
}
