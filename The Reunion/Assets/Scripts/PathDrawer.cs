using UnityEngine;
using System.Collections.Generic;

public class PathDrawer : MonoBehaviour
{
    public GridManager gridManager;
    private Color selectedColor;
    private bool isDrawing = false;
    private List<Tile> drawnTiles = new List<Tile>();
    private Tile startTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start drawing
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile tile = GetTileAtPosition(mousePos);

            if (tile != null && tile.isOccupied) // ✅ Must start from a fixed start tile
            {
                selectedColor = tile.tileColor;
                isDrawing = true;
                drawnTiles.Clear();
                drawnTiles.Add(tile);
                startTile = tile;
                Debug.Log("Started path from tile: " + tile);
            }
        }

        if (Input.GetMouseButton(0) && isDrawing) // Continue drawing
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile tile = GetTileAtPosition(mousePos);

            if (tile != null && tile.tileColor == Color.white) // ✅ Only draw on blank tiles
            {
                tile.SetColor(selectedColor);
                drawnTiles.Add(tile);
            }
        }

        if (Input.GetMouseButtonUp(0)) // Stop drawing
        {
            isDrawing = false;

            if (!PathEndsCorrectly()) // ✅ If the path does not end correctly, reset it
            {
                Debug.Log("❌ Invalid path: resetting...");
                ResetPath();
            }
            else
            {
                Debug.Log("✅ Valid path drawn!");
            }
        }
    }

    Tile GetTileAtPosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return hit.collider != null ? hit.collider.GetComponent<Tile>() : null;
    }

    bool PathEndsCorrectly()
    {
        if (drawnTiles.Count == 0) return false;

        Tile lastTile = drawnTiles[drawnTiles.Count - 1]; // Get last drawn tile

        foreach (Tile tile in gridManager.GetAllTiles()) // Check if last tile is an endpoint
        {
            if (tile.tileColor == selectedColor && tile.isOccupied && tile != startTile)
            {
                return tile == lastTile; // ✅ Must end at the correct endpoint
            }
        }
        return false;
    }

    void ResetPath()
    {
        Debug.Log("❌ Invalid path! Resetting...");

        foreach (Tile tile in drawnTiles)
        {
            if (!tile.isOccupied) // ✅ Only reset non-fixed tiles
            {
                Debug.Log("Clearing tile at position: " + tile.x + ", " + tile.y);
                tile.ClearTile();
            }
        }
        drawnTiles.Clear(); // Clear the drawn path list
    }
}
