using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance;

    public bool hammerMode = false;

    public int hammerCount = 3;
    public int shuffleCount = 2;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateHammer()
    {
        if (hammerCount <= 0)
            return;

        hammerCount--;

        hammerMode = true;

        Debug.Log("Hammer Activated");
    }

    public void UseShuffle()
    {
        if (shuffleCount <= 0)
            return;

        shuffleCount--;

        FindFirstObjectByType<GridManager>()
            .ShuffleBoard();

        Debug.Log("Shuffle Used");
    }
}