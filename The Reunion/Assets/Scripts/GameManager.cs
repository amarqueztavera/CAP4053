using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Tile[] startPoints;
    public Tile[] endPoints;

    void Update()
    {
        if (gridManager == null || !gridManager.IsGridInitialized()) 
            return; // Prevents running before the grid is ready

        // Ensure startPoints and endPoints are assigned
        if (startPoints.Length == 0 || endPoints.Length == 0) 
            return;

        if (PlayerHasDrawnPaths() && CheckWinCondition())
        {
            Debug.Log("🎉 Puzzle Solved! 🎉");
        }
    }

    bool CheckWinCondition()
    {
        foreach (var start in startPoints)
        {
            bool connected = false;

            foreach (var end in endPoints)
            {
                // Ensure path exists between start and end
                if (start.tileColor == end.tileColor && start.isOccupied && end.isOccupied)
                {
                    // Verify that a valid path exists between start and end
                    if (IsPathComplete(start, end))
                    {
                        connected = true;
                        break;
                    }
                }
            }

            if (!connected)
                return false; // If any color is not properly connected, return false
        }
        return true; // All colors are connected
    }

    bool IsPathComplete(Tile start, Tile end)
    {
        // Perform a basic check to ensure all tiles between start & end are occupied
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Tile tile = gridManager.GetTileAt(x, y);
                if (tile == null || tile.tileColor != start.tileColor) // Check if path tiles match the color
                    return false;
            }
        }

        return true; // Path is correctly filled
    }

    bool PlayerHasDrawnPaths()
    {
        foreach (Tile tile in gridManager.GetAllTiles())
        {
            if (tile.isOccupied && !IsStartOrEndTile(tile))
            {
                return true; // The player has drawn at least one path
            }
        }
        return false; // No paths drawn yet
    }

    bool IsStartOrEndTile(Tile tile)
    {
        foreach (Tile start in startPoints)
        {
            if (tile == start) return true;
        }
        foreach (Tile end in endPoints)
        {
            if (tile == end) return true;
        }
        return false;
    }
}
