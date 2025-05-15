using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public string enemyName; // The name of the enemy type
    public int health;       // The default starting HP of the enemy
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<EnemyType> enemyTypes = new List<EnemyType>(); // List storing all enemy types and their HP

    /// <summary>
    /// Gets the starting HP of a specified enemy type.
    /// </summary>
    /// <param name="enemyName">The name of the enemy type.</param>
    /// <returns>The starting HP if found, or 0 if not found.</returns>
    public int GetEnemyHealth(string enemyName)
    {
        foreach (var type in enemyTypes)
        {
            if (type.enemyName == enemyName)
            {
                return type.health;
            }
        }

        Debug.LogWarning($"Enemy type \"{enemyName}\" not found in EnemyManager!");
        return 0; // Default value if the enemy type is not found
    }

    /// <summary>
    /// Sets the HP for a specific enemy type.
    /// </summary>
    /// <param name="enemyName">The name of the enemy type.</param>
    /// <param name="newHealth">The new HP value to set.</param>
    public void SetEnemyHealth(string enemyName, int newHealth)
    {
        foreach (var type in enemyTypes)
        {
            if (type.enemyName == enemyName)
            {
                type.health = newHealth;
                Debug.Log($"Updated {enemyName}'s health to {newHealth}!");
                return;
            }
        }

        Debug.LogWarning($"Enemy type \"{enemyName}\" not found in EnemyManager!");
    }

    /// <summary>
    /// Provides the full list of enemy types and their associated HP values.
    /// </summary>
    /// <returns>A list of all enemy types.</returns>
    public List<EnemyType> GetAllEnemyTypes()
    {
        return enemyTypes;
    }
}