using UnityEngine;

public class ClickableClue : MonoBehaviour
{
    public Clue clue; // Reference to the clue data
    public InventoryManager inventoryManager; // Reference to the inventory manager

    void OnMouseDown()
    {
        Debug.Log("Clue clicked: " + clue.clueName);
        if (inventoryManager.AddClue(clue))
        {
            Debug.Log("Clue added to inventory: " + clue.clueName);
        }
    }
}