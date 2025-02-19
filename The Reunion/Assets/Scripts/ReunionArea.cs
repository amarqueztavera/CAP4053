using UnityEngine;

public class ReunionArea : MonoBehaviour
{
    public SuspicionManager suspicionManager;

    // When player enters reunion area, suspicion decreases

    void Start()
    {
        Debug.Log("ReunionArea script is active!"); // Add this line
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Entered Reunion Area: {other.name}");
        if (other.CompareTag("Player"))
        {
            suspicionManager.SetReunionArea(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Exited Reunion Area: {other.name}");
        if (other.CompareTag("Player"))
        {
            suspicionManager.SetReunionArea(false);
        }
    }
}