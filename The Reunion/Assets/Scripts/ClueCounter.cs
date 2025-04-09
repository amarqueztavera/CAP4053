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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddClue()
    {
        ClueCount = Mathf.Clamp(ClueCount + 1, 0, cluesRequiredForEnding);
    }

    private void Start()
    {
        suspicionManager = GetComponent<SuspicionManager>();
        if (suspicionManager == null)
        {
            Debug.LogError("SuspicionManager not found on the same GameObject!");
        }
    }

    private void CheckActProgression()
    {
        // Prototype progression: 1 clue = Act 2, 2 clues = Act 3
        int currentAct = Mathf.Clamp(ClueCount + 1, 1, 3);

        if (SuspicionManager.Instance != null)
        {
            SuspicionManager.Instance.SetAct(currentAct);
        }
        else
        {
            Debug.LogError("SuspicionManager instance not found!");
        }

        DoorLockController.UpdateAllLocks(currentAct);
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
        ClueCount = 0;
        UpdateClueDisplay();
    }
}
