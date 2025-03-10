// DoorLockController.cs
using UnityEngine;
using System.Collections.Generic;

public class DoorLockController : MonoBehaviour
{
    [Header("Act Settings")]
    public int requiredAct = 2; // Set to 2 for Act 2 doors, 3 for Act 3 doors

    private static List<DoorLockController> allDoorLocks = new List<DoorLockController>();

    void Start()
    {
        // Initialize state based on current act
        //UpdateLockState(SuspicionManager.Instance.currentAct);
    }

    void Awake() => allDoorLocks.Add(this);
    void OnDestroy() => allDoorLocks.Remove(this);


    public void UpdateLockState(int currentAct)
    {
        // Disable entire padlock object when act requirement met
        gameObject.SetActive(currentAct < requiredAct);
    }

    public static void UpdateAllLocks(int currentAct)
    {
        DoorLockController[] allLocks = FindObjectsByType<DoorLockController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DoorLockController lockObj in allLocks)
        {
            lockObj.UpdateLockState(currentAct);
        }
    }
}