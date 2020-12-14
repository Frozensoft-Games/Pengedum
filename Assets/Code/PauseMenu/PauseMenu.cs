using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public static bool optionsOpen;

    public GameObject pauseMenuUi;

    public GameObject optionsMenu;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (gameIsPaused && !optionsOpen)
            Resume();
        else if(gameIsPaused && optionsOpen)
            OptionsBack();
        else 
            Pause();
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log(DoorManager.isOpen + " " + OpenPCManager.isOpen);
        if(!DoorManager.isOpen && !OpenPCManager.isOpen)
            Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        GameManager.instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        if(GameSaveManager.instance != null)
            GameSaveManager.instance.Save();

        Application.Quit();
    }

    // Options for the game
    public void Options()
    {
        pauseMenuUi.SetActive(false);
        optionsMenu.SetActive(true);
        optionsOpen = true;
    }

    // Options Back
    public void OptionsBack()
    {
        pauseMenuUi.SetActive(true);
        optionsMenu.SetActive(false);
        optionsOpen = false;
    }
}
