using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] string _nextLevelName;

    public static bool GameIsPaused = false;
    public GameObject hud;
    public GameObject pauseMenuUI;
    public GameObject optMenuUI;

    void Update()
    {
        Debug.Log("Hello");
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
        hud.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Back()
    {
        optMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    void Pause()
    {
        hud.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Options()
    {
        optMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
}