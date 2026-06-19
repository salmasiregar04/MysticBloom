using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;

    private void Start()
    {
        float musicVolume =
            SaveManager.Instance
            .CurrentData
            .musicVolume;

        float sfxVolume =
            SaveManager.Instance
            .CurrentData
            .sfxVolume;

        bool fullscreen =
            SaveManager.Instance
            .CurrentData
            .fullscreen;

    Screen.fullScreen = fullscreen;
    fullscreenToggle.isOn = fullscreen;

    if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(musicVolume);
            AudioManager.Instance.SetSFXVolume(sfxVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }

        SaveManager.Instance
            .CurrentData
            .musicVolume = value;

        SaveManager.Instance.SaveGame();
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }

        SaveManager.Instance
            .CurrentData
            .sfxVolume = value;

        SaveManager.Instance.SaveGame();    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode =
                FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode =
                FullScreenMode.Windowed;
        }

        Screen.fullScreen = isFullscreen;

        SaveManager.Instance
            .CurrentData
            .fullscreen =
            isFullscreen;

        SaveManager.Instance.SaveGame();

        Debug.Log(
            "Fullscreen: " +
            Screen.fullScreen +
            " | Mode: " +
            Screen.fullScreenMode
        );
    }
    
    public void BackToMenu()
    {
        if (NavigationManager.OpenedFromPause)
        {
            PauseManager pauseManager =
                FindFirstObjectByType<PauseManager>();

            if (pauseManager != null)
            {
                pauseManager.pausePanel.SetActive(true);
            }

            SceneManager.UnloadSceneAsync("Options");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}