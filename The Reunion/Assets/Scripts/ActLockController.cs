using System.Collections.Generic;
using UnityEngine;

public class ActLockController : MonoBehaviour
{
    public int requiredAct; // Set in Inspector (2 or 3)
    private static List<ActLockController> allLockControllers = new List<ActLockController>;
    private bool isLocked = true;

    void Awake()
    {
        // Register self to static list
        if (!allLockControllers.Contains(this))
        {
            allLockControllers.Add(this);
        }
    }

    void OnDestroy()
    {
        // Unregister when destroyed
        allLockControllers.Remove(this);
    }

    public static void UpdateAllLocks(int currentAct)
    {
        foreach (ActLockController lockController in allLockControllers)
        {
            lockController.UpdateLockState(currentAct);
        }
    }

    public void UpdateLockState(int currentAct)
    {
        // For prototype: Act 2 unlocks at 1 clue (act=2), Act 3 at 2 clues (act=3)
        gameObject.SetActive(currentAct < requiredAct);
    }
}