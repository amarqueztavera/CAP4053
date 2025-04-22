using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 5;
    private Tile[,] grid;

    void Start()
    {
        CreateGrid(); // This ensures the grid is created when the scene starts
    }


    public void CreateGrid()
    {
        grid = new Tile[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObj.transform.parent = transform;

                Tile tile = tileObj.GetComponent<Tile>();
                tile.SetPosition(x, y);
                grid[x, y] = tile;
            }
        }

        // Set predefined start & end points
        SetFixedTiles();
    }

    void SetFixedTiles()
    {
        grid[0, 0].SetColor(Color.red);  // Red start
        grid[4, 4].SetColor(Color.red);  // Red end
        grid[0, 4].SetColor(Color.blue); // Blue start
        grid[4, 0].SetColor(Color.blue); // Blue end
        grid[2, 2].SetColor(Color.yellow); // Yellow start
        grid[3, 3].SetColor(Color.yellow); // Yellow end
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            return grid[x, y];
        return null;
    }

    public Tile[] GetAllTiles()
    {
        if (grid == null) return new Tile[0]; // Prevents the null error

        Tile[] tiles = new Tile[gridSize * gridSize];
        int index = 0;
        foreach (Tile tile in grid)
        {
            tiles[index++] = tile;
        }
        return tiles;
    }


    public bool IsGridInitialized()
    {
        return grid != null;
    }

}
