using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeaponEvents : MonoBehaviour
{
    public enum EventType
    {
        ToggleObject,
        EnableObject,
        DisableObject
    }

    [Serializable]
    public class TimedEvent
    {
        public string eventName;
        public GameObject targetObject;
        public EventType eventType;
        [HideInInspector]
        public bool hasTriggered;
        
        public bool IsValid() => targetObject != null;
    }

    [SerializeField] private TimedEvent[] events;
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool randomEvents;
    [SerializeField] private List<float> triggerTimes = new List<float>(4);

    private float timer;
    private bool isRunning;
    private List<int> availableEventIndices = new List<int>();
    private int currentTriggerIndex;

    private void OnValidate()
    {
        // Ensure trigger times are in ascending order
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
            Debug.LogWarning($"[{gameObject.name}] WeaponEvents: No events configured.");
            return;
        }

        if (triggerTimes == null || triggerTimes.Count == 0)
        {
            Debug.LogError($"[{gameObject.name}] WeaponEvents: No trigger times configured.");
            enabled = false;
            return;
        }

        // Validate all events
        for (int i = 0; i < events.Length; i++)
        {
            if (!events[i].IsValid())
            {
                Debug.LogWarning($"[{gameObject.name}] WeaponEvents: Event {i} ({events[i].eventName}) has invalid configuration.");
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
            
            // Check if we've completed all events
            if (currentTriggerIndex >= triggerTimes.Count)
            {
                OnEventsComplete();
            }
        }
    }

    private void OnEventsComplete()
    {
        isRunning = false;
        Debug.Log($"[{gameObject.name}] WeaponEvents: All events completed.");
    }

    private void StartEvents()
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
            Debug.LogWarning($"[{gameObject.name}] WeaponEvents: Cannot resume - all events completed.");
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
        
        if (events != null)
        {
            foreach (TimedEvent timedEvent in events)
            {
                timedEvent.hasTriggered = false;
            }
        }
    }

    private void TriggerNextEvent()
    {
        if (events == null || events.Length == 0) return;

        // If this is the first event, initialize available indices
        if (availableEventIndices.Count == 0)
        {
            availableEventIndices = Enumerable.Range(0, events.Length).ToList();
        }

        if (availableEventIndices.Count == 0) return; // All events have been triggered

        int eventIndex;
        if (randomEvents)
        {
            // Randomly select from remaining events
            int randomIndex = UnityEngine.Random.Range(0, availableEventIndices.Count);
            eventIndex = availableEventIndices[randomIndex];
            availableEventIndices.RemoveAt(randomIndex);
        }
        else
        {
            // Take the next available event
            eventIndex = availableEventIndices[0];
            availableEventIndices.RemoveAt(0);
        }

        if (eventIndex < events.Length && events[eventIndex].IsValid())
        {
            ExecuteEvent(events[eventIndex]);
            events[eventIndex].hasTriggered = true;
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] WeaponEvents: Skipping invalid event at index {eventIndex}");
        }
    }

    private void ExecuteEvent(TimedEvent timedEvent)
    {
        if (!timedEvent.IsValid())
        {
            Debug.LogWarning($"[{gameObject.name}] WeaponEvents: Attempted to execute invalid event '{timedEvent.eventName}'");
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
            }
            Debug.Log($"[{gameObject.name}] WeaponEvents: Executed event '{timedEvent.eventName}'");
        }
        catch (Exception e)
        {
            Debug.LogError($"[{gameObject.name}] WeaponEvents: Error executing event '{timedEvent.eventName}': {e.Message}");
        }
    }
}