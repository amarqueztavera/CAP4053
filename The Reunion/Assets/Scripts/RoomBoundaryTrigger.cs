using UnityEngine;
using UnityEngine.UI;

public class RoomBoundaryTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public GameObject[] roomInteractables; // Assign puzzles/clues in this room

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Entered ROom");
            SetInteractablesState(true); // Enable when entering the room
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("exit ROom");
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

    //private void SetInteractablesState(bool state)
    //{
    //    foreach (GameObject obj in roomInteractables)
    //    {
    //        // Enable the NON-TRIGGER collider for clicks
    //        Collider2D clickCollider = obj.GetComponent<Collider2D>();
    //        if (clickCollider != null && !clickCollider.isTrigger)
    //        {
    //            Debug.Log("click collider enabled");
    //            clickCollider.enabled = state;
    //        }

    //        //// Optional: Toggle outline visibility
    //        //Outline outline = obj.GetComponent<Outline>();
    //        //if (outline != null) outline.enabled = state;
    //    }
    //}
}