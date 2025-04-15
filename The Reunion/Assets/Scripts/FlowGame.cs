using System.Collections.Generic;
using UnityEngine;

public class FlowGame : MonoBehaviour
{
    [Header("Game Settings")]
    public float lineThickness = 0.2f;
    public float clickRadius = 0.5f;
    
    [Header("Debug Settings")]
    public bool showColliders = true;
    public bool verboseLogging = true;
    
    private Dictionary<Color, Transform> startDots = new Dictionary<Color, Transform>();
    private Dictionary<Color, Transform> endDots = new Dictionary<Color, Transform>();
    private Dictionary<Color, List<Vector2Int>> colorPaths = new Dictionary<Color, List<Vector2Int>>();
    private Color selectedColor;
    private bool isDrawing = false;
    private Vector2Int lastGridPos;

    void Start()
    {
        Log("Initializing game...");
        FindAllDots();
        InitializeColorPaths();
        Log($"Found {startDots.Count} color pairs to connect");
        CheckLayerSetup();
    }

    void Update()
    {
        if (showColliders)
        {
            DrawDebugColliders();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Log("Mouse button down - attempting to start drawing");
            TryStartDrawing();
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Log("Mouse button up - stopping drawing");
            StopDrawing();
        }
    }

    void InitializeColorPaths()
    {
        colorPaths.Clear();
        foreach (var color in startDots.Keys)
        {
            colorPaths[color] = new List<Vector2Int>();
            Log($"Initialized path for color: {color}");
        }
    }

    void FindAllDots()
    {
        startDots.Clear();
        endDots.Clear();

        SpriteRenderer[] allRenderers = FindObjectsOfType<SpriteRenderer>();
        Log($"Scanning {allRenderers.Length} SpriteRenderers for dots...");
        
        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr.gameObject.CompareTag("Dot"))
            {
                Color dotColor = sr.color;
                Log($"Found dot with color: {dotColor} at {sr.transform.position}");
                
                if (!startDots.ContainsKey(dotColor))
                {
                    startDots[dotColor] = sr.transform;
                    Log($"Registered as START dot for color {dotColor}");
                }
                else if (!endDots.ContainsKey(dotColor))
                {
                    endDots[dotColor] = sr.transform;
                    Log($"Registered as END dot for color {dotColor}");
                }
            }
        }
    }

    void TryStartDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Log($"Checking click at world position: {mousePos}");

        int dotLayerMask = 1 << LayerMask.NameToLayer("Dots");
        Collider2D[] dotHits = Physics2D.OverlapCircleAll(mousePos, clickRadius, dotLayerMask);
        
        Log($"Found {dotHits.Length} dot colliders at click position");
        
        foreach (Collider2D hit in dotHits)
        {
            if (hit != null && hit.CompareTag("Dot"))
            {
                Log($"Hit dot: {hit.name} at {hit.transform.position}");
                
                SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null && startDots.ContainsKey(sr.color))
                {
                    selectedColor = sr.color;
                    isDrawing = true;
                    Log($"Started drawing path for color: {selectedColor}");
                    ClearPath(selectedColor);
                    lastGridPos = GetGridPosition(hit.transform.position);
                    AddToPath(selectedColor, lastGridPos);
                    return;
                }
            }
        }
        
        LogWarning("No valid dot clicked. Possible issues:");
        LogWarning($"- Dot layer: {LayerMask.LayerToName(LayerMask.NameToLayer("Dots"))}");
        LogWarning($"- Collider enabled: {(dotHits.Length > 0 ? dotHits[0].enabled.ToString() : "no hits")}");
    }

    void ContinueDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform currentCell = GetCellUnderMouse(mousePos);
        
        if (currentCell != null)
        {
            Vector2Int gridPos = GetGridPosition(currentCell.position);
            if (gridPos != lastGridPos && IsValidMove(gridPos))
            {
                Log($"Adding point to path: {gridPos}");
                AddToPath(selectedColor, gridPos);
                lastGridPos = gridPos;
            }
        }
    }

    void StopDrawing()
    {
        if (!isDrawing) return;
        
        isDrawing = false;
        Log($"Finished drawing path for color: {selectedColor}");
        CheckWinCondition();
    }

    Transform GetCellUnderMouse(Vector2 mousePos)
    {
        int cellLayerMask = 1 << LayerMask.NameToLayer("Cells");
        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, clickRadius, cellLayerMask);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Cell"))
                return hit.transform;
        }
        
        LogWarning($"No cell found at {mousePos}");
        return null;
    }

    Vector2Int GetGridPosition(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }

    bool IsValidMove(Vector2Int gridPos)
    {
        if (!colorPaths.ContainsKey(selectedColor)) 
        {
            LogWarning($"No path exists for color: {selectedColor}");
            return false;
        }

        if (colorPaths[selectedColor].Count > 0)
        {
            Vector2Int lastPos = colorPaths[selectedColor][colorPaths[selectedColor].Count - 1];
            if (Mathf.Abs(gridPos.x - lastPos.x) + Mathf.Abs(gridPos.y - lastPos.y) != 1)
            {
                Log($"Position {gridPos} is not adjacent to last position {lastPos}");
                return false;
            }
        }

        foreach (var path in colorPaths)
        {
            if (path.Key != selectedColor && path.Value.Contains(gridPos))
            {
                Log($"Position {gridPos} is already in another path");
                return false;
            }
        }

        return true;
    }

    void AddToPath(Color color, Vector2Int pos)
    {
        if (!colorPaths.ContainsKey(color))
            colorPaths[color] = new List<Vector2Int>();
        
        colorPaths[color].Add(pos);
        DrawPaths();
    }

    void ClearPath(Color color)
    {
        if (colorPaths.ContainsKey(color))
            colorPaths[color].Clear();
        
        DrawPaths();
    }

    void DrawPaths()
    {
        LineRenderer[] oldLines = FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer line in oldLines)
        {
            if (line.gameObject.name == "Line")
                Destroy(line.gameObject);
        }

        foreach (var kvp in colorPaths)
        {
            if (kvp.Value.Count < 2) continue;

            GameObject lineObj = new GameObject("Line");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.startWidth = lineThickness;
            lr.endWidth = lineThickness;
            lr.material = new Material(Shader.Find("Sprites/Default")) { color = kvp.Key };
            lr.useWorldSpace = true;

            Vector3[] positions = new Vector3[kvp.Value.Count];
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                positions[i] = new Vector3(kvp.Value[i].x, kvp.Value[i].y, 0);
            }
            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
        }
    }

    void CheckWinCondition()
    {
        if (startDots.Count != endDots.Count)
        {
            LogWarning($"Mismatched start/end dots: {startDots.Count} starts vs {endDots.Count} ends");
            return;
        }

        foreach (Color color in startDots.Keys)
        {
            if (!colorPaths.ContainsKey(color) || colorPaths[color].Count == 0)
            {
                Log($"No path exists for color: {color}");
                return;
            }

            Vector2Int lastPos = colorPaths[color][colorPaths[color].Count - 1];
            Vector2Int endPos = GetGridPosition(endDots[color].position);
            
            if (lastPos != endPos)
            {
                Log($"Path for {color} doesn't reach end. Last: {lastPos}, End: {endPos}");
                return;
            }
        }

        Log("YOU WIN! All paths connected correctly!");
    }

    void DrawDebugColliders()
    {
        foreach (var dot in GameObject.FindGameObjectsWithTag("Dot"))
        {
            CircleCollider2D collider = dot.GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                GizmosUtils.DrawWireCircle(
                    dot.transform.position, 
                    collider.radius, 
                    Color.green,
                    0.1f
                );
            }
        }
    }

    void CheckLayerSetup()
    {
        GameObject testDot = GameObject.FindWithTag("Dot");
        if (testDot != null)
        {
            Log($"Dot layer: {testDot.layer} ({LayerMask.LayerToName(testDot.layer)})");
            Log($"Collider radius: {testDot.GetComponent<CircleCollider2D>()?.radius ?? -1f}");
        }
    }

    void Log(string message)
    {
        if (verboseLogging) Debug.Log($"[FlowGame] {message}");
    }

    void LogWarning(string message)
    {
        Debug.LogWarning($"[FlowGame] {message}");
    }
}

public static class GizmosUtils
{
    public static void DrawWireCircle(Vector3 center, float radius, Color color, float duration = 0.1f)
    {
        Vector3 prevPos = center + new Vector3(radius, 0, 0);
        for (int i = 0; i <= 30; i++)
        {
            float angle = (float)i / 30 * Mathf.PI * 2;
            Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Debug.DrawLine(prevPos, newPos, color, duration);
            prevPos = newPos;
        }
    }
}