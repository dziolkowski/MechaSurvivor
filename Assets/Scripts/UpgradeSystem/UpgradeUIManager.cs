using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public List<Button> upgradeButtons;
    public List<Image> icons;
    public List<TMP_Text> titles;
    public List<TMP_Text> descriptions;
    public List<List<Image>> upgradeLevelIndicators; // 3 listy po 5 obrazkow ka¿da

    public List<StatUpgradeData> allUpgrades; // Wszystkie upgrady dostepne w grze

    private List<StatUpgradeData> availableUpgrades = new();

    void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgradeChoices(List<StatUpgradeData> availableUpgrades)
    {
        upgradePanel.SetActive(true); // Pokazujemy ekran upgradu

        // Przycinamy, zeby nie przekroczyc ilosci przyciskow:
        if (availableUpgrades.Count > upgradeButtons.Count)
        {
            availableUpgrades = availableUpgrades.GetRange(0, upgradeButtons.Count);
        }

        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            StatUpgradeData upgrade = availableUpgrades[i];

            upgradeButtons[i].gameObject.SetActive(true);
            icons[i].sprite = upgrade.icon;
            titles[i].text = upgrade.upgradeTitle;
            descriptions[i].text = upgrade.description;

            // Aktualizacja poziomow upgradu
            int currentLevel = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
            for (int j = 0; j < upgradeLevelIndicators[i].Count; j++)
            {
                upgradeLevelIndicators[i][j].color = j < currentLevel ? Color.green : Color.gray;
            }

            int index = i;
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(availableUpgrades[index]));
        }

        // Ukryj niewykorzystane przyciski jesli lista upgradow jest krotsza niz ilosc slotow
        for (int i = availableUpgrades.Count; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }


    void ApplyUpgrade(StatUpgradeData upgrade)
    {
        Debug.Log("Upgrade applied: " + upgrade.upgradeTitle);
        PlayerUpgradeTracker.Instance.ApplyUpgrade(upgrade);
        //HideUpgradeUI();  // jesli masz metode do ukrycia UI
    }

    void SelectUpgrade(StatUpgradeData upgrade)
    {
        PlayerUpgradeTracker.Instance.ApplyUpgrade(upgrade);

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private List<StatUpgradeData> FilterValidUpgrades()
    {
        List<StatUpgradeData> valid = new();

        foreach (var upgrade in allUpgrades)
        {
            int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
            if (level >= 5) continue; // Maksymalnie 5 poziomow

            if (!upgrade.isPlayerUpgrade) // Broñ
            {
               if (WeaponManager.Instance.HasWeapon(upgrade.weaponType))
                    valid.Add(upgrade);
            }
            else // Gracz
            {
                valid.Add(upgrade);
            }
        }

        // Jesli nic nie znaleziono, dodaj awaryjne 
        if (valid.Count == 0)
        {
            foreach (var upgrade in allUpgrades)
            {
                if (upgrade.isPlayerUpgrade &&
                   (upgrade.statType == StatType.MaxHealth || upgrade.statType == StatType.ShieldRegen))
                {
                    valid.Add(upgrade);
                }
            }
        }

        return valid;
    }


    private List<StatUpgradeData> GetUniqueStatUpgrades(List<StatUpgradeData> upgrades)
    {
        var unique = new List<StatUpgradeData>();
        var seen = new HashSet<(WeaponType, StatType)>();

        foreach (var upgrade in upgrades)
        {
            var key = (upgrade.weaponType, upgrade.statType);
            if (!seen.Contains(key))
            {
                unique.Add(upgrade);
                seen.Add(key);
            }
        }

        return unique;
    }
}
