using System;
using UnityEngine;

public class ClueEventManager : MonoBehaviour
{

    public static event Action<string> OnPuzzleCompleted;

    public static void PuzzleCompleted(string puzzleID)
    {
        OnPuzzleCompleted?.Invoke(puzzleID);
        Debug.Log($"EventManager: Puzzle completed event triggered for puzzle '{puzzleID}'.");
    }
}
