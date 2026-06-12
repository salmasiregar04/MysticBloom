using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public Button level2Button;
    public Button level3Button;

    public TMP_Text level2Text;
    public TMP_Text level3Text;

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

        level2Text.text =
            level2Unlocked ? "2" : "[Locked]\n2";

        level3Text.text =
            level3Unlocked ? "3" : "[Locked]\n3";
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