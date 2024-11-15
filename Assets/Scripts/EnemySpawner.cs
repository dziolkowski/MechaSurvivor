using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Prefab przeciwnika, który bêdziemy instancjonowaæ
    public GameObject enemyPrefab;

    // Zakres, w którym przeciwnicy s¹ spawnowani
    public float spawnRangeX = 10f;
    public float spawnRangeZ = 10f;

    // Iloœæ spawnowanych przeciwników
    public int numberOfEnemies = 3;

    void Start()
    {
        // Uruchamiamy spawnowanie przeciwników
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Losowanie pozycji X i Z w zadanym zakresie
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            Vector3 spawnPosition = new Vector3(randomX, 0, randomZ);

            // Spawnowanie przeciwnika w wylosowanej pozycji
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
