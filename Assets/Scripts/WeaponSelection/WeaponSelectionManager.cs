using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour
{
    public List<WeaponData> availableWeapons; // Lista wszystkich broni
    public Transform topSlot; // Referencja do Top Slot
    public List<Button> topWeaponButtons; // Recznie przypisane przyciski w UI
    public WeaponDropManager dropManager; // Odniesienie do systemu dropu
    public Image topSlotIconHUD; // Ikona zmieniana na interfejsie

    void Start()
    {
        Time.timeScale = 0; // Pauza

        if (topWeaponButtons.Count != availableWeapons.Count)
        {
            Debug.LogWarning("Liczba przycisków nie odpowiada liczbie dostêpnych broni!");
        }

        for (int i = 0; i < availableWeapons.Count; i++)
        {
            var weapon = availableWeapons[i];
            var button = topWeaponButtons[i];

            // Ustaw ikone na przycisku 
            Image iconImage = button.transform.Find("Icon").GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = weapon.icon;
            }

            // Potrzebujemy tymczasowej zmiennej, zeby uniknac problemu z referencja w petli
            WeaponData capturedWeapon = weapon;

            button.onClick.RemoveAllListeners(); // Czyscimy stare eventy
            button.onClick.AddListener(() => SelectTopWeapon(capturedWeapon));
        }
    }

    void SelectTopWeapon(WeaponData weapon)
    {
        topSlot.GetComponent<WeaponSlot>().AssignWeapon(weapon);

        var baseWeapon = topSlot.GetComponentInChildren<BaseWeapon>();
        if (baseWeapon != null)
        {
            WeaponManager.Instance.RegisterWeapon(baseWeapon);
        }

        // Aktualizowanie ikony na interfejsie
        if (topSlotIconHUD != null)
        {
            topSlotIconHUD.sprite = weapon.icon;
            topSlotIconHUD.enabled = true; // Upewniamy sie, ze jest widoczna
        }

        dropManager.Initialize(availableWeapons); // Przekazujemy pozostale bronie do drop systemu
        gameObject.SetActive(false); // Chowamy UI wyboru

        Time.timeScale = 1; // Wznowienie gry
    }
}
