using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab to spawn
    public GameObject enemyPrefab;

    // List of spawned waves
    //public Dictionary<int, Wave> listOfWaves;
    public int numberOfEnemies = 5;

    private int currentWave = 0;
    private float timePassed = 0f;
    [SerializeField] private float timeToSpawnWave = 5f;

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    void Start()
    {
        // Uruchamiamy spawnowanie przeciwników
        //SpawnEnemies();

        // get wave from list of waves
        // go through enemies inside that wave
        // 
    }
    void Update() {
        timePassed += Time.deltaTime;
        if (timePassed > timeToSpawnWave) {
            SpawnEnemies();
            timePassed = 0f;
            currentWave++;
            print("Wave: " + currentWave);
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
