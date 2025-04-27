using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType { Top, Side } // Typ slotu: top lub side
public class WeaponSlot : MonoBehaviour
{
    public SlotType slotType; // Typ slotu
    private GameObject currentWeaponModel; // Model broni przypiety do slotu
    private MonoBehaviour currentWeaponScript;// Skrypt broni przypiety do slotu

    public void EquipWeapon(WeaponData weaponData) // Funkcja do zalozenia nowej broni
    {
        if (currentWeaponModel != null) // Usuwanie broni jesli jest w slocie
            Destroy(currentWeaponModel);

        if (currentWeaponScript != null)
            Destroy(currentWeaponScript);

        // Przypisanie odpowiedniego modelu i skryptu do typu slotu
        if (slotType == SlotType.Top)
        {
            currentWeaponModel = Instantiate(weaponData.topModelPrefab, transform);
            currentWeaponScript = gameObject.AddComponent(System.Type.GetType(weaponData.topWeaponScript.name)) as MonoBehaviour;
        }
        else
        {
            currentWeaponModel = Instantiate(weaponData.sideModelPrefab, transform);
            currentWeaponScript = gameObject.AddComponent(System.Type.GetType(weaponData.sideWeaponScript.name)) as MonoBehaviour;
        }
    }
}
