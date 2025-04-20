using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab; // Prefab tarczy
    public float respawnTime = 10f; // Czas, po ktorym tarcza pojawia sie na mapie
    private GameObject currentShield;

    void Start()
    {
        SpawnShield();
    }

    void SpawnShield()
    {
        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(CheckRespawn), respawnTime);
    }

    void CheckRespawn()
    {
        if (currentShield == null)
        {
            SpawnShield();
        }
        else
        {
            Invoke(nameof(CheckRespawn), 1f);
        }
    }
}
