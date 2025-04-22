using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public Color tileColor = Color.white;
    public bool isOccupied = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetColor(Color color)
    {
        if (!isOccupied) // Prevent overwriting fixed tiles
        {
            tileColor = color;
            isOccupied = true;
            spriteRenderer.color = color;
        }
    }

    public void ClearTile()
    {
        if (tileColor != Color.white) // Only clear if not already empty
        {
            Debug.Log("Clearing tile at position: " + x + ", " + y); // Debug log
            tileColor = Color.white;
            spriteRenderer.color = Color.white;
            isOccupied = false; // Reset occupied status after clearing
        }
    }
}
