using UnityEngine;

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
        // Ensure there is only one instance of NPCStateManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: Keep this object between scenes
        }
        else
        {
            Destroy(gameObject);  // If another instance exists, destroy this one
        }
    }
}