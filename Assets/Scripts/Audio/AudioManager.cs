using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip swapClip;
    public AudioClip matchClip;
    public AudioClip buttonClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySwap()
    {
        sfxSource.PlayOneShot(swapClip);
    }

    public void PlayMatch()
    {
        sfxSource.PlayOneShot(matchClip);
    }

    public void PlayButton()
    {
        Debug.Log("Button Sound");
        sfxSource.PlayOneShot(buttonClip);
    }

    public void PlayWin()
    {
        sfxSource.PlayOneShot(winClip);
    }

    public void PlayLose()
    {
        sfxSource.PlayOneShot(loseClip);
    }

        public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}