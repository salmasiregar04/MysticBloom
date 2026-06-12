using UnityEngine;

[CreateAssetMenu(
    fileName = "NewLevelData",
    menuName = "MysticBloom/Level Data"
)]
public class LevelData : ScriptableObject
{
    public int moves;
    public int targetRed;
    public int targetBlue;
}