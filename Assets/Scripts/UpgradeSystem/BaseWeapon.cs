using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponType weaponType;

    protected virtual void Start()
    {
        WeaponManager.Instance?.RegisterWeapon(this);
    }

    public virtual void ApplyUpgrade(StatUpgradeData upgrade)
    {
        // Ta funkcja bedzie nadpisywana w kazdej broni
    }
}
