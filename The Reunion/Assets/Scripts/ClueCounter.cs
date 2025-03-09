using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueCounter : MonoBehaviour
{
    [Header("Settings")]
    public string endingSceneName = "End Scene"; // Name of ending scene

    [Header("UI")]
    public TMP_Text cluesText;

    public SuspicionManager suspicionManager;
    private int clueCount = 0;


    private void Start()
    {
        suspicionManager = GetComponent<SuspicionManager>();
        if (suspicionManager == null)
        {
            Debug.LogError("SuspicionManager not found on the same GameObject!");
        }
    }

    // When a clue is collected
    public void AddClue()
    {
        clueCount++;
        UpdateClueDisplay();


        // Calculate current act (1-3) based on clues collected
        //int currentAct = Mathf.Clamp((clueCount-1 / 3) + 1, 1, 3); // 0-2: Act 1, 3-5: Act 2, 6-8: Act 3

        // for prototype act 2 starts after 1 clue, act 3 starts after 2.
        //int currentAct = Mathf.Clamp((clueCount - 1) / 1 + 1, 1, 3); 
        int currentAct = Mathf.Clamp(clueCount + 1, 1, 3);
        suspicionManager.SetAct(currentAct);

        DoorLockController.UpdateAllLocks(currentAct); // Update door locks

        // Check for ending
        if (clueCount >= 9)
        {
            SceneManager.LoadScene(endingSceneName); 
            return;
        }
    }

    private void UpdateClueDisplay()
    {
        cluesText.text = $"{clueCount}/9 Clues Found";
    }
}
