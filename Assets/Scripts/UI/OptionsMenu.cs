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
        PlayerPrefs.GetFloat("MusicVolume", 1f);

    float sfxVolume =
        PlayerPrefs.GetFloat("SFXVolume", 1f);

    musicSlider.value = musicVolume;
    sfxSlider.value = sfxVolume;
    bool fullscreen =
        PlayerPrefs.GetInt(
            "Fullscreen",
            1
        ) == 1;

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

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

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