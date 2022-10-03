using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    // t‰t‰ voi k‰ytt‰‰ esim ‰‰niss‰ kun painaa esci‰ niin musiikki vaikka lakkaa
    // if (PauseMenu.GameIsPaused{}
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;


    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        
        Debug.Log("Opening menu...");
        Time.timeScale = 1f;
        // avaa menu scenen
        SceneManager.LoadScene("Menu");

        // testi
        Resume();
        
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
