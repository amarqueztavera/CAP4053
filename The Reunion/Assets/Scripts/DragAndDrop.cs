using UnityEngine;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    public GameObject SelectedPiece; 
    int OIL = 1; 
    private int totalPieces = 36;
    private int correctlyPlacedPieces = 0;

    [Header("Completion Settings")]
    public string puzzleID = "note"; // Should match PuzzleTrigger's puzzleID
    public float returnDelay = 1.5f; // Delay before returning to map

    [Header("Clue Settings")]
    public string clueID = "Note"; // The ID of the clue to be added

    void Start()
    {
        Debug.Log($"Total puzzle pieces: {totalPieces}");
    }

    // Called by PiecesScript when a piece snaps into place
    public void PieceSnappedIntoPlace()
    {
        correctlyPlacedPieces++;
        Debug.Log($"Piece snapped! Correctly placed: {correctlyPlacedPieces}/{totalPieces}");
        CheckPuzzleCompletion();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.transform.CompareTag("Puzzle"))
            {
                if (!hit.transform.GetComponent<PiecesScript>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    SelectedPiece.GetComponent<PiecesScript>().Selected = true;   
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<PiecesScript>().Selected = false;
                SelectedPiece = null;  
            }     
        }

        if (SelectedPiece != null) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
            SelectedPiece.transform.position = new Vector3(mousePos.x, mousePos.y, 0); 
        }
    }

    private void CheckPuzzleCompletion()
    {
        if (correctlyPlacedPieces >= totalPieces)
        {
            Debug.Log("Puzzle completed!");
            
            SaveSystem.MarkPuzzleComplete(clueID); 
            ClueEventManager.PuzzleCompleted(clueID); 

            Clue clueFromDB = ClueDatabase.Instance.GetClueByName(clueID);
            if (clueFromDB != null)
            {
                InventoryManager.Instance.AddClue(clueFromDB);
                Debug.Log($"Clue '{clueID}' added from database.");
            }
            else 
            {
                Debug.LogError($"Clue '{clueID}' NOT found in database!");
            }

            PlayerPrefs.Save();
            Debug.Log($"Puzzle completed. Returning to map...");
            Invoke("ReturnToMap", returnDelay);
            
        }
    }

    private void ReturnToMap()
    {
        PuzzleSceneSwapper.Instance.ReturnToMap();
    }
}