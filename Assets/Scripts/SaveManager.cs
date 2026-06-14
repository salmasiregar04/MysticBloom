using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

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

        savePath =
            Application.persistentDataPath +
            "/save.json";

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.level2Unlocked =
            PlayerPrefs.GetInt(
                "Level2Unlocked",
                0
            ) == 1;

        data.level3Unlocked =
            PlayerPrefs.GetInt(
                "Level3Unlocked",
                0
            ) == 1;

        string json =
            JsonUtility.ToJson(
                data,
                true
            );

        File.WriteAllText(
            savePath,
            json
        );

        Debug.Log(
            "Game Saved: " +
            savePath
        );
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
            return;

        string json =
            File.ReadAllText(
                savePath
            );

        SaveData data =
            JsonUtility.FromJson<SaveData>(
                json
            );

        PlayerPrefs.SetInt(
            "Level2Unlocked",
            data.level2Unlocked ? 1 : 0
        );

        PlayerPrefs.SetInt(
            "Level3Unlocked",
            data.level3Unlocked ? 1 : 0
        );

        PlayerPrefs.Save();

        Debug.Log("Game Loaded");
    }
}