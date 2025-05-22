using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerShield : MonoBehaviour
{
    public static PlayerShield Instance { get; private set; }

    private int shieldPoints = 0; // Obecna ilosc tarczy
    public int maxShieldPoints = 0;

    public int CurrentShieldPoints => shieldPoints;
    public int MaxShieldPoints => maxShieldPoints;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }



    public bool HasShield()
    {
        return shieldPoints > 0;
    }


    public void AddShield(int amount)
    {
        shieldPoints = Mathf.Min(shieldPoints + amount, maxShieldPoints);
        Debug.Log("Tarcza doladowana! " + shieldPoints + " / " + maxShieldPoints);
    }

    // Upgrade tarczy
    public void UpgradeShield(int amount)
    {
        maxShieldPoints += amount;
        shieldPoints = maxShieldPoints; // Pe³ne na³adowanie po upgrade
        Debug.Log("Tarcza ulepszona! Nowy max: " + maxShieldPoints);
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
