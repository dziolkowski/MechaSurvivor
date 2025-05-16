using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedEventManager : MonoBehaviour
{
    [System.Serializable]
    public class ScheduledEvent
    {
        public enum EventType { IncreaseEnemyHP, ToggleCanSpawn } // Action type
        public EventType eventType;                              // The type of event
        public string targetName;                                // Target enemy name (for both events)
        public int newHealthValue;                               // New health value (only for IncreaseEnemyHP)
        public bool newCanSpawnValue = true;                     // New canSpawn value (only for ToggleCanSpawn)
        public float triggerTime;                                // When to trigger (in seconds since game started)
        public bool hasOccurred = false;                         // Indicates if the event has occurred
    }

    [SerializeField] private List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>(); // Event list
    [SerializeField] private EnemyManager enemyManager;           // Reference to EnemyManager
    [SerializeField] private SpawnEnemiesOnNavMesh spawner;       // Reference to OutsideCameraEnemySpawner

    private float gameStartTime;

    void Start()
    {
        gameStartTime = Time.time; // Track game start time
        StartCoroutine(ProcessEvents());
    }

    private IEnumerator ProcessEvents()
    {
        while (scheduledEvents.Count > 0)
        {
            var nextEvent = scheduledEvents[0];

            // Calculate the wait time based on the next event's trigger time
            float waitTime = nextEvent.triggerTime - (Time.time - gameStartTime);

            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            ExecuteEvent(nextEvent); // Execute the next event
            nextEvent.hasOccurred = true; // Mark the event as occurred

            // Remove the occurred event from the list
            scheduledEvents.RemoveAt(0);
        }

        if (scheduledEvents.Count == 0)
        {
            Debug.Log("All events completed");
        }
        else
        {
            Debug.LogWarning("Some events are missing or ended incorrect!");
        }
    }

    private void ExecuteEvent(ScheduledEvent scheduledEvent)
    {
        switch (scheduledEvent.eventType)
        {
            case ScheduledEvent.EventType.IncreaseEnemyHP:
                if (enemyManager != null)
                {
                    enemyManager.SetEnemyHealth(scheduledEvent.targetName, scheduledEvent.newHealthValue);
                }
                else
                {
                    Debug.LogError("EnemyManager reference is missing!");
                }
                break;

            case ScheduledEvent.EventType.ToggleCanSpawn:
                if (spawner != null)
                {
                    if (scheduledEvent.newCanSpawnValue)
                    {
                        spawner.StartSpawning(scheduledEvent.targetName); // Start spawning if newCanSpawnValue is true
                    }
                    else
                    {
                        spawner.StopSpawning(scheduledEvent.targetName); // Stop spawning if newCanSpawnValue is false
                    }
                }
                else
                {
                    Debug.LogError("Spawner reference is missing!");
                }
                break;

            default:
                Debug.LogWarning("Unknown event type!");
                break;
        }
    }

    /// <summary>
    /// Removes events that have already occurred.
    /// </summary>
    public void RemoveOccurredEvents()
    {
        scheduledEvents.RemoveAll(e => e.hasOccurred);
        Debug.Log("Removed all occurred events.");
    }
}