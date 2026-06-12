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
        musicSlider.value =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        AudioListener.volume =
            musicSlider.value;
        sfxSlider.value = 1f;

        fullscreenToggle.isOn =
            Screen.fullScreen;
    }

    public void SetMusicVolume(float value)
    {
        Debug.Log("Music Volume: " + value);
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        Debug.Log("SFX Volume: " + value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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