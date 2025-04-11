using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    [SerializeField] bool HasDeathAnimation; // tymczasowe rozwiazanie dla przeciwnikow bez animacji smierci aby poprawnie umierali

    public int scoreValue = 10;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        print(gameObject + " damage received!");
        if (currentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false; // wylaczenie collidera aby nie zadawac graczowi obrazen /L
            gameObject.GetComponent<NavMeshAgent>().isStopped = true; // zatrzymanie przeciwnika w momencie kiedy ma 0 HP /L
            // WORKAROUND - USUNAC POZNIEJ /L
            if (HasDeathAnimation) { // jesli ma anmacje smierci, to Die() zostanie wywolana po animacji
                animator.SetTrigger("Death");
                print("kaboom");
            }
            else Die(); // jesli nie ma animacji smierci to wywoluje Die()
        }
    }

    void Die()
    {
        //print("dead");
        ScoreManager.Instance.AddPoints(scoreValue); // Dodaje punkty po smierci przeciwnika
        Destroy(gameObject); // Niszczenie przeciwnika po smierci
    }
}
