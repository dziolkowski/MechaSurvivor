using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeTracker : MonoBehaviour
{
    // Singleton
    public static PlayerUpgradeTracker Instance;

    // Trzymamy poziomy upgradow
    public Dictionary<StatUpgradeData, int> upgradeLevels = new();

    private void Awake()
    {
        // Ustawiamy instancjê Singletona
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Jeœli ju¿ istnieje, usuwamy duplikat
        }
    }

    // Tu metoda do stosowania upgradu
    public void ApplyUpgrade(StatUpgradeData upgrade)
    {
        if (upgradeLevels.ContainsKey(upgrade))
        {
            upgradeLevels[upgrade]++;
        }
        else
        {
            upgradeLevels[upgrade] = 1;
        }

        // Mozesz dodaæ tu efekty typu: zwieksz HP
        Debug.Log($"Upgrade applied: {upgrade.name}, new level: {upgradeLevels[upgrade]}");
    }

    // Mo¿esz dodac metody do pobierania poziomu upgrade itp.
    public int GetUpgradeLevel(StatUpgradeData upgrade)
    {
        return upgradeLevels.TryGetValue(upgrade, out var level) ? level : 0;
    }
}
