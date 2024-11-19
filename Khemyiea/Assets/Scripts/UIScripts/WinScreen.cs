using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] string _nextLevelName;
    [SerializeField] string _restartLevelName;
    public GameObject winScreen;

    public void Restart()
    {
        SceneManager.LoadScene(_restartLevelName);
        Time.timeScale = 1.0f;
        winScreen.SetActive(false);
    }

    public void Credits()
    {
        Application.OpenURL("morgankenner-portfolio.carrd.co");
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
}
