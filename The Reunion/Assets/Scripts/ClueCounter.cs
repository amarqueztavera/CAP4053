using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueCounter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int cluesRequiredForEnding = 10; // Set to 10 for final game
    public string endingSceneName = "End Scene"; // Name of ending scene

    [Header("UI")]
    [SerializeField] private TMP_Text cluesText;

    // Singleton pattern for easy access
    public static ClueCounter Instance { get; private set; }
    private const string CLUE_COUNT_KEY = "PlayerClueCount";
    private const string CLUES_REQUIRED_KEY = "CluesRequired";
    private const string CURRENT_ACT_KEY = "CurrentAct";


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

            // Initialize with default values for new game
            if (!PlayerPrefs.HasKey(CLUE_COUNT_KEY))
            {
                PlayerPrefs.SetInt(CLUE_COUNT_KEY, 0);
                PlayerPrefs.SetInt(CURRENT_ACT_KEY, 1); // Default to Act 1 for new games
                PlayerPrefs.Save();
            }

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
        cluesRequiredForEnding = PlayerPrefs.GetInt(CLUES_REQUIRED_KEY, 10);
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
        // New game starts at Act 1 (0�1 clues)
        // 3 clues = Act 2, 7 clues = Act 3 (TEMPORARY FOR TESTING)
        if (ClueCount >= 7)
            return 3;
        else if (ClueCount >= 3)
            return 2;
        else
            return 1;
    }

    private void InitializeActProgression()
    {
        // Always start at Act 1 for new games, use saved value otherwise
        int startingAct = PlayerPrefs.GetInt(CURRENT_ACT_KEY, 1); // Default to 1 if not found

        SuspicionManager.Instance?.SetAct(startingAct);
        DoorLockController.UpdateAllLocks(startingAct);
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
            int currentAct = CalculateCurrentAct(); // Get the current act for display
            cluesText.text = $"Current Act: {currentAct}/3 \n{ClueCount}/{cluesRequiredForEnding} Clues Found";
        }
        else
        {
            Debug.LogWarning("Clues Text reference is missing!");
        }
    }

    // For debugging purposes
    [ContextMenu("Reset Progression")]
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteKey(CLUE_COUNT_KEY);
        PlayerPrefs.DeleteKey(CURRENT_ACT_KEY);
        PlayerPrefs.DeleteKey(CLUES_REQUIRED_KEY);

        ClueCount = 0;
        PlayerPrefs.SetInt(CURRENT_ACT_KEY, 1); // Reset to Act 1
        PlayerPrefs.Save();

        CheckActProgression(); // This will update to Act 1
    }
}
