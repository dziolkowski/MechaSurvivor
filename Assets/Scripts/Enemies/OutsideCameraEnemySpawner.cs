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

    [SerializeField] private List<EnemySpawnConfig> enemySpawnConfigs = new List<EnemySpawnConfig>(); // Enemy configurations
    [SerializeField] private float spawnRadius = 50f;         // Maximum spawn distance
    [SerializeField] private float spawnMargin = 5f;          // Minimum spawn margin
    [SerializeField] private Transform playerTransform;       // Player position reference
    [SerializeField] private LayerMask obstacleLayers;        // Layers to avoid while spawning

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
            yield return new WaitForSeconds(config.spawnInterval);

            // Continuously try to find a valid spawn position
            Vector3 spawnPosition = GetValidSpawnPosition();
            Instantiate(config.prefab, spawnPosition, Quaternion.identity);
            //Debug.Log($"Spawned {config.enemyName} at {spawnPosition}");
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        while (true) // Keep trying until a valid position is found
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random 2D direction
            float randomDistance = Random.Range(spawnRadius - spawnMargin, spawnRadius);
            Vector3 randomPoint = playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;

            randomPoint.y = playerTransform.position.y; // Ensure the same height as the player

            // Validate the position using NavMesh and obstacles
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) &&
                !Physics.Linecast(playerTransform.position, hit.position, obstacleLayers))
            {
                return hit.position; // Valid position found, return it
            }

            Debug.Log("Retrying to find a valid spawn position...");
        }
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