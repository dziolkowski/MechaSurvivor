using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    // Lista posiadanych broni
    private List<BaseWeapon> equippedWeapons = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Dodaje bron do listy jesli nie zostala juz dodana
    public void RegisterWeapon(BaseWeapon weapon)
    {
        if (!equippedWeapons.Contains(weapon))
        {
            equippedWeapons.Add(weapon);
        }
    }

    // Sprawdza czy gracz posiada bron danego typu
    public bool HasWeapon(WeaponType type)
    {
        return equippedWeapons.Exists(w => w.weaponType == type);
    }

    // Zwraca wszystkie bronie danego typu
    public List<BaseWeapon> GetWeaponsOfType(WeaponType type)
    {
        return equippedWeapons.FindAll(w => w.weaponType == type);
    }
}
