using UnityEngine;


// In any script, you can access and modify the flags like this:
// NPCStateManager.Instance.act1 = true;  // Door 1 is now accessible
// NPCStateManager.Instance.act2 = false; // Door 2 is no longer accessible


public class NPCStateManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static NPCStateManager Instance;

    // Flags to represent the accessibility of doors
    public bool act1 = true;
    public bool act2 = false;
    public bool act3 = false;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }
}