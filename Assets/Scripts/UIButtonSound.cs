using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButton();
        }
    }
}