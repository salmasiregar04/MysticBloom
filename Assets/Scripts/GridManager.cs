using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Size")]
    public int width = 8;
    public int height = 8;

    [Header("Tile Prefabs")]
    public GameObject[] tilePrefabs;

    [Header("References")]
    public Transform tileContainer;

    private GameObject[,] grid;

    [Header("Animation")]
    public float moveSpeed = 8f;

    void Start()
    {
        grid = new GameObject[width, height];

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTile(x, y);
            }
        }
    }

    public void SpawnTile(int x, int y)
    {
        int randomIndex =
            Random.Range(0, tilePrefabs.Length);

        GameObject tile = Instantiate(
            tilePrefabs[randomIndex],
            new Vector2(x, height + 2),
            Quaternion.identity,
            tileContainer
        );

        Tile tileScript = tile.GetComponent<Tile>();

        tileScript.SetCoordinates(x, y);

        StartCoroutine(
            tileScript.MoveToPosition(
                new Vector2(x, y)
            )
        );

        grid[x, y] = tile;
    }

    public GameObject GetTile(int x, int y)
    {
        return grid[x, y];
    }

    public void SetTile(int x, int y, GameObject tile)
    {
        grid[x, y] = tile;
    }

    public void SwapTilesInGrid(Tile tileA, Tile tileB)
    {
        int ax = tileA.x;
        int ay = tileA.y;

        int bx = tileB.x;
        int by = tileB.y;

        GameObject temp = grid[ax, ay];

        grid[ax, ay] = grid[bx, by];
        grid[bx, by] = temp;
    }

    // =========================
    // MATCH CHECK
    // =========================

    public List<GameObject> FindMatches()
    {
        List<GameObject> matchedTiles =
            new List<GameObject>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject currentTile = grid[x, y];

                if (currentTile == null)
                    continue;

                string currentType =
                    currentTile.GetComponent<TileData>().tileType;

                // HORIZONTAL
                if (x < width - 2)
                {
                    GameObject tile1 = grid[x + 1, y];
                    GameObject tile2 = grid[x + 2, y];

                    if (tile1 != null && tile2 != null)
                    {
                        string type1 =
                            tile1.GetComponent<TileData>().tileType;

                        string type2 =
                            tile2.GetComponent<TileData>().tileType;

                        if (currentType == type1 &&
                            currentType == type2)
                        {
                            matchedTiles.Add(currentTile);
                            matchedTiles.Add(tile1);
                            matchedTiles.Add(tile2);
                        }
                    }
                }

                // VERTICAL
                if (y < height - 2)
                {
                    GameObject tile1 = grid[x, y + 1];
                    GameObject tile2 = grid[x, y + 2];

                    if (tile1 != null && tile2 != null)
                    {
                        string type1 =
                            tile1.GetComponent<TileData>().tileType;

                        string type2 =
                            tile2.GetComponent<TileData>().tileType;

                        if (currentType == type1 &&
                            currentType == type2)
                        {
                            matchedTiles.Add(currentTile);
                            matchedTiles.Add(tile1);
                            matchedTiles.Add(tile2);
                        }
                    }
                }
            }
        }

        return matchedTiles;
    }

    // =========================
    // DESTROY MATCHES
    // =========================

    public IEnumerator DestroyMatches()
    {
        List<GameObject> matchedTiles = FindMatches();

        if (matchedTiles.Count == 0)
            yield break;

        foreach (GameObject tile in matchedTiles)
        {
            if (tile != null)
            {
                TileData data =
                    tile.GetComponent<TileData>();

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
                }

                Tile tileScript =
                    tile.GetComponent<Tile>();

                grid[tileScript.x, tileScript.y] = null;

                StartCoroutine(
                    tile.GetComponent<Tile>()
                        .PopAnimation()
                );
            }
        }

        yield return new WaitForSeconds(0.35f);

        CollapseColumns();

        yield return new WaitForSeconds(0.15f);

        foreach (GameObject tile in matchedTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }
        RefillBoard();

        yield return new WaitForSeconds(0.35f);

        // cek cascade
        StartCoroutine(DestroyMatches());

        yield return new WaitForSeconds(0.35f);

        foreach (GameObject tile in matchedTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }

        CollapseColumns();

        yield return new WaitForSeconds(0.35f);

        RefillBoard();

        // cek apakah masih ada move
        if (!HasPossibleMoves())
        {
            ShuffleBoard();
        }
    }

    // =========================
    // GRAVITY
    // =========================

    void CollapseColumns()
    {
        for (int x = 0; x < width; x++)
        {
            int emptySpaces = 0;

            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    emptySpaces++;
                }
                else if (emptySpaces > 0)
                {
                    GameObject fallingTile =
                        grid[x, y];

                    grid[x, y - emptySpaces] =
                        fallingTile;

                    grid[x, y] = null;

                    Tile tileScript =
                        fallingTile.GetComponent<Tile>();

                    tileScript.SetCoordinates(
                        x,
                        y - emptySpaces
                    );

                    StartCoroutine(
                        tileScript.MoveToPosition(
                            new Vector2(
                                x,
                                y - emptySpaces
                            )
                        )
                    );
                }
            }
        }
    }

    // =========================
    // REFILL
    // =========================

    void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    SpawnTile(x, y);
                }
            }
        }
    }

    // =========================
    // CHECK POSSIBLE MOVES
    // =========================

    public bool HasPossibleMoves()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // cek kanan
                if (x < width - 1)
                {
                    if (WouldCreateMatch(x, y, x + 1, y))
                    {
                        return true;
                    }
                }

                // cek atas
                if (y < height - 1)
                {
                    if (WouldCreateMatch(x, y, x, y + 1))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // =========================
    // SIMULATE SWAP
    // =========================

    bool WouldCreateMatch(
        int x1,
        int y1,
        int x2,
        int y2)
    {
        GameObject tileA = grid[x1, y1];
        GameObject tileB = grid[x2, y2];

        if (tileA == null || tileB == null)
            return false;

        // swap sementara
        grid[x1, y1] = tileB;
        grid[x2, y2] = tileA;

        bool hasMatch =
            CheckMatchAt(x1, y1) ||
            CheckMatchAt(x2, y2);

        // balikin lagi
        grid[x1, y1] = tileA;
        grid[x2, y2] = tileB;

        return hasMatch;
    }

    // =========================
    // CHECK MATCH AT POSITION
    // =========================

    bool CheckMatchAt(int x, int y)
    {
        GameObject centerTile = grid[x, y];

        if (centerTile == null)
            return false;

        string centerType =
            centerTile.GetComponent<TileData>().tileType;

        // horizontal
        int horizontalCount = 1;

        // kiri
        for (int i = x - 1; i >= 0; i--)
        {
            if (grid[i, y] != null &&
                grid[i, y]
                .GetComponent<TileData>()
                .tileType == centerType)
            {
                horizontalCount++;
            }
            else
            {
                break;
            }
        }

        // kanan
        for (int i = x + 1; i < width; i++)
        {
            if (grid[i, y] != null &&
                grid[i, y]
                .GetComponent<TileData>()
                .tileType == centerType)
            {
                horizontalCount++;
            }
            else
            {
                break;
            }
        }

        // vertical
        int verticalCount = 1;

        // bawah
        for (int i = y - 1; i >= 0; i--)
        {
            if (grid[x, i] != null &&
                grid[x, i]
                .GetComponent<TileData>()
                .tileType == centerType)
            {
                verticalCount++;
            }
            else
            {
                break;
            }
        }

        // atas
        for (int i = y + 1; i < height; i++)
        {
            if (grid[x, i] != null &&
                grid[x, i]
                .GetComponent<TileData>()
                .tileType == centerType)
            {
                verticalCount++;
            }
            else
            {
                break;
            }
        }

        return horizontalCount >= 3 ||
            verticalCount >= 3;
    }

    // =========================
    // SHUFFLE BOARD
    // =========================

    public void ShuffleBoard()
    {
        List<GameObject> allTiles =
            new List<GameObject>();

        // ambil semua tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    allTiles.Add(grid[x, y]);
                }
            }
        }

        // shuffle posisi
        foreach (GameObject tile in allTiles)
        {
            int randomX = Random.Range(0, width);
            int randomY = Random.Range(0, height);

            GameObject otherTile =
                grid[randomX, randomY];

            if (otherTile == null)
                continue;

            // tukar posisi visual
            Vector3 tempPos =
                tile.transform.position;

            tile.transform.position =
                otherTile.transform.position;

            otherTile.transform.position =
                tempPos;

            // tukar array
            grid[tile.GetComponent<Tile>().x,
                tile.GetComponent<Tile>().y]
                = otherTile;

            grid[randomX, randomY]
                = tile;

            // tukar koordinat
            int tempX = tile.GetComponent<Tile>().x;
            int tempY = tile.GetComponent<Tile>().y;

            tile.GetComponent<Tile>()
                .SetCoordinates(randomX, randomY);

            otherTile.GetComponent<Tile>()
                .SetCoordinates(tempX, tempY);
        }

        Debug.Log("Board diacak!");
    }

    public IEnumerator RefillAfterHammer()
    {
        CollapseColumns();

        yield return new WaitForSeconds(0.35f);

        RefillBoard();

        yield return new WaitForSeconds(0.35f);

        while (FindMatches().Count > 0)
        {
            yield return StartCoroutine(
                DestroyMatches()
            );
        }
    }
}