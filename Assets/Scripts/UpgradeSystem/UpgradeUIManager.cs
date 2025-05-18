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
                            group[j].gameObject.SetActive(j < currentLevel); // Ilosc kwadratow oznacza poziom ugradu
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
        List<StatUpgradeData> weaponUpgrades = new();
        List<StatUpgradeData> playerUpgrades = new();

        foreach (var upgrade in allUpgrades)
        {
            if (PlayerUpgradeTracker.Instance == null) continue;

            int level = PlayerUpgradeTracker.Instance.GetUpgradeLevel(upgrade);
            if (level >= 5) continue;

            if (!upgrade.isPlayerUpgrade)
            {
                if (WeaponManager.Instance != null &&
                    WeaponManager.Instance.HasWeapon(upgrade.weaponType) &&
                    !weaponUpgrades.Exists(u => u.weaponType == upgrade.weaponType))
                {
                    weaponUpgrades.Add(upgrade);
                }
            }
            else
            {
                playerUpgrades.Add(upgrade);
            }
        }

        // Tasujemy
        ShuffleList(weaponUpgrades);
        ShuffleList(playerUpgrades);

        List<StatUpgradeData> filtered = new();

        // Dodajemy rozne upgrady i pomijamy te, ktore byly ostatnio pokazane
        foreach (var u in weaponUpgrades)
        {
            if (!lastUpgradeChoices.Contains(u))
                filtered.Add(u);
        }
        foreach (var u in playerUpgrades)
        {
            if (!lastUpgradeChoices.Contains(u))
                filtered.Add(u);
        }

        ShuffleList(filtered);

        // Jesli nie ma wystarczajacej liczby unikalnych, dodajemy reszte
        if (filtered.Count < count)
        {
            List<StatUpgradeData> fallback = new();
            fallback.AddRange(weaponUpgrades);
            fallback.AddRange(playerUpgrades);
            ShuffleList(fallback);

            foreach (var u in fallback)
            {
                if (!filtered.Contains(u))
                    filtered.Add(u);
                if (filtered.Count >= count) break;
            }
        }

        List<StatUpgradeData> final = filtered.GetRange(0, Mathf.Min(count, filtered.Count));
        lastUpgradeChoices = new List<StatUpgradeData>(final);
        return final;
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
