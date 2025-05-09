using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    private HashSet<WeaponType> ownedWeapons = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddWeapon(WeaponType weaponType)
    {
        ownedWeapons.Add(weaponType);
    }

    public bool HasWeapon(WeaponType weaponType)
    {
        return ownedWeapons.Contains(weaponType);
    }
}
