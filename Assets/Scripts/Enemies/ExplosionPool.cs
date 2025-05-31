using System.Collections;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    private int damage;
    private float radius;

    public void Prepare(int damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
    }

    // Uruchom eksplozje manualnie
    public void Explode()
    {
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        yield return new WaitForSeconds(0.1f); // Ewentualna animacja przed eksplozja

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    ph.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject); // Usun obiekt po eksplozji
    }
}
