using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData CurrentData;

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
        string json =
            JsonUtility.ToJson(
                CurrentData,
                true
            );

        File.WriteAllText(
            savePath,
            json
        );

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json =
                File.ReadAllText(savePath);

            CurrentData =
                JsonUtility.FromJson<SaveData>(
                    json
                );
        }
        else
        {
            CurrentData =
                new SaveData();

            SaveGame();
        }

        Debug.Log("Game Loaded");
    }
}