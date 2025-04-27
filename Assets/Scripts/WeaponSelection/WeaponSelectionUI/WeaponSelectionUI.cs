using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class WeaponSelectionUI : MonoBehaviour
{
    public GameObject selectionPanel; // Panel wyboru broni
    public GameObject weaponButtonPrefab; // Prefab przycisku broni
    public Transform weaponButtonParent; // Gdzie beda tworzone przyciski
    public WeaponManager weaponManager; // Odwolanie do WeaponManagera
    public SlotSelectionUI slotSelectionUI; // Odwolanie do SlotSelectionUI

    private bool choosingTopWeapon = true; // Czy wybieramy bron na czubek

    // Funkcja do wyswietlenia wyboru wszystkich broni (na start)
    public void ShowAllWeapons()
    {
        ClearButtons();

        choosingTopWeapon = true;

        foreach (var weapon in weaponManager.allWeapons)
        {
            CreateWeaponButton(weapon);
        }

        selectionPanel.SetActive(true);
    }

    // Funkcja do wyswietlenia wyboru 3 losowych broni (w trakcie gry)
    public void ShowRandomWeapons()
    {
        ClearButtons();

        choosingTopWeapon = false;

        List<WeaponData> randomWeapons = GetRandomWeapons(3);

        foreach (var weapon in randomWeapons)
        {
            CreateWeaponButton(weapon);
        }

        selectionPanel.SetActive(true);
    }

    // Tworzenie przycisku dla danej broni
    private void CreateWeaponButton(WeaponData weaponData)
    {
        GameObject buttonObj = Instantiate(weaponButtonPrefab, weaponButtonParent);
        WeaponButton button = buttonObj.GetComponent<WeaponButton>();
        button.Setup(this, weaponData);
    }

    // Czyszczenie starych przyciskow
    private void ClearButtons()
    {
        foreach (Transform child in weaponButtonParent)
        {
            Destroy(child.gameObject);
        }
    }

    // Funkcja ktora wywolujemy po kliknieciu broni
    public void OnWeaponSelected(WeaponData weaponData)
    {
        if (choosingTopWeapon)
        {
            weaponManager.EquipTopWeapon(weaponData);
        }
        else
        {
            ShowSlotSelection(weaponData);
        }

        selectionPanel.SetActive(false);
    }

    // Pokazywanie wyboru slotu dla side weapon
    private void ShowSlotSelection(WeaponData weaponData)
    {
        slotSelectionUI.ShowSlotSelection(weaponData);
    }

    // Funkcja do losowania broni
    private List<WeaponData> GetRandomWeapons(int count)
    {
        List<WeaponData> copy = new List<WeaponData>(weaponManager.allWeapons);
        List<WeaponData> randomList = new List<WeaponData>();

        for (int i = 0; i < count; i++)
        {
            if (copy.Count == 0) break;
            int index = Random.Range(0, copy.Count);
            randomList.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return randomList;
    }
}
