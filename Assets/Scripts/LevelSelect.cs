using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button level2Button;
    public Button level3Button;

    public GameObject level2UnlockedImage;
    public GameObject level2LockedImage;

    public GameObject level3UnlockedImage;
    public GameObject level3LockedImage;

    private void Start()
    {
        bool level2Unlocked =
            PlayerPrefs.GetInt(
                "Level2Unlocked",
                0
            ) == 1;

        bool level3Unlocked =
            PlayerPrefs.GetInt(
                "Level3Unlocked",
                0
            ) == 1;

        level2Button.interactable =
            level2Unlocked;

        level3Button.interactable =
            level3Unlocked;

        level2UnlockedImage.SetActive(
            level2Unlocked
        );

        level2LockedImage.SetActive(
            !level2Unlocked
        );

        level3UnlockedImage.SetActive(
            level3Unlocked
        );

        level3LockedImage.SetActive(
            !level3Unlocked
        );
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}