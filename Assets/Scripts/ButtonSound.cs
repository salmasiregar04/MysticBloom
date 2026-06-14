using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButton()
    {
        AudioManager.Instance.PlayButton();
    }
}