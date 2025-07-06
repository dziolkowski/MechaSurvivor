using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordBuffZone : MonoBehaviour
{
    public float buffRadius = 10f; // Promien strefy buffa
    public float buffCooldown = 10f; // Czas odnowienia buffa
    public int healthIncrease = 20; // Ilosc zdrowia dodawana przez buff
    public int damageIncrease = 5; // Ilosc obrazen dodawana przez buff
    public GameObject buffEffectPrefab; // Prefab efektu wizualnego

    private Dictionary<GameObject, float> buffedEnemies = new Dictionary<GameObject, float>();

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRadius);
        HashSet<GameObject> currentEnemies = new HashSet<GameObject>();

        foreach (Collider collider in colliders)
        {
            GameObject enemy = collider.gameObject;

            if (!enemy.CompareTag("Enemy")) continue;
            if (enemy == this.gameObject) continue; // Nie buffuj samego siebie 

            currentEnemies.Add(enemy);

            // Pobierz lub dodaj komponent
            EnemyBuffReceiver receiver = enemy.GetComponent<EnemyBuffReceiver>();
            if (receiver == null)
            {
                receiver = enemy.AddComponent<EnemyBuffReceiver>();
            }

            // Sprawdzanie czy to czas na kolejny buff
            bool shouldBuff = !buffedEnemies.ContainsKey(enemy) || Time.time - buffedEnemies[enemy] >= buffCooldown;

            if (shouldBuff)
            {
                // Zwieksz zdrowie
                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.maxHealth += healthIncrease;
                    health.currentHealth += healthIncrease;
                }
                else
                {
                    SplatterHealth splatter = enemy.GetComponent<SplatterHealth>();
                    if (splatter != null)
                    {
                        splatter.maxHealth += healthIncrease;
                        splatter.currentHealth += healthIncrease;
                    }
                }

                // Zwieksz obrazenia
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.damage += damageIncrease;
                }

                KamikazeAI kamikazeAI = enemy.GetComponent<KamikazeAI>();
                if (kamikazeAI != null)
                {
                    kamikazeAI.damage += damageIncrease;
                }

                // Efekt wizualny
                if (buffEffectPrefab != null && receiver.buffEffect == null)
                {
                    GameObject effect = Instantiate(buffEffectPrefab, enemy.transform.position + Vector3.up * 2f, Quaternion.identity);
                    effect.transform.SetParent(enemy.transform);
                    receiver.buffEffect = effect;
                }

                // Zapisz czas ostatniego buffa
                buffedEnemies[enemy] = Time.time;
            }
        }

        // Usuwanie efektu wizualnego z przeciwnikow, ktorzy wyszli ze strefy
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var kvp in buffedEnemies)
        {
            GameObject enemy = kvp.Key;
            if (!currentEnemies.Contains(enemy))
            {
                EnemyBuffReceiver receiver = enemy.GetComponent<EnemyBuffReceiver>();
                if (receiver != null && receiver.buffEffect != null)
                {
                    Destroy(receiver.buffEffect);
                    receiver.buffEffect = null;
                }

                toRemove.Add(enemy);
            }
        }

        foreach (GameObject enemy in toRemove)
        {
            buffedEnemies.Remove(enemy);
        }
    }

    private void OnDestroy()
    {
        foreach (var kvp in buffedEnemies)
        {
            GameObject enemy = kvp.Key;
            EnemyBuffReceiver receiver = enemy.GetComponent<EnemyBuffReceiver>();
            if (receiver != null && receiver.buffEffect != null)
            {
                Destroy(receiver.buffEffect);
                receiver.buffEffect = null;
            }
        }

        buffedEnemies.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, buffRadius);
    }
}
