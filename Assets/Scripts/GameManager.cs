using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Data")]
    public LevelData levelData;
    
    [Header("Moves")]
    public int movesRemaining = 20;
    public TMP_Text MovesText;

    [Header("Objectives")]
    public int targetRed = 15;
    public int collectedRed = 0;

    public int targetBlue = 10;
    public int collectedBlue = 0;

    public int targetGreen = 10;
    public int collectedGreen = 0;

    public int targetYellow = 5;
    public int collectedYellow = 0;

    public TMP_Text RedObjectiveText;
    public TMP_Text BlueObjectiveText;
    public TMP_Text GreenObjectiveText;
    public TMP_Text YellowObjectiveText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (levelData != null)
        {
            movesRemaining =
                levelData.moves;

            targetRed =
                levelData.targetRed;

            targetBlue =
                levelData.targetBlue;

            targetGreen =
                levelData.targetGreen;

            targetYellow =
                levelData.targetYellow;
        }

        UpdateMovesUI();
        UpdateObjectiveUI();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);
    }

    // =========================
    // MOVES
    // =========================

    public void UseMove()
    {
        if (gameEnded)
            return;

        movesRemaining--;

        UpdateMovesUI();

        CheckLoseCondition();
    }

    void UpdateMovesUI()
    {
        if (MovesText != null)
        {
            MovesText.text =
                "Moves: " + movesRemaining;
        }
    }

    // =========================
    // OBJECTIVES
    // =========================

    public void CollectRed()
    {
        if (gameEnded)
            return;

        collectedRed++;

        UpdateObjectiveUI();

        CheckWinCondition();
    }

    public void CollectBlue()
    {
        if (gameEnded)
            return;

        collectedBlue++;

        UpdateObjectiveUI();

        CheckWinCondition();
    }

    public void CollectGreen()
    {
        if (gameEnded)
            return;

        collectedGreen++;

        UpdateObjectiveUI();

        CheckWinCondition();
    }

    public void CollectYellow()
    {
        if (gameEnded)
            return;

        collectedYellow++;

        UpdateObjectiveUI();

        CheckWinCondition();
    }

    void UpdateObjectiveUI()
    {
        int redRemaining =
            Mathf.Max(0, targetRed - collectedRed);

        int blueRemaining =
            Mathf.Max(0, targetBlue - collectedBlue);

        int greenRemaining =
            Mathf.Max(0, targetGreen - collectedGreen);
            
        int yellowRemaining =
            Mathf.Max(0, targetYellow - collectedYellow);

        if (RedObjectiveText != null)
        {
            RedObjectiveText.text =
                redRemaining.ToString();
        }

        if (BlueObjectiveText != null)
        {
            BlueObjectiveText.text =
                blueRemaining.ToString();
        }

        if (GreenObjectiveText != null)
        {
            GreenObjectiveText.text =
                greenRemaining.ToString();
        }

        if (YellowObjectiveText != null)
        {
            YellowObjectiveText.text =
                yellowRemaining.ToString();
        }
    }

    // =========================
    // WIN
    // =========================

    void CheckWinCondition()
    {
        Debug.Log(
            "Red: " + collectedRed + "/" + targetRed +
            " | Blue: " + collectedBlue + "/" + targetBlue +
            " | Green: " + collectedGreen + "/" + targetGreen +
            " | Yellow: " + collectedYellow + "/" + targetYellow
        );

        if (
            collectedRed >= targetRed &&
            collectedBlue >= targetBlue &&
            collectedGreen >= targetGreen &&
            collectedYellow >= targetYellow
        )
        {
            Debug.Log("YOU WIN");

            AudioManager.Instance.PlayWin();

            gameEnded = true;

            string currentLevel =
                SceneManager.GetActiveScene().name;

            if (currentLevel == "Level1")
            {
                PlayerPrefs.SetInt(
                    "Level2Unlocked",
                    1
                );
            }

            if (currentLevel == "Level2")
            {
                PlayerPrefs.SetInt(
                    "Level3Unlocked",
                    1
                );
            }

            PlayerPrefs.Save();

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
        }
    }
    // =========================
    // LOSE
    // =========================

    void CheckLoseCondition()
    {
        if (
            movesRemaining <= 0 &&
            (
                collectedRed < targetRed ||
                collectedBlue < targetBlue ||
                collectedGreen < targetGreen ||
                collectedYellow < targetYellow  
            )
           )
        {
            gameEnded = true;

            Debug.Log("YOU LOSE");

            AudioManager.Instance.PlayLose();

            if (losePanel != null)
            {
                losePanel.SetActive(true);
            }
        }
    }
}