using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))] // Forces collider component
public class PuzzleTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string puzzleSceneName;
    public string puzzleID; // Unique identifier for this puzzle

    [Header("References")]
    private Collider2D interactionCollider;

    void Start()
    {
        interactionCollider = GetComponent<Collider2D>();

        // Disable interaction if the puzzle is already completed
        if (SaveSystem.IsPuzzleComplete(puzzleID))
        {
            interactionCollider.enabled = false;
            Debug.Log($"Puzzle {puzzleID} is already completed. Disabling trigger.");
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

                if (hit.collider.gameObject == gameObject && !string.IsNullOrEmpty(puzzleSceneName))
                {
                    Debug.Log($"Clicked {name}! Loading {puzzleSceneName}");
                    PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
                }
            }
            else
            {
                Debug.Log("No collider hit.");
            }
        }
    }

    void OnMouseDown()
    {
        // Early exit if the puzzle is already completed
        if (SaveSystem.IsPuzzleComplete(puzzleID)) return;

        if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
        {
            Debug.LogError("Add PhysicsRaycaster to Main Camera!");
            return;
        }

        if (!string.IsNullOrEmpty(puzzleSceneName))
        {
            Debug.Log($"Attempting to load {puzzleSceneName}");
            PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
        }
    }
}