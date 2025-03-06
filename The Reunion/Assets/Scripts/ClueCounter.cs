using UnityEngine;
using UnityEngine.SceneManagement;


public class Clue
{
    public string clueName; // Name of the clue
    public Sprite icon; // Icon to display in the inventory
    public GameObject gameObject; // Reference to the clue in the game world
    [TextArea] // Makes the description field multi-line in the Inspector
    public string description; // Description of the clue
}
public class ClueCounter : MonoBehaviour
{
    public SuspicionManager suspicionManager;
    public string endingSceneName = "End Scene"; // Name of ending scene
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
            SceneManager.LoadScene(endingSceneName); // Update with reall ending scene
            return;
        }

        // Update act based on clue count
        int newAct = Mathf.Clamp((clueCount / 3) + 1, 1, 3); // 0-2: Act 1, 3-5: Act 2, 6-8: Act 3
        suspicionManager.SetAct(newAct);
    }
}
