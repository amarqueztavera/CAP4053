using UnityEngine;
using System.Collections.Generic;

public class DotConnection : MonoBehaviour
{
    public Color[] dotColors; // Assign colors in inspector corresponding to dot types
    public LineRenderer linePrefab; // Assign a LineRenderer prefab in inspector
    
    private Dictionary<Vector2Int, GameObject> cells = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> dots = new Dictionary<Vector2Int, GameObject>();
    private LineRenderer currentLine;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private Color currentColor;
    private bool isDrawing = false;
    private int successfulConnections = 0;
    private const int WIN_CONDITION = 5;
    [Header("Clue Settings")]
    public string clueID = "note"; // The ID of the clue to be added


    void Start()
    {
        // Initialize cell and dot positions
        InitializeGrid();
    }

    void InitializeGrid()
    {
        GameObject[] cellObjects = GameObject.FindGameObjectsWithTag("Cell");
        foreach (GameObject cell in cellObjects)
        {
            Vector2Int pos = new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.y);
            cells[pos] = cell;
        }

        GameObject[] dotObjects = GameObject.FindGameObjectsWithTag("Dot");
        foreach (GameObject dot in dotObjects)
        {
            Vector2Int pos = new Vector2Int((int)dot.transform.position.x, (int)dot.transform.position.y);
            dots[pos] = dot;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (isDrawing && Input.GetMouseButton(0))
        {
            ContinueDrawing();
        }
        else if (isDrawing && Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }
    }

    void StartDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        // Check if we clicked on a dot
        if (dots.TryGetValue(gridPos, out GameObject dot))
        {
            // Find the color of the dot (assuming you have a Dot script with a colorIndex)
            Dot dotScript = dot.GetComponent<Dot>();
            if (dotScript != null && !dotScript.connected) // Only start from unconnected dots
            {
                currentColor = dotColors[dotScript.colorIndex];
                currentPath.Clear();
                currentPath.Add(gridPos);
                
                // Create line renderer
                currentLine = Instantiate(linePrefab);
                currentLine.startColor = currentColor;
                currentLine.endColor = currentColor;
                currentLine.positionCount = 1;
                currentLine.SetPosition(0, (Vector2)gridPos);
                
                isDrawing = true;
            }
        }
    }

    void ContinueDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        // Check if we're still on the grid
        if (!cells.ContainsKey(gridPos)) return;

        // Check if this is a new position and adjacent to last position
        if (currentPath.Count > 0 && !currentPath.Contains(gridPos))
        {
            Vector2Int lastPos = currentPath[currentPath.Count - 1];
            if (IsAdjacent(lastPos, gridPos))
            {
                // Check if we hit another dot
                if (dots.TryGetValue(gridPos, out GameObject dot))
                {
                    Dot dotScript = dot.GetComponent<Dot>();
                    if (dotScript != null && !dotScript.connected && dotColors[dotScript.colorIndex] == currentColor)
                    {
                        // Valid connection - complete the line
                        currentPath.Add(gridPos);
                        UpdateLineRenderer();
                        
                        // Mark dots as connected
                        MarkDotsAsConnected();
                        
                        successfulConnections++;
                        CheckWinCondition();
                        
                        isDrawing = false;
                        return;
                    }
                    else
                    {
                        // Wrong color dot or already connected - cancel drawing
                        CancelDrawing();
                        return;
                    }
                }

                // Add to path
                currentPath.Add(gridPos);
                UpdateLineRenderer();
            }
        }
    }

    void MarkDotsAsConnected()
    {
        foreach (Vector2Int pos in currentPath)
        {
            if (dots.TryGetValue(pos, out GameObject dot))
            {
                Dot dotScript = dot.GetComponent<Dot>();
                if (dotScript != null)
                {
                    dotScript.connected = true;
                }
            }
        }
    }

    void CheckWinCondition()
    {
        if (successfulConnections >= WIN_CONDITION)
        {
            Debug.Log("You win!");
            Debug.Log("All pieces are in the right position!");
                        // Add code to handle the completion of the puzzle here
                        //yield return new WaitForSeconds(1.5f); // Adjust time as needed

                        SaveSystem.MarkPuzzleComplete(clueID); 
                        ClueEventManager.PuzzleCompleted(clueID); 
                        Clue clueFromDB = ClueDatabase.Instance.GetClueByName(clueID);
                        if (clueFromDB != null)
                        {
                            InventoryManager.Instance.AddClue(clueFromDB);
                            Debug.Log($"Clue '{clueID}' added from database.");
                        }
                        else {
                            Debug.LogError($"Clue '{clueID}' NOT found in database!");
                        }

                        // Force immediate save
                        PlayerPrefs.Save();

                        Debug.Log("Puzzle completed! Returning to map...");
                        PuzzleSceneSwapper.Instance.ReturnToMap();
            // You could add more win behavior here like showing a win screen
        }
    }

    void EndDrawing()
    {
        // If we didn't connect to a dot of the same color, cancel
        CancelDrawing();
    }

    void CancelDrawing()
    {
        if (currentLine != null)
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
        currentPath.Clear();
        isDrawing = false;
    }

    void UpdateLineRenderer()
    {
        if (currentLine != null)
        {
            currentLine.positionCount = currentPath.Count;
            for (int i = 0; i < currentPath.Count; i++)
            {
                currentLine.SetPosition(i, (Vector2)currentPath[i]);
            }
        }
    }

    bool IsAdjacent(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) <= 1 && Mathf.Abs(pos1.y - pos2.y) <= 1 && 
               !(Mathf.Abs(pos1.x - pos2.x) == 1 && Mathf.Abs(pos1.y - pos2.y) == 1); // No diagonals!
    }
}