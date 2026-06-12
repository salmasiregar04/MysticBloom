using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenOptions()
    {
        NavigationManager.OpenedFromPause = false;

        SceneManager.LoadScene("Options");
    }

    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}