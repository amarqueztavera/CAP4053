using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<Clue> clues = new List<Clue>(); // List to store collected clues
    public GameObject[] inventorySlots; // Array of UI slots (2x5 grid)
    public int maxSlots = 10; // Maximum number of slots

    public GameObject tooltipUI; // Reference to the tooltip UI
    public TMP_Text tooltipText; // Reference to the tooltip text component

    private Coroutine tooltipCoroutine;
    public ClueCounter clueCounter;

    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Hide the tooltip at the start
        //if (tooltipUI != null)
        //{
        //    tooltipUI.SetActive(false);
        //}
    }

    // Add a clue to the inventory
    public bool AddClue(Clue clue)
    {
        // clue is getting added!!
        Debug.Log("clue added");

        if (clues.Count >= maxSlots)
        {
            Debug.Log("Inventory is full!");
            return false; 
        }

        // Check if the clue is already in the inventory
        if (clues.Contains(clue))
        {
            Debug.Log("Clue already in inventory: " + clue.clueName);
            return false;
        }

        clues.Add(clue); // Add the clue to the list
        UpdateInventoryUI(); // Update the inventory UI
        Destroy(clue.gameObject); // Remove the clue from the game world
        clueCounter.AddClue(); // Update clue counter
        return true;
    }

    // Update the inventory UI
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < clues.Count)
            {
                // Set the icon of the slot to the clue's icon
                inventorySlots[i].GetComponent<Image>().sprite = clues[i].icon;
                inventorySlots[i].GetComponent<Image>().color = Color.white; // Make the slot visible

                // Add EventTrigger to the slot for tooltip functionality
                AddEventTrigger(inventorySlots[i], i);
            }
            else
            {
                // Clear the slot if it's empty
                inventorySlots[i].GetComponent<Image>().sprite = null;
                inventorySlots[i].GetComponent<Image>().color = Color.clear; // Make the slot transparent

                // Remove EventTrigger if the slot is empty
                RemoveEventTrigger(inventorySlots[i]);
            }
        }
    }

    // Remove EventTrigger from a slot
    private void RemoveEventTrigger(GameObject slot)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            trigger.triggers.Clear(); // Clear all triggers
        }
    }

    //Add EventTrigger to a slot
    private void AddEventTrigger(GameObject slot, int index)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slot.AddComponent<EventTrigger>();
        }
        else
        {
            trigger.triggers.Clear(); // avoids duplicates
        }

        // Create a new entry for the PointerEnter event
        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((data) => { OnPointerEnterSlot(index); });
        trigger.triggers.Add(entryEnter);

        // Create a new entry for the PointerExit event
        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((data) => { OnPointerExitSlot(); });
        trigger.triggers.Add(entryExit);
    }

    //Called when the pointer enters a slot
    // Tooltip is bugging and flashes when hovering over item in inventory
    private void OnPointerEnterSlot(int index)
    {
        if (index < clues.Count && tooltipUI != null && tooltipText != null)
        {
            tooltipText.text = clues[index].description;

            // Position the tooltip to the right of the slot
            RectTransform slotRect = inventorySlots[index].GetComponent<RectTransform>();
            Vector2 slotPosition = slotRect.position;
            float slotWidth = slotRect.rect.width;

            tooltipUI.transform.position = new Vector2(
                slotPosition.x + slotWidth + 10f, // Offset to the right
                slotPosition.y
            );

            tooltipUI.SetActive(true);
        }
    }

    private void OnPointerExitSlot()
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(false);
        }
    }
}