using System.Collections.Generic;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int level = 0;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    [SerializeField] private float expMultiplier = 1.25f;

    public UpgradeUIManager upgradeUIManager; // Przypisz w Inspectorze
    [SerializeField] private List<StatUpgradeData> allUpgrades; // Dodaj tutaj upgrady

    // Wywoluj to, gdy gracz zabije przeciwnika
    public void AddExperience(int amount)
    {
        currentExp += amount;

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;

        expToNextLevel = Mathf.RoundToInt(expToNextLevel * expMultiplier); // Koszt kolejnego levelu

        // Pokazujemy ekran wyboru upgradow
        upgradeUIManager.ShowUpgradeChoices();
    }
}
