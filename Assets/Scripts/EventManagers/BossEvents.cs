using UnityEngine;
using System;
using System.Collections.Generic;

public class BossEvents : MonoBehaviour
{
    public enum EventType
    {
        ToggleObject,
        EnableObject,
        DisableObject,
        SpawnAtLocation
    }

    [Serializable]
    public class TimedEvent
    {
        public string eventName;          // For easier identification in the Inspector
        public float timeToTrigger;       // When to trigger the event
        public GameObject targetObject;   // Which object to affect/spawn
        public EventType eventType;       // What kind of action to take
        public Transform spawnLocation;   // Optional spawn location
        [HideInInspector]
        public bool hasTriggered;         // Internal tracking
    }

    [SerializeField] private TimedEvent[] events;
    [SerializeField] private bool autoStart = false; // Wy³¹czamy automatyczne rozpoczêcie
    [SerializeField] private bool loopEvents;

    private float timer;
    private bool isRunning;
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Track spawned objects

    public GameObject bossObject; // Referencja do obiektu Bossa (Behemotha)
    public GameObject congratulationsPanel; // Referencja do panelu "Congratulations!"
    private bool bossDefeated = false; // Flaga informuj¹ca czy boss zosta³ ju¿ pokonany

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        foreach (TimedEvent timedEvent in events)
        {
            if (!timedEvent.hasTriggered && timedEvent.targetObject != null && timer >= timedEvent.timeToTrigger)
            {
                ExecuteEvent(timedEvent);
                timedEvent.hasTriggered = true;
            }
        }

        // Sprawdzanie czy boss zosta³ pokonany
        if (!bossDefeated && bossObject == null)
        {
            BossDefeated();
        }
    }

    private void ExecuteEvent(TimedEvent timedEvent)
    {
        switch (timedEvent.eventType)
        {
            case EventType.ToggleObject:
                timedEvent.targetObject.SetActive(!timedEvent.targetObject.activeSelf);
                break;

            case EventType.EnableObject:
                timedEvent.targetObject.SetActive(true);
                if (timedEvent.spawnLocation != null)
                {
                    timedEvent.targetObject.transform.position = timedEvent.spawnLocation.position;
                }
                break;

            case EventType.DisableObject:
                timedEvent.targetObject.SetActive(false);
                break;

            case EventType.SpawnAtLocation:
                Vector3 spawnPosition = timedEvent.spawnLocation != null
                    ? timedEvent.spawnLocation.position
                    : Vector3.zero;

                // Instantiate the new object and add it to our list
                GameObject newObject = Instantiate(timedEvent.targetObject, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newObject);
                break;
        }
    }

    private void BossDefeated()
    {
        bossDefeated = true;
        Debug.Log("Behemoth zosta³ pokonany!"); // Log do debugowania
        Time.timeScale = 0; // Pauza w grze
        congratulationsPanel.SetActive(true); // W³¹czenie panelu "Congratulations!"
        isRunning = false; // Zatrzymanie dalszych zdarzeñ
    }

    public void StartEvents()
    {
        isRunning = true;
        timer = 0f;
        ClearSpawnedObjects();
        foreach (TimedEvent timedEvent in events)
        {
            timedEvent.hasTriggered = false;
        }
    }

    public void PauseEvents()
    {
        isRunning = false;
    }

    public void ResumeEvents()
    {
        isRunning = true;
    }

    public void ResetEvents()
    {
        timer = 0f;
        ClearSpawnedObjects();
        foreach (TimedEvent timedEvent in events)
        {
            timedEvent.hasTriggered = false;
        }
    }

    private void ClearSpawnedObjects()
    {
        // Destroy all spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }

    private void OnDestroy()
    {
        // Clean up any spawned objects when the script is destroyed
        ClearSpawnedObjects();
    }
}