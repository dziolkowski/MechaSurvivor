using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class WeaponDropManager : MonoBehaviour
{
    public List<WeaponData> remainingWeapons = new List<WeaponData>();
    public List<WeaponSlot> sideSlots;
    public List<Image> sideSlotIcons; //Lista ikon, ktore zmieniaja siê na interfejsie
    public float dropInterval = 30f;

    public GameObject dropPanel; // Panel UI z przyciskami wyboru

    // Gotowe przyciski UI (3 sztuki)
    public List<Button> weaponButtons;
    public List<Image> weaponIcons; // Ikony broni na przyciskach
    public List<TMP_Text> weaponNames; // Nazwy broni (jesli uzywasz TextMeshPro)

    private WeaponData selectedWeapon; // Aktualnie wybrana bron

    public GameObject slotSelectionPanel; // Panel do wyboru slotu
    public List<Button> slotButtons; // Przyciski do wyboru slotow



    public void Initialize(List<WeaponData> weapons)
    {
        remainingWeapons = weapons;
        StartCoroutine(DropLoop());
    }

    IEnumerator DropLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(dropInterval);

            if (HasFreeSlot())
            {
                ShowDropOptions();
            }
            else
            {
                // Wszystkie sloty sa zajete 
                Debug.Log("Wszystkie sloty s¹ ju¿ zajête. Dropy zatrzymane.");
                yield break; // Zatrzymujemy petle calkowicie
            }
        }
    }

    void ShowDropOptions()
    {
        Time.timeScale = 0f; // Pauza

        dropPanel.SetActive(true);

        // Losujemy bronie
        List<WeaponData> pool = new List<WeaponData>(remainingWeapons);
        int count = Mathf.Min(3, pool.Count);

        for (int i = 0; i < weaponButtons.Count; i++)
        {
            if (i < count)
            {
                int index = Random.Range(0, pool.Count);
                WeaponData weapon = pool[index];
                pool.RemoveAt(index);

                // Ustaw ikone i nazwe
                if (weaponIcons != null && weaponIcons.Count > i)
                    weaponIcons[i].sprite = weapon.icon;

                if (weaponNames != null && weaponNames.Count > i)
                    weaponNames[i].text = weapon.weaponName;

                // Czyscimy stare listenery
                weaponButtons[i].onClick.RemoveAllListeners();

                // Dodajemy nowego listenera
                weaponButtons[i].onClick.AddListener(() => SelectWeaponForSlot(weapon));

                weaponButtons[i].gameObject.SetActive(true);
            }
            else
            {
                // Ukrywamy przyciski jesli nie mamy broni
                weaponButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void SelectWeaponForSlot(WeaponData weapon)
    {
        selectedWeapon = weapon;
        dropPanel.SetActive(false);

        // Pokazujemy panel do wyboru slotu
        slotSelectionPanel.SetActive(true);

        // Podpinamy listener do kazdego przycisku slotu
        for (int i = 0; i < slotButtons.Count; i++)
        {
            int index = i; // lokalna kopia indeksu
            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => AssignWeaponToSpecificSlot(index));
            slotButtons[i].interactable = true;
        }
    }

    void AssignWeaponToSpecificSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < sideSlots.Count)
        {
            slotButtons[slotIndex].interactable = false; // blokujemy klikniêty przycisk

            if (sideSlots[slotIndex].transform.childCount == 0)
            {
                sideSlots[slotIndex].AssignWeapon(selectedWeapon);

                var baseWeapon = sideSlots[slotIndex].GetComponentInChildren<BaseWeapon>();
                if (baseWeapon != null)
                {
                    WeaponManager.Instance.RegisterWeapon(baseWeapon);
                }

                // Zmieniamy tekst na przycisku na nazwe broni
                TMP_Text buttonText = slotButtons[slotIndex].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = selectedWeapon.weaponName;
                }

                // Aktualizujemy ikone na interfejsie
                if (sideSlotIcons != null && sideSlotIcons.Count > slotIndex)
                {
                    sideSlotIcons[slotIndex].sprite = selectedWeapon.icon;
                    sideSlotIcons[slotIndex].enabled = true; // Upewniamy sie, ze ikona jest widoczna
                }

                slotSelectionPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Debug.Log("Slot zajêty – wybierz inny.");
            }
        }
    }


    bool HasFreeSlot()
    {
        foreach (var slot in sideSlots)
        {
            if (slot.transform.childCount == 0)
            {
                return true; // Wolny slot
            }
        }
        return false; // Wszystkie zajete
    }
}
