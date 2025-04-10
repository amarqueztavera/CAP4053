using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueCounter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int cluesRequiredForEnding = 3; // Set to 9 for final game
    public string endingSceneName = "Thank_you_Scene"; // Name of ending scene

    [Header("UI")]
    [SerializeField] private TMP_Text cluesText;

    // Singleton pattern for easy access
    public static ClueCounter Instance { get; private set; }
    private const string CLUE_COUNT_KEY = "PlayerClueCount";
    private const string CLUES_REQUIRED_KEY = "CluesRequired";


    public SuspicionManager suspicionManager;
    private int _clueCount;

    public int ClueCount
    {
        get => _clueCount;
        private set
        {
            _clueCount = value;
            UpdateClueDisplay();
            CheckActProgression();
            CheckForEnding();
        }
    }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved data on startup
            LoadClueCount(); 
            InitializeActProgression();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        suspicionManager = GetComponent<SuspicionManager>();
        if (suspicionManager == null)
        {
            Debug.LogError("SuspicionManager not found on the same GameObject!");
        }
    }

    private void LoadClueCount()
    {
        _clueCount = PlayerPrefs.GetInt(CLUE_COUNT_KEY, 0);
        cluesRequiredForEnding = PlayerPrefs.GetInt(CLUES_REQUIRED_KEY, 3);
        UpdateClueDisplay();
    }

    public void AddClue()
    {
        ClueCount = Mathf.Clamp(ClueCount + 1, 0, cluesRequiredForEnding);
        PlayerPrefs.SetInt(CLUE_COUNT_KEY, ClueCount);
        PlayerPrefs.Save();
    }

    private void CheckActProgression()
    {
        //// Prototype progression: 1 clue = Act 2, 2 clues = Act 3
        //int currentAct = Mathf.Clamp(ClueCount + 1, 1, 3);

        //if (SuspicionManager.Instance != null)
        //{
        //    SuspicionManager.Instance.SetAct(currentAct);
        //}
        //else
        //{
        //    Debug.LogError("SuspicionManager instance not found!");
        //}

        //DoorLockController.UpdateAllLocks(currentAct);

        // Get the current clue count (will use saved data automatically)
        int currentAct = CalculateCurrentAct();

        if (SuspicionManager.Instance != null)
        {
            SuspicionManager.Instance.SetAct(currentAct);
            PlayerPrefs.SetInt("CurrentAct", currentAct); // Save current act
        }
        else
        {
            Debug.LogError("SuspicionManager instance not found!");
        }

        DoorLockController.UpdateAllLocks(currentAct);
    }

    private int CalculateCurrentAct()
    {
        // Use the saved clue count to determine act
        int savedClues = PlayerPrefs.GetInt(CLUE_COUNT_KEY, 0);

        // Your progression logic (1 clue = Act 2, 2 clues = Act 3, etc.)
        return Mathf.Clamp(savedClues + 1, 1, 3);
    }

    private void InitializeActProgression()
    {
        // Load saved act progression when game starts
        if (PlayerPrefs.HasKey("CurrentAct"))
        {
            int savedAct = PlayerPrefs.GetInt("CurrentAct");
            SuspicionManager.Instance?.SetAct(savedAct);
            DoorLockController.UpdateAllLocks(savedAct);
        }
        else
        {
            // Initialize with current clue count if no save exists
            CheckActProgression();
        }
    }

    private void CheckForEnding()
    {
        if (ClueCount >= cluesRequiredForEnding)
        {
            LoadEndingScene();
        }
    }

    private void LoadEndingScene()
    {
        SceneManager.LoadScene(endingSceneName, LoadSceneMode.Single);

        // Clear saved data when ending is reached
        PlayerPrefs.DeleteKey(CLUE_COUNT_KEY);
        PlayerPrefs.DeleteKey(CLUES_REQUIRED_KEY);

        // Cleanup persistent objects if needed
        if (SuspicionManager.Instance != null)
            Destroy(SuspicionManager.Instance.gameObject);

        Destroy(gameObject); // Destroy the clue counter
    }

    private void UpdateClueDisplay()
    {
        if (cluesText != null)
        {
            cluesText.text = $"{ClueCount}/{cluesRequiredForEnding} Clues Found";
        }
        else
        {
            Debug.LogWarning("Clues Text reference is missing!");
        }
    }

    // For debugging purposes
    [ContextMenu("Reset Clue Counter")]
    private void ResetCounter()
    {
        PlayerPrefs.DeleteKey(CLUE_COUNT_KEY);
        ClueCount = 0;
        UpdateClueDisplay();
    }
}
