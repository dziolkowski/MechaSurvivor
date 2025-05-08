using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    public int shieldValue = 50; // Wartosc tarczy

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShield shield = other.GetComponent<PlayerShield>();
            if (shield != null)
            {
                shield.AddShield(shieldValue);
                Destroy(gameObject);
            }
        }
    }
}
