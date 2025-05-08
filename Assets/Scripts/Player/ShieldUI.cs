using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    public Slider shieldSlider; 
    public GameObject shieldUIContainer; 

    private PlayerShield playerShield;

    void Start()
    {
        playerShield = Object.FindFirstObjectByType<PlayerShield>();
        shieldUIContainer.SetActive(false); // Ukryj na start
    }

    void Update()
    {
        if (playerShield != null)
        {
            if (playerShield.HasShield())
            {
                shieldUIContainer.SetActive(true);
                shieldSlider.maxValue = playerShield.MaxShieldPoints;
                shieldSlider.value = playerShield.CurrentShieldPoints;
            }
            else
            {
                shieldUIContainer.SetActive(false);
            }
        }
    }
}
