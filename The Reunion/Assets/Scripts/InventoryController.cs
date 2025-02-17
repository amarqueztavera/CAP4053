using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryUI;
    //public Button[] inventorySlots; // Assign slots manually in Unity
    private bool isInventoryOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {     
        isInventoryOpen = !isInventoryOpen;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(isInventoryOpen);
        }

        // pause game when inventory is open, resume when closed
        Time.timeScale = isInventoryOpen ? 0f : 1f;

    }
}
