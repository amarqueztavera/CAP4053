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
    //private void UpdateInventoryUI()
    //{
    //    for (int i = 0; i < inventorySlots.Length; i++)
    //    {
    //        if (i < clues.Count)
    //        {
    //            // Set the icon of the slot to the clue's icon
    //            inventorySlots[i].GetComponent<Image>().sprite = clues[i].icon;
    //            inventorySlots[i].GetComponent<Image>().color = Color.white; // Make the slot visible

    //            // Add EventTrigger to the slot for tooltip functionality
    //            AddEventTrigger(inventorySlots[i], i);
    //        }
    //        else
    //        {
    //            // Clear the slot if it's empty
    //            inventorySlots[i].GetComponent<Image>().sprite = null;
    //            inventorySlots[i].GetComponent<Image>().color = Color.clear; // Make the slot transparent

    //            // Remove EventTrigger if the slot is empty
    //            RemoveEventTrigger(inventorySlots[i]);
    //        }
    //    }
    //}

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < clues.Count)
            {
                Image slotImage = inventorySlots[i].GetComponent<Image>();
                slotImage.sprite = clues[i].icon;
                slotImage.color = Color.white;
                AddEventTrigger(inventorySlots[i], i);
            }
            else
            {
                Image slotImage = inventorySlots[i].GetComponent<Image>();
                slotImage.sprite = null;
                slotImage.color = Color.clear;
                RemoveEventTrigger(inventorySlots[i]);
            }
        }
    }

    //// Remove EventTrigger from a slot
    //private void RemoveEventTrigger(GameObject slot)
    //{
    //    EventTrigger trigger = slot.GetComponent<EventTrigger>();
    //    if (trigger != null)
    //    {
    //        trigger.triggers.Clear(); // Clear all triggers
    //    }
    //}

    ////Add EventTrigger to a slot
    //private void AddEventTrigger(GameObject slot, int index)
    //{
    //    EventTrigger trigger = slot.GetComponent<EventTrigger>();
    //    if (trigger == null)
    //    {
    //        trigger = slot.AddComponent<EventTrigger>();
    //    }
    //    else
    //    {
    //        trigger.triggers.Clear(); // avoids duplicates
    //    }

    //    // Create a new entry for the PointerEnter event
    //    EventTrigger.Entry entryEnter = new EventTrigger.Entry
    //    {
    //        eventID = EventTriggerType.PointerEnter
    //    };
    //    entryEnter.callback.AddListener((data) => { OnPointerEnterSlot(index); });
    //    trigger.triggers.Add(entryEnter);

    //    // Create a new entry for the PointerExit event
    //    EventTrigger.Entry entryExit = new EventTrigger.Entry
    //    {
    //        eventID = EventTriggerType.PointerExit
    //    };
    //    entryExit.callback.AddListener((data) => { OnPointerExitSlot(); });
    //    trigger.triggers.Add(entryExit);
    //}

    private void RemoveEventTrigger(GameObject slot)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            trigger.triggers.Clear();
        }
    }

    private void AddEventTrigger(GameObject slot, int index)
    {
        EventTrigger trigger = slot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slot.AddComponent<EventTrigger>();
        }
        else
        {
            trigger.triggers.Clear();
        }

        EventTrigger.Entry clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        clickEntry.callback.AddListener((data) => { OnClueClicked(index); });
        trigger.triggers.Add(clickEntry);
    }

    private void OnClueClicked(int index)
    {
        if (index < clues.Count)
        {
            Clue clickedClue = clues[index];
            ClueDetailPanel.Instance.ShowClueDetails(clickedClue.icon, clickedClue.clueName, clickedClue.description);
        }
    }
}