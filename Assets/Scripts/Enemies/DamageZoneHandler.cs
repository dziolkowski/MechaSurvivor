using System.Collections;
using UnityEngine;

public class DamageZoneHandler : MonoBehaviour
{
    private float lifetime; // Czas zycia plamy
    private float moveSpeedMultiplier; // Mnoznik predkosci ruchu
    private float rotationSpeedMultiplier; // Mnoznik predkosci obrotu

    private bool playerInside = false; // Flaga informujaca, czy gracz znajduje sie w plamie
    private PlayerController playerController; // Referencja do gracza

    // Inicjalizacja danych plamy
    public void Initialize(float life, float moveMult, float rotMult)
    {
        lifetime = life;
        moveSpeedMultiplier = moveMult;
        rotationSpeedMultiplier = rotMult;

        // Uruchamiamy niszczenie plamy po czasie
        Invoke(nameof(DestroySelf), lifetime);
    }

    // Gdy gracz wchodzi do plamy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerInside = true; // Gracz znajduje sie w plamie
                playerController.ModifySpeed(moveSpeedMultiplier, rotationSpeedMultiplier); // Zmniejszamy predkosc gracza
            }
        }
    }

    // Gdy gracz wychodzi z plamy
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerController != null)
        {
            playerController.RemoveSlow(); // Cofamy spowolnienie
            playerInside = false; // Gracz wyszedl z plamy
            playerController = null;
        }
    }

    // Niszczenie plamy po czasie
    private void DestroySelf()
    {
        // Jesli gracz nadal znajduje sie w plamie, trzeba recznie cofnac spowolnienie
        if (playerInside && playerController != null)
        {
            playerController.RemoveSlow();
        }

        Destroy(gameObject); // Usuwamy plame z gry
    }
}

