using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueCounter : MonoBehaviour
{
    public SuspicionManager suspicionManager;
    public string endingSceneName = "EndingScene"; // Name of ending scene
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

        // Check for ending
        if (clueCount >= 9)
        {
            //SceneManager.LoadScene(endingSceneName); // Update with reall ending scene
            return;
        }

        // Update act based on clue count
        int newAct = Mathf.Clamp((clueCount / 3) + 1, 1, 3); // 0-2: Act 1, 3-5: Act 2, 6-8: Act 3
        suspicionManager.SetAct(newAct);
    }
}