using UnityEngine;

public class DebugReset : MonoBehaviour
{
    void Update()
    {
        // Press R to reset all puzzle progress
        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveSystem.ResetAllPuzzleProgress();
            Debug.Log("All puzzle progress reset!");

            // Optional: Reload the scene to see changes immediately
             //UnityEngine.SceneManagement.SceneManager.LoadScene(
             //    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}