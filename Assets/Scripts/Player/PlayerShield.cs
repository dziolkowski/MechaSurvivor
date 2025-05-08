using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public int shieldPoints = 0; // Obecna ilosc tarczy
    private int maxShieldPoints = 0;
    public int CurrentShieldPoints => shieldPoints;
    public int MaxShieldPoints => maxShieldPoints;


    public bool HasShield()
    {
        return shieldPoints > 0;
    }


    public void AddShield(int amount)
    {
        shieldPoints = amount;
        maxShieldPoints = amount;
        Debug.Log("Tarcza aktywna! " + shieldPoints + " punktow");
    }

    public bool AbsorbDamage(int damage)
    {
        if (shieldPoints > 0)
        {
            shieldPoints -= damage;

            if (shieldPoints < 0)
                shieldPoints = 0;

            Debug.Log($"Tarcza pochlonela {damage} obrazen. Pozostlo: {shieldPoints}");

            return true; // Tarcza przejmuje obrazenia
        }

        return false; // Gdy nie ma tarczy Gracz traci HP
    }

    public void RemoveShield()
    {
        shieldPoints = 0;
        Debug.Log("Tarcza zniknela");
    }

    
}
