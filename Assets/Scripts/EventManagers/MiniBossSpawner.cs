using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MiniBossEvents : MonoBehaviour
{
    public enum EventType
    {
        ToggleObject,
        EnableObject,
        DisableObject,
        SpawnMiniBoss
    }

    [Serializable]
    public class TimedEvent
    {
        public string eventName;          // For easier identification in the Inspector
        public GameObject targetObject;    // MiniBoss prefab or object to affect
        public EventType eventType;       // What kind of action to take
        [HideInInspector]
        public bool hasTriggered;         // Internal tracking
        
        public bool IsValid() => targetObject != null;
    }

    [SerializeField] private TimedEvent[] events;
    [SerializeField] private List<float> triggerTimes = new List<float>(4); // 4 time points for events
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool randomEvents;
    [SerializeField] private bool loopEvents;
    
    private float timer;
    private bool isRunning;
    private List<GameObject> spawnedBosses = new List<GameObject>();
    private List<int> availableEventIndices = new List<int>();
    private int currentTriggerIndex;

    private void OnValidate()
    {
        // Keep trigger times in ascending order
        if (triggerTimes.Count > 1)
        {
            for (int i = 1; i < triggerTimes.Count; i++)
            {
                if (triggerTimes[i] < triggerTimes[i - 1])
                {
                    triggerTimes[i] = triggerTimes[i - 1];
                }
            }
        }
    }

    private void Start()
    {
        ValidateSetup();
        if (autoStart)
        {
            StartEvents();
        }
    }

    private void ValidateSetup()
    {
        if (events == null || events.Length == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: No events configured.");
            return;
        }

        if (triggerTimes == null || triggerTimes.Count == 0)
        {
            Debug.LogError($"[{gameObject.name}] MiniBossEvents: No trigger times configured.");
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: No spawn points configured.");
        }

        // Validate all events
        for (int i = 0; i < events.Length; i++)
        {
            if (!events[i].IsValid())
            {
                Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: Event {i} ({events[i].eventName}) has invalid configuration.");
            }
        }
    }

    private void Update()
    {
        if (!isRunning || currentTriggerIndex >= triggerTimes.Count) return;

        timer += Time.deltaTime;

        if (timer >= triggerTimes[currentTriggerIndex])
        {
            TriggerNextEvent();
            currentTriggerIndex++;

            bool allEventsTriggered = currentTriggerIndex >= triggerTimes.Count;
            
            if (allEventsTriggered && loopEvents)
            {
                ResetEvents();
            }
            else if (allEventsTriggered)
            {
                OnEventsComplete();
            }
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
            return null;

        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
    }

    private void TriggerNextEvent()
    {
        if (events == null || events.Length == 0) return;

        // Initialize available indices if needed
        if (availableEventIndices.Count == 0)
        {
            availableEventIndices = Enumerable.Range(0, events.Length).ToList();
        }

        if (availableEventIndices.Count == 0) return;

        int eventIndex;
        if (randomEvents)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableEventIndices.Count);
            eventIndex = availableEventIndices[randomIndex];
            availableEventIndices.RemoveAt(randomIndex);
        }
        else
        {
            eventIndex = availableEventIndices[0];
            availableEventIndices.RemoveAt(0);
        }

        if (eventIndex < events.Length && events[eventIndex].IsValid())
        {
            ExecuteEvent(events[eventIndex]);
            events[eventIndex].hasTriggered = true;
        }
    }

    private void ExecuteEvent(TimedEvent timedEvent)
    {
        if (!timedEvent.IsValid())
        {
            Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: Attempted to execute invalid event '{timedEvent.eventName}'");
            return;
        }

        try
        {
            switch (timedEvent.eventType)
            {
                case EventType.ToggleObject:
                    timedEvent.targetObject.SetActive(!timedEvent.targetObject.activeSelf);
                    break;

                case EventType.EnableObject:
                    timedEvent.targetObject.SetActive(true);
                    break;

                case EventType.DisableObject:
                    timedEvent.targetObject.SetActive(false);
                    break;

                case EventType.SpawnMiniBoss:
                    Transform spawnPoint = GetRandomSpawnPoint();
                    if (spawnPoint != null)
                    {
                        GameObject newBoss = Instantiate(timedEvent.targetObject, spawnPoint.position, spawnPoint.rotation);
                        spawnedBosses.Add(newBoss);
                        Debug.Log($"[{gameObject.name}] MiniBossEvents: Spawned {timedEvent.eventName} at {spawnPoint.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: No valid spawn point for {timedEvent.eventName}");
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[{gameObject.name}] MiniBossEvents: Error executing event '{timedEvent.eventName}': {e.Message}");
        }
    }

    private void ClearSpawnedBosses()
    {
        foreach (GameObject boss in spawnedBosses)
        {
            if (boss != null)
            {
                Destroy(boss);
            }
        }
        spawnedBosses.Clear();
    }

    private void OnEventsComplete()
    {
        isRunning = false;
        Debug.Log($"[{gameObject.name}] MiniBossEvents: All events completed.");
    }

    public void StartEvents()
    {
        ResetState();
        isRunning = true;
    }

    public void PauseEvents()
    {
        isRunning = false;
    }

    public void ResumeEvents()
    {
        if (currentTriggerIndex >= triggerTimes.Count)
        {
            Debug.LogWarning($"[{gameObject.name}] MiniBossEvents: Cannot resume - all events completed.");
            return;
        }
        isRunning = true;
    }

    public void ResetEvents()
    {
        ResetState();
    }

    private void ResetState()
    {
        timer = 0f;
        currentTriggerIndex = 0;
        availableEventIndices.Clear();
        ClearSpawnedBosses();
        
        if (events != null)
        {
            foreach (TimedEvent timedEvent in events)
            {
                timedEvent.hasTriggered = false;
            }
        }
    }

    private void OnDestroy()
    {
        ClearSpawnedBosses();
    }
}