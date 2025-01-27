using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab to spawn
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numberOfEnemies = 5;
    [SerializeField] private int maxWave = 10;
    [SerializeField] private float timePassed = 0f; // time in seconds until spawner starts working
    [SerializeField] private float timeToSpawnWave = 5f; // time between each wave

    private int currentWave = 0;
    //[SerializeField] private bool spawnEnemies = false; // implement in future to externally set spawning enemies

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    void Start()
    {

    }
    void Update() {
        timePassed += Time.deltaTime;
        if (timePassed > timeToSpawnWave) {
            if (currentWave < maxWave) {
                SpawnEnemies();
                timePassed = 0f;
                currentWave++;
                print("Wave: " + currentWave);
            }
            else if (currentWave == maxWave) {
                gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++) {
            Vector3 origin = transform.position;
            Vector3 range = transform.localScale / 2.0f;
            Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                              Random.Range(-range.y, range.y),
                                              Random.Range(-range.z, range.z));
            Vector3 randomCoordinate = new Vector3(origin.x + randomRange.x, 0, origin.z + randomRange.z);
            // Spawnowanie przeciwnika w wylosowanej pozycji
            Instantiate(enemyPrefab, randomCoordinate, Quaternion.identity);
        }
    }
}
