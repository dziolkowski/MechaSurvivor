using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public Image weaponIconImage; // Ikona broni
    private WeaponData weaponData; // Dane broni
    private WeaponSelectionUI selectionUI; // Odwolanie do UI


    public void Setup(WeaponSelectionUI ui, WeaponData data) // Ustawienia przycisku 
    {
        selectionUI = ui;
        weaponData = data;
        weaponIconImage.sprite = data.weaponIcon;
    }


    public void OnButtonClick() // Funkcja wywolana po kliknieciu
    { 
        selectionUI.OnWeaponSelected(weaponData);
    }
}
