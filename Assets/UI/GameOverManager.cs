using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Referencja do panelu Game Over

    public void ShowGameOver() 
    {
        Time.timeScale = 0; // Pauza w grze
        gameOverPanel.SetActive(true); // Wlacza panel Game Over
    }

    public void RestartGame() 
    {
        Time.timeScale = 1; // Wznowienie gry
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GoToMainMenu() 
    {
        Time.timeScale = 1; // Wnozwienie gry
        SceneManager.LoadScene("MainMenu"); // Zaladuj menu glowne
    }
}
