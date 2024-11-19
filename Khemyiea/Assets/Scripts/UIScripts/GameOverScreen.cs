using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] string _nextLevelName;
    [SerializeField] string _restartLevelName;
    public GameObject gameOverScreen;

    public void Restart()
    {
        SceneManager.LoadScene(_restartLevelName);
        gameOverScreen.SetActive(false);
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
