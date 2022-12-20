using System;
using System.Collections;
using System.Collections.Generic;
using escapefromtampere.PlayerControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    // tätä voi käyttää esim äänissä kun painaa esciä niin musiikki vaikka lakkaa

    //if (PauseMenu.GameIsPaused{audioSource.stop}
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Actions.ui.PauseMenuToggle.WasPerformedThisFrame())
        {
            SetPaused(!GameIsPaused);
        }
    }

    public void SetPaused(bool paused)
    {
        GameIsPaused = paused;
        pauseMenuUI.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
        PlayerController.SetCursorLock(!paused);
    }

    // Called by the menu button
    public void LoadMenu()
    {
        
        Debug.Log("Opening menu...");
        Time.timeScale = 1f;
        // avaa menu scenen
        SceneManager.LoadScene("Menu");
    }

    // Called by the quit button
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
