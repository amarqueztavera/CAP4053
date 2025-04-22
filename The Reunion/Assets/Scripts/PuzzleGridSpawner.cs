using UnityEngine;

public class PuzzleGridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int columns = 5;
    public int rows = 5;

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int i = 0; i < columns * rows; i++)
        {
            GameObject tile = Instantiate(tilePrefab, transform);
            PipeTile pipe = tile.GetComponent<PipeTile>();
            pipe.SetRandomType();
        }
    }

    public void ResetTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            PipeTile pipe = transform.GetChild(i).GetComponent<PipeTile>();
            pipe.SetRandomType(); // re-randomize
        }
    }
}

