using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PillManager : MonoBehaviour
{
    [Header("Clue Settings")]
    public Clue clueToAdd; // Assign in Inspector

    public static PillManager Instance;
    public GameObject winText;

    private Dictionary<string, int> placedPills = new Dictionary<string, int>(); // Tracks placed pills by color
    private int totalPillsPerColor = 3; // Number of pills per color
    private int totalColors = 3; // Number of different pill colors

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPill(GameObject pill, string pillColor)
    {
        if (!placedPills.ContainsKey(pillColor))
        {
            placedPills[pillColor] = 0; // Initialize count for this color
        }

        placedPills[pillColor]++; // Increment count for this color

        if (AllPillsSorted())
        {
            ShowWinMessage();
        }
    }

    private bool AllPillsSorted()
    {
        // If there are not enough colors sorted, return false
        if (placedPills.Count < totalColors)
        {
            return false;
        }

        // Check that every color has the required number of pills sorted
        foreach (var pillCount in placedPills.Values)
        {
            if (pillCount < totalPillsPerColor)
            {
                return false;
            }
        }

        Debug.Log("Load back into game");
        PuzzleSceneSwapper.Instance.ReturnToMap();

        // Add clue and return to game
        Debug.Log("Clue Added!");
        InventoryManager.Instance.AddClue(clueToAdd);

        return true; // All colors have been sorted correctly
    }

    public void ShowWinMessage()
    {
        if (winText != null)
        {
            winText.SetActive(true);
        }
        Debug.Log("You Win!");
    }
}
