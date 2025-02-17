using UnityEngine;

public class SuspicionTester : MonoBehaviour
{
    public SuspicionManager suspicionManager;

    void Update()
    {
        // Simulate suspicion increase with a key press
        if (Input.GetKeyDown(KeyCode.I))
        {
            suspicionManager.IncreaseSuspicion(10f); // Increase suspicion by 10%
        }

        // Simulate suspicion decrease with a key press
        if (Input.GetKeyDown(KeyCode.O))
        {
            suspicionManager.IncreaseSuspicion(-10f); // Decrease suspicion by 10%
        }

        // Simulate act progression with a key press
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            suspicionManager.SetAct(1); // Switch to Act 1
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            suspicionManager.SetAct(2); // Switch to Act 2
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            suspicionManager.SetAct(3); // Switch to Act 3
        }
    }
}