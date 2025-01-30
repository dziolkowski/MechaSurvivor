using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInGameMenu : MonoBehaviour {
    public GameObject menuPopup;
    public GameObject inGameUI;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // Sprawdza, czy nacisnieto ESC
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused) {
                PauseGame();  // Pauzuje gre, gdy menu jest otwarte
            }
            else {
                ResumeGame(); // Wznawia gre, gdy menu nie jest otwarte
            }
        }
    }

    void PauseGame() {
        isPaused = true;
        Time.timeScale = 0f; // Pauzuje gre (zatrzymuje czas)
        inGameUI.SetActive(false); // Ukrywa UI gry
        menuPopup.SetActive(true);  // Pokazuje menu pauzy
    }

    // funkcja jest public aby przycisk z UI mial do niej dostep
    public void ResumeGame() {
        isPaused = false;
        Time.timeScale = 1f; // Wznawia gre
        inGameUI.SetActive(true); // Pokazuje UI gry
        menuPopup.SetActive(false);  // Ukrywa menu pauzy
    }

}
