using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemiesOnNavMesh : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnConfig
    {
        public string enemyName;          // Enemy type identifier
        public GameObject prefab;         // Enemy prefab
        public float spawnInterval = 5f;  // Time between spawns
        public bool canSpawn = false;     // Toggle spawning on/off
    }

    [SerializeField] private List<EnemySpawnConfig> enemySpawnConfigs = new List<EnemySpawnConfig>();
    [SerializeField] private float spawnRadius = 50f;         // Maximum spawn distance
    [SerializeField] private float spawnMargin = 5f;          // Minimum spawn margin
    [SerializeField] private Transform playerTransform;       // Player position reference
    [SerializeField] private LayerMask obstacleLayers;        // Layers to avoid while spawning
    [SerializeField] private int maxActiveEnemies = 100;       // Maximum number of active enemies
    [SerializeField] private int resumeSpawnThreshold = 10;   // Number of enemies to reduce before resuming spawning 

    [SerializeField] private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track active enemies

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in SpawnEnemiesOnNavMesh.");
            return;
        }

        StartSpawningAll();
    }

    private void StartSpawningAll()
    {
        foreach (var config in enemySpawnConfigs)
        {
            if (config.canSpawn)
            {
                StartCoroutine(SpawnEnemyCoroutine(config));
            }
        }
    }

    private IEnumerator SpawnEnemyCoroutine(EnemySpawnConfig config)
    {
        while (config.canSpawn)
        {
            CleanUpDestroyedEnemies(); // Remove any destroyed enemies from the list

            while (spawnedEnemies.Count >= maxActiveEnemies)
            {
                // Wait for a short interval to recheck the conditions
                yield return new WaitForSeconds(1f);

                // Cleanup destroyed enemies again if needed
                CleanUpDestroyedEnemies();

                // Check if the condition to resume spawning is met
                if (spawnedEnemies.Count <= maxActiveEnemies - resumeSpawnThreshold)
                {
                    break;
                }
            }

            yield return new WaitForSeconds(config.spawnInterval);

            // Spawn an enemy if within limits
            Vector3 spawnPosition = GetValidSpawnPosition();
            GameObject enemy = Instantiate(config.prefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy); // Add the new enemy to the list
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        while (true)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(spawnRadius - spawnMargin, spawnRadius);
            Vector3 randomPoint = playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;

            randomPoint.y = playerTransform.position.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) &&
                !Physics.Linecast(playerTransform.position, hit.position, obstacleLayers))
            {
                return hit.position;
            }

            Debug.Log("Retrying to find a valid spawn position...");
        }
    }

    private void CleanUpDestroyedEnemies()
    {
        // Remove enemies that have been destroyed from the list
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    public void StartSpawning(string enemyName)
    {
        var config = enemySpawnConfigs.Find(e => e.enemyName == enemyName);
        if (config == null)
        {
            Debug.LogError($"Enemy config for {enemyName} not found!");
            return;
        }

        if (!config.canSpawn)
        {
            config.canSpawn = true;
            StartCoroutine(SpawnEnemyCoroutine(config));
            Debug.Log($"Spawning started for {enemyName}");
        }
    }

    public void StopSpawning(string enemyName)
    {
        var config = enemySpawnConfigs.Find(e => e.enemyName == enemyName);
        if (config == null)
        {
            Debug.LogError($"Enemy config for {enemyName} not found!");
            return;
        }

        if (config.canSpawn)
        {
            config.canSpawn = false;
            Debug.Log($"Spawning stopped for {enemyName}");
        }
    }

    public void StopAllSpawning()
    {
        foreach (var config in enemySpawnConfigs)
        {
            config.canSpawn = false;
        }
        Debug.Log("All enemy spawning stopped.");
    }
}