using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> allWeapons = new List<WeaponData>(); //Lista wszystkich dostepnych broni
    public WeaponSlot topSlot; // Top Slot postaci
    public List<WeaponSlot> sideSlots = new List<WeaponSlot>(); // Lista wszystkich slotow Side
    public WeaponSelectionUI weaponSelectionUI;
    public SlotSelectionUI slotSelectionUI;


    private void Start()
    {
        ShowTopWeaponSelection(); // Wybor broni Top slot na poczatku gry
    }


    public void EquipTopWeapon(WeaponData weapon) // Zalozenie broni na czubku 
    { 
        topSlot.EquipWeapon(weapon);
    }


    public void EquipSideWeapon(WeaponData weapon, int sideSlotIndex) // Zalozenie broni na korpusie
    { 
        if (sideSlotIndex >= 0 && sideSlotIndex < sideSlots.Count)
        {
            sideSlots[sideSlotIndex].EquipWeapon(weapon);
        }
    }


    private void ShowTopWeaponSelection() 
    {
        weaponSelectionUI.ShowAllWeapons();
    }


    private void ShowSideWeaponSelection() 
    {
        weaponSelectionUI.ShowRandomWeapons();
    }


    // Licznik czasu, po ktorym dostajemy losowe 3 bronie do wyboru
    public float timeBetweenChoices = 30f; // Czas, po ktorym mozemy wybrac nowa bron
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenChoices) 
        {
            timer = 0f;
            ShowSideWeaponSelection();
        }
    }
}
