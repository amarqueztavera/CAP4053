using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlowGame : MonoBehaviour
{
    [Header("Line Settings")]
    public float lineThickness = 0.2f;
    public float clickRadius = 0.5f;

    private readonly Dictionary<Color, (Vector2Int start, Vector2Int end)> dotPositions = new()
    {
        { Color.red,    (new Vector2Int(0,4), new Vector2Int(1,0)) },
        { Color.blue,   (new Vector2Int(2,0), new Vector2Int(2,3)) },
        { Color.green,  (new Vector2Int(1,1), new Vector2Int(2,4)) },
        { Color.yellow, (new Vector2Int(4,4), new Vector2Int(3,1)) },
        { Color.magenta,(new Vector2Int(3,0), new Vector2Int(4,3)) }
    };

    private readonly Dictionary<Color, List<Vector2Int>> correctPaths = new()
    {
        { Color.red,     new List<Vector2Int> { new(0,4), new(0,3), new(0,2), new(0,1), new(0,0), new(1,0) } },
        { Color.green,   new List<Vector2Int> { new(1,1), new(1,2), new(1,3), new(1,4), new(2,4) } },
        { Color.blue,    new List<Vector2Int> { new(2,0), new(2,1), new(2,2), new(2,3) } },
        { Color.magenta, new List<Vector2Int> { new(3,0), new(4,0), new(4,1), new(4,2), new(4,3) } },
        { Color.yellow,  new List<Vector2Int> { new(3,1), new(3,2), new(3,3), new(3,4), new(4,4) } },
    };

    private Dictionary<Color, Transform> startDots = new();
    private Dictionary<Color, Transform> endDots = new();
    private Dictionary<Color, List<Vector3>> colorPaths = new();
    private Color selectedColor;
    private bool isDrawing = false;

    void Start()
    {
        InitializeDots();
        InitializeColorPaths();
        //CenterCamera();
    }

    void InitializeDots()
    {
        foreach (GameObject dot in GameObject.FindGameObjectsWithTag("Dot"))
        {
            Vector2Int pos = new(
                Mathf.RoundToInt(dot.transform.position.x),
                Mathf.RoundToInt(dot.transform.position.y)
            );

            foreach (var kvp in dotPositions)
            {
                if (pos == kvp.Value.start)
                {
                    startDots[kvp.Key] = dot.transform;
                    dot.GetComponent<SpriteRenderer>().color = kvp.Key;
                }
                else if (pos == kvp.Value.end)
                {
                    endDots[kvp.Key] = dot.transform;
                    dot.GetComponent<SpriteRenderer>().color = kvp.Key;
                }
            }
        }
    }

    void InitializeColorPaths()
    {
        colorPaths.Clear();
        var allColors = startDots.Keys.Union(endDots.Keys);
        foreach (var color in allColors)
        {
            colorPaths[color] = new List<Vector3>();
        }
    }

    /*void CenterCamera()
    {
        Camera.main.orthographicSize = 3.5f;
        Camera.main.transform.position = new Vector3(2.5f, 2.5f, -10f);
    }*/

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryStartDrawing();
        else if (Input.GetMouseButton(0) && isDrawing) ContinueDrawing();
        else if (Input.GetMouseButtonUp(0)) StopDrawing();
    }

    void TryStartDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);

        if (hit != null && hit.CompareTag("Dot"))
        {
            SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();
            if (sr != null && (startDots.ContainsKey(sr.color) || endDots.ContainsKey(sr.color)))
            {
                selectedColor = sr.color;
                isDrawing = true;
                ClearPath(selectedColor);
                colorPaths[selectedColor].Add(hit.transform.position);
            }
        }
    }

    void ContinueDrawing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);

        if (hit != null && hit.CompareTag("Cell"))
        {
            Vector3 cellCenter = new(
                Mathf.Round(mousePos.x),
                Mathf.Round(mousePos.y),
                0
            );

            if (colorPaths[selectedColor].Count == 0 ||
                Vector3.Distance(colorPaths[selectedColor][^1], cellCenter) > 0.1f)
            {
                if (IsValidMove(cellCenter))
                {
                    colorPaths[selectedColor].Add(cellCenter);
                    DrawPaths();
                }
            }
        }
    }

    void StopDrawing()
    {
        if (!isDrawing) return;

        isDrawing = false;

        if (!IsPathCorrect(selectedColor))
        {
            Debug.Log($"Incorrect path for {selectedColor}. Clearing.");
            ClearPath(selectedColor);
        }

        CheckWinCondition();
    }

    bool IsValidMove(Vector3 position)
    {
        if (colorPaths[selectedColor].Count > 0)
        {
            Vector3 lastPos = colorPaths[selectedColor][^1];
            if (Vector3.Distance(lastPos, position) > 1.1f) return false;
        }

        foreach (var path in colorPaths)
        {
            if (path.Key != selectedColor && path.Value.Contains(position)) return false;
        }

        return true;
    }

    bool IsPathCorrect(Color color)
    {
        if (!colorPaths.ContainsKey(color)) return false;
        List<Vector3> drawn = colorPaths[color];
        List<Vector2Int> correct = correctPaths[color];

        if (drawn.Count != correct.Count) return false;

        for (int i = 0; i < correct.Count; i++)
        {
            Vector2Int expected = correct[i];
            Vector2Int drawnPoint = new Vector2Int(Mathf.RoundToInt(drawn[i].x), Mathf.RoundToInt(drawn[i].y));

            if (drawnPoint != expected)
                return false;
        }

        return true;
    }

    void DrawPaths()
    {
        foreach (LineRenderer line in FindObjectsOfType<LineRenderer>())
        {
            Destroy(line.gameObject);
        }

        foreach (var kvp in colorPaths)
        {
            if (kvp.Value.Count < 2) continue;

            GameObject lineObj = new($"Line_{kvp.Key}");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = kvp.Value.Count;
            lr.SetPositions(kvp.Value.ToArray());
            lr.startWidth = lineThickness;
            lr.endWidth = lineThickness;
            lr.useWorldSpace = true;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = kvp.Key;
            lr.endColor = kvp.Key;
            lr.numCapVertices = 8;
        }
    }

    void ClearPath(Color color)
    {
        if (colorPaths.ContainsKey(color)) colorPaths[color].Clear();
        DrawPaths();
    }

    void CheckWinCondition()
    {
        foreach (var color in correctPaths.Keys)
        {
            if (!IsPathCorrect(color))
            {
                Debug.Log($"Path not complete or incorrect for {color}");
                return;
            }
        }

        Debug.Log("YOU WIN! All paths connected correctly!");
    }
}
