using System.Collections.Generic;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int level = 0;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public UpgradeUIManager upgradeUIManager; // Przypisz w Inspectorze
    [SerializeField] private List<StatUpgradeData> allUpgrades; // Dodaj tutaj upgrade'y

    // Wywo³uj to, gdy gracz zabije przeciwnika
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

        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.25f); // Koszt kolejnego levelu

        // Pokazujemy ekran wyboru upgradow
        upgradeUIManager.ShowUpgradeChoices(allUpgrades);
    }
}
