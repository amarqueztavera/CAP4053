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

    // Save the current inventory clues to PlayerPrefs
    public void SaveInventoryToPrefs()
    {
        List<string> clueIDs = new List<string>();
        foreach (Clue clue in clues)
        {
            clueIDs.Add(clue.clueName); // assuming clueName is unique ID
        }
        SaveSystem.SaveInventory(clueIDs);
    }

    // Load the inventory from PlayerPrefs
    public void LoadInventoryFromSave()
    {
        List<string> clueIDs = SaveSystem.LoadInventory();
        clues.Clear();

        foreach (string clueID in clueIDs)
        {
            Clue loadedClue = ClueDatabase.Instance.GetClueByName(clueID);
            if (loadedClue != null)
            {
                clues.Add(loadedClue);
            }
            else
            {
                Debug.LogWarning($"Clue '{clueID}' not found in ClueDatabase!");
            }
        }

        UpdateInventoryUI();
    }

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
        LoadInventoryFromSave();
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
        clueCounter.AddClue(); // Update clue counter
        SaveInventoryToPrefs(); // Save after adding clue
        Destroy(clue.gameObject); // Remove the clue from the game world
        return true;
    }

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