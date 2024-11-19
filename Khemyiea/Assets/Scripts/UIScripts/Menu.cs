using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] string _nextLevelName;
    public GameObject optMenuUI;

    public void StartGame()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    public void Options()
    {
        optMenuUI.SetActive(true);
    }

    public void Back()
    {
        optMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Credits()
    {
        Application.OpenURL("morgankenner-portfolio.carrd.co");
    }
}