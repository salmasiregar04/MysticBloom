using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Vector2 startMousePosition;

    private bool hasSwiped = false;

    private float swipeThreshold = 0.3f;

    private GridManager gridManager;

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    public void SetCoordinates(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    void OnMouseDown()
    {
        if (BoosterManager.Instance != null &&
            BoosterManager.Instance.hammerMode)
        {
            StartCoroutine(HammerDestroy());

            BoosterManager.Instance.hammerMode = false;

            return;
        }

        startMousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hasSwiped = false;
    }

    void OnMouseDrag()
    {
        if (hasSwiped)
            return;

        Vector2 currentMousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 difference =
            currentMousePosition - startMousePosition;

        if (difference.magnitude < swipeThreshold)
            return;

        hasSwiped = true;

        if (Mathf.Abs(difference.x) >
            Mathf.Abs(difference.y))
        {
            if (difference.x > 0)
            {
                TrySwap(x + 1, y);
            }
            else
            {
                TrySwap(x - 1, y);
            }
        }
        else
        {
            if (difference.y > 0)
            {
                TrySwap(x, y + 1);
            }
            else
            {
                TrySwap(x, y - 1);
            }
        }
    }

    void TrySwap(int targetX, int targetY)
    {
        if (targetX < 0 || targetX >= gridManager.width ||
            targetY < 0 || targetY >= gridManager.height)
        {
            return;
        }

        GameObject targetTile =
            gridManager.GetTile(targetX, targetY);

        if (targetTile != null)
        {
            StartCoroutine(
                SwapAndCheckMatch(
                    targetTile.GetComponent<Tile>()
                )
            );
        }
    }

    IEnumerator SwapAndCheckMatch(Tile otherTile)
    {
        yield return StartCoroutine(
            SwapTiles(otherTile)
        );

        var matches = gridManager.FindMatches();

        if (matches.Count == 0)
        {
            yield return StartCoroutine(
                SwapTiles(otherTile)
            );        }
        else
        {
            GameManager.Instance.UseMove();

            StartCoroutine(
                gridManager.DestroyMatches()
            );
        }
    }

    IEnumerator SwapTiles(Tile otherTile)
    {
        Vector3 myPosition = transform.position;
        Vector3 otherPosition = otherTile.transform.position;

        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position =
                Vector3.Lerp(
                    myPosition,
                    otherPosition,
                    elapsed / duration
                );

            otherTile.transform.position =
                Vector3.Lerp(
                    otherPosition,
                    myPosition,
                    elapsed / duration
                );

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = otherPosition;
        otherTile.transform.position = myPosition;

        int myX = x;
        int myY = y;

        int otherX = otherTile.x;
        int otherY = otherTile.y;

        x = otherX;
        y = otherY;

        otherTile.x = myX;
        otherTile.y = myY;

        gridManager.SwapTilesInGrid(this, otherTile);
    }

    public IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (
            Vector3.Distance(
                transform.position,
                targetPosition
            ) > 0.01f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    gridManager.moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        transform.position = targetPosition;
    }

    public IEnumerator PopAnimation()
    {
        Vector3 originalScale = transform.localScale;

        // membesar sedikit
        float growDuration = 0.05f;
        float t = 0f;

        while (t < growDuration)
        {
            transform.localScale =
                Vector3.Lerp(
                    originalScale,
                    originalScale * 1.15f,
                    t / growDuration
                );

            t += Time.deltaTime;
            yield return null;
        }

        // mengecil sampai hilang
        float shrinkDuration = 0.1f;
        t = 0f;

        Vector3 currentScale =
            transform.localScale;

        while (t < shrinkDuration)
        {
            transform.localScale =
                Vector3.Lerp(
                    currentScale,
                    Vector3.zero,
                    t / shrinkDuration
                );

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }

    public IEnumerator HammerDestroy()
    {
        TileData data =
            GetComponent<TileData>();

        if (data != null)
        {
            if (data.tileType == "Red")
            {
                GameManager.Instance.CollectRed();
            }

            if (data.tileType == "Blue")
            {
                GameManager.Instance.CollectBlue();
            }

            if (data.tileType == "Green")
            {
                GameManager.Instance.CollectGreen();
            }

            if (data.tileType == "Yellow")
            {
                GameManager.Instance.CollectYellow();
            }
        }

        yield return StartCoroutine(
            PopAnimation()
        );

        GridManager grid =
            FindFirstObjectByType<GridManager>();

        grid.SetTile(x, y, null);

        Destroy(gameObject);

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(
            grid.RefillAfterHammer()
        );    
    }
}