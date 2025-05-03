using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {
    // Called when we click the "Play" button. 
    public void LoadLevel(int level) {
        SceneManager.LoadScene(level);
    }
    
    public void OnSettingsButton() {
        Debug.Log("Open settings menu TBD");
    }

    // Called when we click the "Quit" button.
    public void OnQuitButton() {
        Application.Quit();
    }
}
