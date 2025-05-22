using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab; // Prefab tarczy
    public float respawnTime = 10f; // Czas, po kttrym tarcza pojawi siê po podniesieniu
    private GameObject currentShield;

    private void OnDrawGizmosSelected() {
        DrawSphere(transform.position, 1f);
    }

    private void DrawSphere(Vector3 position, float radius) {
        Gizmos.color = Color.red; // Set the color of the gizmo
        Gizmos.DrawWireSphere(position, radius); // Draw the sphere gizmo
    }


    void Start()
    {
        SpawnShield();
    }

    void SpawnShield()
    {
        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);

        // Przekazujemy referencjê do spawnera
        ShieldPickup shieldScript = currentShield.GetComponent<ShieldPickup>();
        if (shieldScript != null)
        {
            shieldScript.spawner = this;
        }
    }

    // Wolane przez tarcze, gdy zostanie podniesiona
    public void OnShieldPickedUp()
    {
        currentShield = null;
        Invoke(nameof(SpawnShield), respawnTime);
    }
}
