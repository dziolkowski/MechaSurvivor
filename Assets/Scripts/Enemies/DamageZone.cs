using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 20; // Obrazenia od AOE
    private bool hasDamaged = false; // Czy juz zadano obrazenia

    void OnTriggerEnter(Collider other)
    {
        if (hasDamaged) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                hasDamaged = true;
            }
        }
    }
}

