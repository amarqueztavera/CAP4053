using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChemicalBoardManager : MonoBehaviour
{
    public int rows = 6;
    public int columns = 6;
    public Transform puzzleArea;
    public PipeTile[,] grid;

    public Vector2Int startCoords;
    public Vector2Int endCoords;

    public TextMeshProUGUI resultText;
    public Button submitButton;
    public Button resetButton; // 🔁 Added

    private List<PipeTile> solutionPath = new List<PipeTile>();

    void Start()
    {
        StartCoroutine(DelayedSetup());
    }

    IEnumerator DelayedSetup()
    {
        yield return null;

        BuildGrid();
        submitButton.onClick.AddListener(CheckPath);
        resetButton.onClick.AddListener(ResetBoard); // 🔁 Connect button
    }

    void BuildGrid()
    {
        grid = new PipeTile[rows, columns];
        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                PipeTile tile = puzzleArea.GetChild(index).GetComponent<PipeTile>();
                grid[y, x] = tile;
                index++;
            }
        }
    }

    void CheckPath()
    {
        bool[,] visited = new bool[rows, columns];

        // Clear previous highlights
        foreach (PipeTile tile in solutionPath)
            tile.HighlightPath(false);
        solutionPath.Clear();

        bool success = Traverse(startCoords.x, startCoords.y, -1, -1, visited);

        if (success)
        {
            ShowResult("SUCCESS: Chemical reached the flask!", Color.green);
            foreach (PipeTile tile in solutionPath)
                tile.HighlightPath(true);
        }
        else
        {
            ShowResult("Flow is broken.", Color.red);
        }
    }

    void ShowResult(string message, Color color)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = message;
        resultText.color = color;
    }

    // 🔁 Reset and randomize all tiles
    void ResetBoard()
    {
        resultText.gameObject.SetActive(false);

        foreach (PipeTile tile in solutionPath)
            tile.HighlightPath(false);
        solutionPath.Clear();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                grid[y, x].SetRandomType();
            }
        }
    }

    // DFS style path checking
    bool Traverse(int x, int y, int fromX, int fromY, bool[,] visited)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows) return false;
        if (visited[y, x]) return false;

        visited[y, x] = true;

        PipeTile current = grid[y, x];
        solutionPath.Add(current);

        if (new Vector2Int(x, y) == endCoords)
            return true;

        if (current.connectsTop && y > 0 && grid[y - 1, x].connectsBottom)
            if (Traverse(x, y - 1, x, y, visited)) return true;

        if (current.connectsRight && x < columns - 1 && grid[y, x + 1].connectsLeft)
            if (Traverse(x + 1, y, x, y, visited)) return true;

        if (current.connectsBottom && y < rows - 1 && grid[y + 1, x].connectsTop)
            if (Traverse(x, y + 1, x, y, visited)) return true;

        if (current.connectsLeft && x > 0 && grid[y, x - 1].connectsRight)
            if (Traverse(x - 1, y, x, y, visited)) return true;

        solutionPath.Remove(current);
        return false;
    }
}
