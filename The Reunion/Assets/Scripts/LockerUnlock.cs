using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockerUnlock : MonoBehaviour
{
    [Header("UI References")]
    public InputField inputField1;
    public InputField inputField2;
    public InputField inputField3;
    public Text resultText;
    public GameObject locker; // Reference to the locker
    public string puzzleID; // Unique ID

    [Header("Clue Settings")]
    public Clue clueToAdd; // Assign in Inspector

    private string correctCode1 = "08";
    private string correctCode2 = "09";
    private string correctCode3 = "11";

    public void CheckCode()
    {
        Debug.Log("button clicked");
        // Normalize input by removing leading zeros
        string enteredCode1 = inputField1.text.TrimStart('0');
        string enteredCode2 = inputField2.text.TrimStart('0');
        string enteredCode3 = inputField3.text.TrimStart('0');

        // Normalize correct codes as well
        string normalizedCode1 = correctCode1.TrimStart('0');
        string normalizedCode2 = correctCode2.TrimStart('0');
        string normalizedCode3 = correctCode3.TrimStart('0');

        // Compare normalized values
        if (enteredCode1 == normalizedCode1 && enteredCode2 == normalizedCode2 && enteredCode3 == normalizedCode3)
        {
            resultText.text = "Locker Unlocked!";
            resultText.color = Color.green;
            StartCoroutine(UnlockAndReturn());
            UnlockLocker();
        }
        else
        {
            resultText.text = "Incorrect Code!";
            resultText.color = Color.red;
        }
    }

    private IEnumerator UnlockAndReturn()
    {
        UnlockLocker();
        yield return new WaitForSeconds(1f);
        Debug.Log("Load back into game");
        PuzzleSceneSwapper.Instance.ReturnToMap();

        // Save completion state
        SaveSystem.MarkPuzzleComplete(puzzleID);
    }

    void UnlockLocker()
    {
        locker.SetActive(false); // Hide the locker when unlocked
        Debug.Log("Locker has been unlocked!");

        // Add clue and return to game
        Debug.Log("Clue Added!");
        InventoryManager.Instance.AddClue(clueToAdd);
    }

    // 🔹 New Method for Exiting Without Adding Clue
    public void ExitPuzzleScene()
    {
        Debug.Log("Exiting puzzle scene without solving.");
        PuzzleSceneSwapper.Instance.ReturnToMap(); // Go back without adding the clue
    }
}
