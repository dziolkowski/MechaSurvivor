using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Obiekt gracza
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private float retreatDistance = 2f; // Odleglosc odskoku
    [SerializeField] private float retreatDuration = 0.5f; // Czas trwania odskoku
    [SerializeField] private int damageDealt = 2;
    private bool isRetreating = false; // Sprawdzanie czy przeciwniki odskakuje

    [Header("Debug - do NOT ship set to FALSE")]
    [SerializeField] private bool isMoving = true;

    void Start()
    {
        if (Time.timeScale == 0) return; // Pauza - przerwanie ruchu
        if(isMoving == false) {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
        // Pobieranie aktualnego gracza za kazdym razem (jesli gracz zniknal i pojawil sie ponownie)
        GameObject findPlayer = GameObject.FindGameObjectWithTag("Player");

        if (findPlayer == null)
        {
            //Debug.LogWarning("Player not found! Enemy cannot move.");
            return;
        }

        player = findPlayer.transform; // Ustawienie aktualnego obiektu gracza
        agent.SetDestination(player.position); // Ustaw cel NavMeshAgent na aktualna pozycje gracza
    }

    public void DealDamage(PlayerHealth playerHealth)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageDealt); // Zadawanie obrazen graczowi
            StartCoroutine(RetreatFromPlayer()); // Odskok
        }
    }

    private IEnumerator RetreatFromPlayer()
    {
        isRetreating = true;

        // Oblicz kierunek odskoku
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * retreatDistance;

        // Ustaw cel NavMeshAgent
        agent.SetDestination(retreatPosition);
        //Debug.Log("Enemy is retreating to position: " + retreatPosition);

        yield return new WaitForSeconds(retreatDuration); // Poczekaj na zakonczenie odskoku

        isRetreating = false;
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Player"))// Sprawdzanie, czy przeciwnik wszedl w kolizje z graczem
        {
            Debug.Log("Enemy collided with player!");
            if (GetComponent<EnemyHealth>().currentHealth <= 0) { // jesli przeciwnik ma 0 hp, nie zadaje obrazen przy stycznosci /L
                return;
            }
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();// Pobieranie komponentu PlayerHealth z obiektu gracza
            if (playerHealth != null)
            {
                DealDamage(playerHealth); // Zadawanie obrazen graczowi
            }
        }
    }
}
