using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float lifetime = 0.5f; // Jak dlugo ma istniec AOE

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
