using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionManager : MonoBehaviour
{
    [SerializeField] private List<StatUpgradeData> availableUpgrades;
    [SerializeField] private List<Button> upgradeButtons; // Przyciski na UI

    private void Start()
    {
        ShowUpgrades();
    }

    private void ShowUpgrades()
    {
        Time.timeScale = 0f; // Pauza na czas wyboru

        var chosenUpgrades = GetRandomUpgrades(upgradeButtons.Count);

        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            var upgrade = chosenUpgrades[i];
            var button = upgradeButtons[i];
            var text = button.GetComponentInChildren<Text>();
            text.text = upgrade.name;

            int index = i; // zabezpieczenie przed problemem z zamknieciem zmiennej w lambdzie
            button.onClick.RemoveAllListeners(); // usuwamy stare nasluchiwacze
            button.onClick.AddListener(() =>
            {
                PlayerUpgradeTracker.Instance.ApplyUpgrade(chosenUpgrades[index]);
                HideUpgradeUI();
            });
        }
    }

    private List<StatUpgradeData> GetRandomUpgrades(int count)
    {
        var shuffled = new List<StatUpgradeData>(availableUpgrades);
        ShuffleList(shuffled);

        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }

    private void ShuffleList(List<StatUpgradeData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void HideUpgradeUI()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1f; // Wznowienie gry po wyborze 
    }
}
