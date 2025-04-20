using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SaveSystem
{
    private const string PUZZLE_PREFIX = "Puzzle_";
    private const string CLUE_PREFIX = "Clue_";
    private const string INVENTORY_KEY = "Inventory";

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

    public static void ResetAllPuzzleProgress()
    {
        PlayerPrefs.DeleteAll(); // Clears all saved data
        PlayerPrefs.Save();
        Debug.Log("All puzzle progress reset.");
    }

    // Inventory saving
    public static void SaveInventory(List<string> clueIDs)
    {
        // Joins all clue IDs into one comma-separated string
        string serializedInventory = string.Join(",", clueIDs);
        PlayerPrefs.SetString(INVENTORY_KEY, serializedInventory);
        PlayerPrefs.Save();
        Debug.Log($"Inventory saved: {serializedInventory}");
    }

    public static List<string> LoadInventory()
    {
        string serializedInventory = PlayerPrefs.GetString(INVENTORY_KEY, "");
        return string.IsNullOrEmpty(serializedInventory)
            ? new List<string>()
            : serializedInventory.Split(',').ToList();
    }

    public static void MarkClueCollected(string clueID)
    {
        PlayerPrefs.SetInt(CLUE_PREFIX + clueID, 1);
        PlayerPrefs.Save();
    }

    public static bool IsClueCollected(string clueID)
    {
        return PlayerPrefs.GetInt(CLUE_PREFIX + clueID, 0) == 1;
    }

    public static List<string> AllClueIDs()
    {
        if (ClueDatabase.Instance == null)
        {
            Debug.LogWarning("ClueDatabase not initialized.");
            return new List<string>();
        }

        return ClueDatabase.Instance.allClues.ConvertAll(clue => clue.clueName);
    }

    public static void ResetAllClueProgress()
    {
        foreach (string clueID in AllClueIDs())
        {
            PlayerPrefs.DeleteKey(CLUE_PREFIX + clueID);
        }
        PlayerPrefs.Save();
    }

    public static void ResetInventory()
    {
        PlayerPrefs.DeleteKey(INVENTORY_KEY);
        PlayerPrefs.Save();
        Debug.Log("Inventory reset.");
    }

}