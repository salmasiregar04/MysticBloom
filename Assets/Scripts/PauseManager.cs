using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenOptions()
    {
        NavigationManager.OpenedFromPause = true;

        pausePanel.SetActive(false);

        SceneManager.LoadScene(
            "Options",
            LoadSceneMode.Additive
        );
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}