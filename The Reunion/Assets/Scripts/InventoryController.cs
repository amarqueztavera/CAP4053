using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject hotbarUI;
    private bool isInventoryOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
        if (hotbarUI != null)
        {
            hotbarUI.SetActive(true);
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
        if (hotbarUI != null) 
        { 
            hotbarUI.SetActive(!isInventoryOpen);
        }

        // pause game when inventory is open, resume when closed
        Time.timeScale = isInventoryOpen ? 0f : 1f;

    }
}
