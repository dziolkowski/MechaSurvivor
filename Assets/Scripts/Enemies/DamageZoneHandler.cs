using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneHandler : MonoBehaviour
{
    private float lifetime;
    private float moveSpeedMultiplier;
    private float rotationSpeedMultiplier;
    private PlayerController playerController;

    public void Initialize(float life, float moveMult, float rotMult)
    {
        lifetime = life;
        moveSpeedMultiplier = moveMult;
        rotationSpeedMultiplier = rotMult;
        Invoke(nameof(DestroySelf), lifetime);
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
        if (playerController != null)
        {
            playerController.ResetSpeed();
        }
        Destroy(gameObject);
    }
}

