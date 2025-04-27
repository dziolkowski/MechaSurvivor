using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    public TextMeshProUGUI slotText; // Napisa na przycisku
    private SlotSelectionUI selectionUI; // Odwolanie do SlotSelectionUI
    private int slotIndex; // Numer slotu


    public void Setup(SlotSelectionUI ui, int index) // Ustawienia przycisku 
    {
        selectionUI = ui;
        slotIndex = index;
        slotText.text = "Slot " + (index +1);   
    }

    public void OnButtonClick() // Funkcja wywolywana po kliknieciu
    { 
        selectionUI.OnSlotSelected(slotIndex);
    }
}
