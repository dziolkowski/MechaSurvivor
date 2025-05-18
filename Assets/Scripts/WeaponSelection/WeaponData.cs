using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/WeaponData" )]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public GameObject topSlotPrefab; // Bron na czubku
    public GameObject sideSlotPrefab; // Bron na korpusie

    public WeaponType weaponType;
}
