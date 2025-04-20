using UnityEngine;
using UnityEngine.UI;

public class RoomBoundaryTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public GameObject[] roomInteractables; // Assign puzzles/clues in this room


    private bool playerInRoom = false;
    private Collider2D roomCollider;
    private GameObject player;

    private void Start()
    {
        roomCollider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag(playerTag);

        // Ensure all interactables' colliders are disabled when the scene starts
        SetInteractablesState(false);

        // Check if player is already inside the room on Start
        if (player != null && roomCollider.bounds.Contains(player.transform.position))
        {
            Debug.Log("Player was already inside room at Start()");
            playerInRoom = true;
            SetInteractablesState(true);
        }
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
            if (obj == null) continue;

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