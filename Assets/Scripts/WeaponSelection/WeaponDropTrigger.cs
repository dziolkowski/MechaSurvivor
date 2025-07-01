using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            var weaponDropManager = FindObjectOfType<WeaponDropManager>();
            if (weaponDropManager != null && weaponDropManager.HasFreeSlot()) // Upewnianie czy sa sloty
            {
                weaponDropManager.ShowDropOptions();
            }

            // Po uzyciu zniszczenie triggera
            Destroy(gameObject);
        }
    }
}

