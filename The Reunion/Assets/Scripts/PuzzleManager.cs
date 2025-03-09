using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleManager : MonoBehaviour
{
    [Header("Completion Settings")]
    public Clue clueToAdd; // Assign in Inspector
    public string completionEvent = "PuzzleComplete"; // Match your puzzle logic

    void Start()
    {
        // Hook into your puzzle completion system
        //EventSystem.Subscribe(completionEvent, OnPuzzleSolved);
    }

    void OnPuzzleSolved()
    {
        // Add clue and return to map
        InventoryManager.Instance.AddClue(clueToAdd);
        PuzzleSceneSwapper.Instance.LoadMap();
    }
}