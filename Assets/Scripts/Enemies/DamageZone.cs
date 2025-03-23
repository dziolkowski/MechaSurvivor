using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f; // Czas po, ktorym znika plama
    [SerializeField] private float moveSpeedMultiplier = 0.5f; // Zmniejszenie predkosci poruszania
    [SerializeField] private float rotationSpeedMultiplier = 0.5f; // Zmniejszenie predkosci obrotu
    private PlayerController playerController;

    private void Start()
    {
        Invoke(nameof(DestroySelf), lifetime); // Plama znika po okreslonym czasie
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ModifySpeed(moveSpeedMultiplier, rotationSpeedMultiplier);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerController != null)
        {
            playerController.ResetSpeed();
            playerController = null;
        }
    }

    private void DestroySelf()
    {
        if (playerController != null) // Jesli gracz nadal jest w plamie, resetujemy jego predkosc
        {
            playerController.ResetSpeed();
        }
        Destroy(gameObject);
    }
}

