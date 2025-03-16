using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawSideWeapon : MonoBehaviour
{
    [SerializeField] private int chainsawDamage = 10;
    [SerializeField] private float timeToAttack = 1f;

    private float attackCooldown = 0f;
    private List<GameObject> targets;

    private void Start() {
        targets = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        attackCooldown += Time.deltaTime;
        // Sprawdzenie, czy trafiono przeciwnika
        if (attackCooldown >= timeToAttack) {
            CleanUpList();
            foreach (GameObject obj in targets) {
                DealDamage(obj);
                attackCooldown = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Check if the other collider has a GameObject
        if (other.gameObject != null && other.gameObject.CompareTag("Enemy")) {
            // Add the GameObject to the list
            targets.Add(other.gameObject);
            Debug.Log("Added: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other) {
        // Check if the other collider has a GameObject
        if (other.gameObject != null && other.gameObject.CompareTag("Enemy")) {
            // Remove the GameObject from the list
            targets.Remove(other.gameObject);
            Debug.Log("Removed: " + other.gameObject.name);
        }
    }

    void DealDamage(GameObject target) {
        // Sprawdzanie, czy trafiony obiekt ma komponent EnemyHealth
        IDamageable enemyHealth = target.GetComponent<IDamageable>();
        if (enemyHealth != null) {
            enemyHealth.TakeDamage(chainsawDamage);
        }
    }
    // Method to clean up the list by removing destroyed objects
    private void CleanUpList() {
        targets.RemoveAll(obj => obj == null);
    }

}
