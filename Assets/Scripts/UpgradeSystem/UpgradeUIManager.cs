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

    [System.Serializable]
    public class UpgradeLevelIndicatorGroup
    {
        public List<Image> indicators; // 5 kwadratow na upgrade
    }

    public List<UpgradeLevelIndicatorGroup> upgradeLevelIndicators;
    public List<StatUpgradeData> allUpgrades; // Wszystkie upgrady dostepne w grze

    private List<StatUpgradeData> lastUpgradeChoices = new(); // Zapamietane poprzednie upgrady

    private void Start()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        else
            Debug.LogError("Upgrade panel is NOT assigned!");
    }

    public void ShowUpgradeChoices()
    {
        List<StatUpgradeData> availableUpgrades = GetRandomUpgradeChoices(3);

        if (availableUpgrades.Count == 0)
        {
            Debug.LogWarning("No available upgrades found!");
            return;
        }

        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        Time.timeScale = 0f; // Pauza 

        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            if (i >= upgradeButtons.Count)
            {
                Debug.LogWarning($"No button available for upgrade index {i}");
                continue;
            }

            StatUpgradeData upgrade = availableUpgrades[i];
            Debug.Log($"Setting up upgrade {i}: {upgrade.upgradeTitle}");

            if (upgradeButtons[i] != null)
            {
                upgradeButtons[i].gameObject.SetActive(true);

                if (icons.Count > i && icons[i] != null)
                    icons[i].sprite = upgrade.icon;
                if (titles.Count > i && titles[i] != null)
                    titles[i].text = upgrade.upgradeTitle;
                if (descriptions.Count > i && descriptions[i] != null)
                    descriptions[i].text = upgrade.description;

                // Pokazujemy poziomy upgradu
                if (upgradeLevelIndicators.Count > i && upgradeLevelIndicators[i] != null)
                {
                    int currentLevel = PlayerUpgradeTracker.Instance != null ?
                        PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade) : 0;

                    Debug.Log($"Upgrade '{upgrade.upgradeTitle}' current level: {currentLevel}");

                    var group = upgradeLevelIndicators[i].indicators;
                    for (int j = 0; j < group.Count; j++)
                    {
                        if (group[j] != null)
                            group[j].gameObject.SetActive(j < currentLevel); // Ilosc kwadratow oznacza poziom upgradu
                    }
                }
                else
                {
                    Debug.LogWarning($"No level indicators for button {i}");
                }

                int index = i;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(availableUpgrades[index]));
            }
            else
            {
                Debug.LogWarning($"Upgrade button {i} is null");
            }
        }

        // Ukryj niewykorzystane przyciski
        for (int i = availableUpgrades.Count; i < upgradeButtons.Count; i++)
        {
            if (upgradeButtons[i] != null)
                upgradeButtons[i].gameObject.SetActive(false);
        }
    }

    void ApplyUpgrade(StatUpgradeData upgrade)
    {
        Debug.Log("Upgrade applied: " + upgrade.upgradeTitle);
        if (PlayerUpgradeTracker.Instance != null)
            PlayerUpgradeTracker.Instance.ApplyUpgrade(upgrade);
        else
            Debug.LogError("PlayerUpgradeTracker instance is missing!");

        HideUpgradeUI(); // Zamknij panel
    }

    void HideUpgradeUI()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Time.timeScale = 1f; // Wznowienie gry
    }

    private List<StatUpgradeData> GetRandomUpgradeChoices(int count)
    {
        List<StatUpgradeData> validUpgrades = new();

        // Zbieramy tylko upgrady, ktore nie osiagnely 5 poziomu
        foreach (var upgrade in allUpgrades)
        {
            if (PlayerUpgradeTracker.Instance == null) continue;

            int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
            if (level >= 5) continue;

            // Sprawdzamy czy upgrade dotyczy broni posiadanej przez gracza
            if (!upgrade.isPlayerUpgrade)
            {
                if (WeaponManager.Instance != null && WeaponManager.Instance.HasWeapon(upgrade.weaponType))
                {
                    validUpgrades.Add(upgrade);
                }
            }
            else
            {
                // Upgrady gracza
                validUpgrades.Add(upgrade);
            }
        }

        // Tasujemy aby zapewnic pelna losowosc
        ShuffleList(validUpgrades);

        List<StatUpgradeData> selected = new();
        HashSet<string> usedWeaponStatCombos = new(); // Unikalna kombinacja: weaponType + stat
        HashSet<string> usedPlayerStats = new(); // Zeby uniknac powtorek upgrade'ow playera

        foreach (var upgrade in validUpgrades)
        {
            // Tworzymy unikalny klucz
            string key = upgrade.isPlayerUpgrade
                ? $"PLAYER_{upgrade.statType}"
                : $"{upgrade.weaponType}_{upgrade.statType}";

            // Brak mozliwosci pojawienia siê upgradu z ta sama statystyka dla tej samej broni
            if (upgrade.isPlayerUpgrade)
            {
                if (usedPlayerStats.Contains(key)) continue;
                usedPlayerStats.Add(key);
            }
            else
            {
                if (usedWeaponStatCombos.Contains(key)) continue;
                usedWeaponStatCombos.Add(key);
            }

            selected.Add(upgrade);
            if (selected.Count >= count)
                break;
        }

        // Zapamietujemy wybrane upgrady aby uniknac ich w nastepnym pokazie
        lastUpgradeChoices = new List<StatUpgradeData>(selected);
        return selected;
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random(); // Lepsze losowanie 
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
