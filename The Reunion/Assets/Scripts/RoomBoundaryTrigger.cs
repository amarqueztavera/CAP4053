using UnityEngine;
using UnityEngine.UI;

public class RoomBoundaryTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public GameObject[] roomInteractables; // Assign puzzles/clues in this room
    private bool playerInRoom = false;

    private void Start()
    {
        // Ensure all interactables' colliders are disabled when the scene starts
        SetInteractablesState(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && other.GetComponent<PlayerMovement>() != null) // Check if it's the main player
        {
            Debug.Log("Entered ROom");
            playerInRoom = true;
            SetInteractablesState(true); // Enable when entering the room
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && other.GetComponent<PlayerMovement>() != null)
        {
            Debug.Log("exit ROom");
            playerInRoom = false;
            SetInteractablesState(false); // Disable when exiting the room
        }
    }

    private void SetInteractablesState(bool state)
    {
        foreach (GameObject obj in roomInteractables)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null) collider.enabled = state;

            // Optional: Toggle outline visibility
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null) outline.enabled = state;
        }
    }

    public bool IsPlayerInRoom()
    {
        return playerInRoom;
    }
}