using UnityEngine;
using UnityEngine.UI;

public class ClueUnlocker : MonoBehaviour
{
    [Header("Clue Settings")]
    public Clue clueToAdd; // Assign this in the Inspector

    [Header("UI Settings")]
    public Button clueButton; // Assign the button in Inspector

    void Start()
    {
        if (clueButton != null)
        {
            clueButton.onClick.AddListener(UnlockClueAndExit);
        }
        else
        {
            Debug.LogError("Clue Button is not assigned in the Inspector!");
        }
    }

    public void UnlockClueAndExit()
    {
        if (clueToAdd != null)
        {
            Debug.Log("Clue Added!");
            InventoryManager.Instance.AddClue(clueToAdd);
        }
        else
        {
            Debug.LogError("No Clue assigned to ClueUnlocker!");
        }

        if (PuzzleSceneSwapper.Instance != null)
        {
            Debug.Log("Exiting scene and returning to main game.");
            PuzzleSceneSwapper.Instance.ReturnToMap();
        }
        else
        {
            Debug.LogError("PuzzleSceneSwapper.Instance is NULL! Ensure PuzzleSceneSwapper exists in the scene.");
        }
    }
}

