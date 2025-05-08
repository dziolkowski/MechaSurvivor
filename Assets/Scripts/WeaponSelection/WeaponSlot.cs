using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public enum SlotType { Top, Side}
    public SlotType slotType;


    public void AssignWeapon(WeaponData weapon) 
    {
        GameObject prefab = slotType == SlotType.Top ? weapon.topSlotPrefab : weapon.sideSlotPrefab;
        Instantiate(prefab, transform.position, transform.rotation, transform);
    }
}
