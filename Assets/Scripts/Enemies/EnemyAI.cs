using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float retreatDistance = 2f;
    [SerializeField] private float retreatDuration = 0.5f;
    [SerializeField] public int damage = 2; 
    private bool isRetreating = false;

    [Header("Debug - do NOT ship set to FALSE")]
    [SerializeField] private bool isMoving = true;

    void Start()
    {
        if (Time.timeScale == 0) return;
        if (!isMoving)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
    }

    void Update()
    {
        if (!isRetreating)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");

        if (findPlayer == null) return;

        player = findPlayer.transform;
        agent.SetDestination(player.position);
    }

    public void DealDamage(PlayerHealth playerHealth)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            StartCoroutine(RetreatFromPlayer());
        }
    }

    private IEnumerator RetreatFromPlayer()
    {
        isRetreating = true;

        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * retreatDistance;

        agent.SetDestination(retreatPosition);

        yield return new WaitForSeconds(retreatDuration);

        isRetreating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with player!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                DealDamage(playerHealth);
            }
        }
    }

    // Zwieksz obrazenia przeciwnika
    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }
}
