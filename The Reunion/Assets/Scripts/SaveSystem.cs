using UnityEngine;

public static class SaveSystem
{
    private const string PUZZLE_PREFIX = "Puzzle_";

    // Save a puzzle as completed
    public static void MarkPuzzleComplete(string puzzleID)
    {
        PlayerPrefs.SetInt(PUZZLE_PREFIX + puzzleID, 1);
        PlayerPrefs.Save();
    }

    // Check if a puzzle is completed
    public static bool IsPuzzleComplete(string puzzleID)
    {
        return PlayerPrefs.GetInt(PUZZLE_PREFIX + puzzleID, 0) == 1;
    }
}