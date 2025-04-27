using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSelectionUI : MonoBehaviour
{
    public GameObject slotSelectionPanel; // Panel wyboru slotu
    public GameObject slotButtonPrefab; // Prefab przycisku slotu
    public Transform slotButtonParent; 
    public WeaponManager weaponManager; // Odwolanie do WeaponManagera


    private WeaponData pendingWeapon; // Bron, ktora ma zostac przypieta


    public void ShowSlotSelection(WeaponData weaponData) 
    {
        ClearButtons();

        pendingWeapon = weaponData;

        for (int i = 0; i < weaponManager.sideSlots.Count; i++) 
        {
            CreateSlotButton(i);
        }

        slotSelectionPanel.SetActive(true);
    }


    private void CreateSlotButton(int slotIndex) // Tworzenie przycisku dla slotu
    {
        GameObject buttonObj = Instantiate(slotButtonPrefab, slotButtonParent);
        SlotButton button = buttonObj.GetComponent<SlotButton>();
        button.Setup(this, slotIndex);
    }


    public void OnSlotSelected(int slotIndex) // Funkcja wywolania po kliknieciu slotu
    {
        weaponManager.EquipSideWeapon(pendingWeapon, slotIndex);
        slotSelectionPanel.SetActive(false);
    }


    private void ClearButtons() // Czyszczenie starych przyciskow
    {
        foreach (Transform child in slotButtonParent)
        {
            Destroy(child.gameObject);
        }
    }
}
