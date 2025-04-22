using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))] // Forces collider component
public class PuzzleTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string puzzleSceneName;
    public string puzzleID; // Unique identifier for this puzzle

    [Header("Visual Feedback")]
    public Color incompleteColor = Color.yellow; // Default color (yellow)
    public Color completeColor = Color.green;    // Color when puzzle is done

    [Header("References")]
    private SpriteRenderer spriteRenderer;
    private Collider2D interactionCollider;


    void OnEnable()
    {
        ClueEventManager.OnPuzzleCompleted += HandlePuzzleCompleted;
        RefreshPuzzleState();
    }

    void OnDisable()
    {
        ClueEventManager.OnPuzzleCompleted -= HandlePuzzleCompleted;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactionCollider = GetComponent<Collider2D>();
        RefreshPuzzleState();

        //Debug.Log($"PuzzleTrigger ({puzzleID}) initialized. Completed: {SaveSystem.IsPuzzleComplete(puzzleID)}");
    }

    public void RefreshPuzzleState()
    {
        if (SaveSystem.IsPuzzleComplete(puzzleID))
        {
            MarkAsComplete();

            //Debug.Log($"Puzzle {puzzleID} is already completed. Disabling trigger.");
            spriteRenderer.color = completeColor;
        }
        else
        {
            spriteRenderer.color = incompleteColor;
            //interactionCollider.enabled = true;


            // Do NOT enable the collider here
            // Leave enabling to RoomBoundaryTrigger

            //Debug.Log($"PuzzleTrigger ({puzzleID}) set to incomplete (yellow).");
        }
    }

    void Update()
    {
        // Early exit if the puzzle is already completed
        if (SaveSystem.IsPuzzleComplete(puzzleID)) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Objects");

            // Use a larger distance to properly hit interactables
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                Debug.Log("Hit layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                Debug.Log($"Clicked {name}! puzzle scene: {puzzleSceneName}");

                if (hit.collider.gameObject == gameObject && !string.IsNullOrEmpty(puzzleSceneName))
                {
                    Debug.Log($"Clicked {name}! Loading {puzzleSceneName}");
                    PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
                }
            }
            else
            {
                //Debug.Log("No collider hit.");
            }
        }
    }

    /*void OnMouseDown()
    {
        if (SaveSystem.IsPuzzleComplete(puzzleID)) return;
        if (string.IsNullOrEmpty(puzzleSceneName)) return;

        Debug.Log($"Loading puzzle: {puzzleSceneName}");
        TryLoadPuzzle();
    }*/
    void OnMouseDown()
{
    //if (SaveSystem.IsPuzzleComplete(puzzleID)) return;
    if (string.IsNullOrEmpty(puzzleSceneName)) return;
    Debug.Log($"Loading puzzle: {puzzleSceneName}");
    TryLoadPuzzle();
}

    private void TryLoadPuzzle()
    {
        if (PuzzleSceneSwapper.Instance == null)
        {
            Debug.LogError("PuzzleSceneSwapper instance missing!");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(puzzleSceneName))
        {
            Debug.LogError($"Scene {puzzleSceneName} not in build settings!");
            return;
        }

        PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
    }

    // Call this when the puzzle is completed
    public void MarkAsComplete()
    {
        spriteRenderer.color = completeColor; // Change to green
        interactionCollider.enabled = false; // Disable interaction
        Debug.Log($"Puzzle {puzzleID} marked as complete (visual feedback applied).");
    }

    private void HandlePuzzleCompleted(string completedPuzzleID)
    {
        Debug.Log($"PuzzleTrigger ({puzzleID}) received event for puzzle {completedPuzzleID}.");

        if (completedPuzzleID == puzzleID)
        {
            MarkAsComplete();
        }
        else
        {
            Debug.Log($"PuzzleTrigger ({puzzleID}) ignored event for '{completedPuzzleID}'.");
        }
    }

}