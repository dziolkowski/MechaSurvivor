using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Przypisanie obiektu gracza z poziomu Unity
    public Transform player; 
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] bool isMoving = true;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (isMoving == false) // do testowania/debugowania
        {
            return;
        }
        MoveToPlayer();
    }

    private void MoveToPlayer() 
    {
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");
        var moveToPlayer = findPlayer.transform.position;
        UnityEngine.AI.NavMeshAgent enemyNavMoveTowardPlayer = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyNavMoveTowardPlayer.destination = moveToPlayer;
        if(findPlayer = null) 
        {
            Debug.LogError("Player not found");
        }
    }
}
