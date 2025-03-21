using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private float slowMultiplier = 0.5f;
    private PlayerController playerController;

    private void Start()
    {
        Invoke(nameof(DestroySelf), lifetime); // Plama znika po okre�lonym czasie
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ModifySpeed(slowMultiplier);
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
        if (playerController != null) // Je�li gracz nadal jest w plamie, resetujemy jego pr�dko��
        {
            playerController.ResetSpeed();
        }
        Destroy(gameObject);
    }
}
