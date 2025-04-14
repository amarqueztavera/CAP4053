using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Runtime.CompilerServices;
using Kinnly;

public class SuspicionManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider suspicionMeter;
    public TMP_Text suspicionText;
    public Image handle;

    [Header("Suspicion Settings")]
    [Tooltip("Base suspicion increase per second")]
    public float baseSuspicionRate = 0.1f;
    public float suspicionDecreaseRate = 2f; // Rate when in reunion area
    public float currentSuspicion = 0f;

    [Header("Act Settings")]
    public int currentAct = 1; // 1, 2, or 3
    public float[] actMultipliers = { 1f, 1.5f, 2f }; // Multipliers for acts 1, 2, 3

    [Header("Reunion Area Settings")]
    [SerializeField] private float reunionAreaCooldown = 2f; // Time after leaving when suspicion won't increase
    private float _lastReunionExitTime;

    private bool _isInReunionArea = false;
    private Coroutine suspicionCoroutine;

    public static SuspicionManager Instance;
    public static event System.Action<int> OnActChanged;

    // getter for checking if player is in reunion area
    public bool IsInReunionArea { get; private set; }

    public bool IsInCooldown
    {
        get
        {
            return Time.time > _lastReunionExitTime + reunionAreaCooldown;
        }
    }


    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep alive between scenes
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        suspicionMeter.maxValue = 100;

        // Make handle non-interactive
        if (handle != null)
        {
            handle.raycastTarget = false;
        }
        else
        {
            Debug.LogWarning("Handle Image reference missing in SuspicionManager");
        }

        Debug.Log("Initializing suspicion manager");
        suspicionCoroutine = StartCoroutine(UpdateSuspicion());

        // Verify coroutine started
        if (suspicionCoroutine == null)
        {
            Debug.LogError("Failed to start suspicion coroutine!");
        }
    }

    private IEnumerator UpdateSuspicion()
    {
        while (true)
        {
            if (!IsInReunionArea) // Only increase when outside reunion area
            {
                if (!NPCStateManager.Instance.maxSuspicion) // Only increase if not in alert state
                {
                    float multiplier = actMultipliers[currentAct - 1];
                    float rate = baseSuspicionRate * multiplier;
                    currentSuspicion = Mathf.Min(100, currentSuspicion + rate * Time.deltaTime);
                    UpdateUI();

                    if (currentSuspicion >= 100)
                    {
                        TriggerAlert();
                        yield break;
                    }
                }
            }
            else // Inside reunion area
            {
                // Keep resetting to 0 while in safe zone
                if (currentSuspicion > 0)
                {
                    currentSuspicion = 0;
                    UpdateUI();
                }
            }
            yield return null;
        }
    }

    void UpdateUI()
    {
        if (suspicionMeter == null)
        {
            Debug.LogError("Suspicion Meter reference is null!");
            return;
        }

        suspicionMeter.value = currentSuspicion;

        if (suspicionText != null)
        {
            suspicionText.text = $"Suspicion: {(int)currentSuspicion}%";
        }
        else
        {
            Debug.LogError("Suspicion Text reference is null!");
        }

        // Visual verification
        Debug.Log($"UI Updated - Value: {suspicionMeter.value}, Text: {suspicionText.text}");


    }

    public void IncreaseSuspicion(float amount)
    {
        currentSuspicion = Mathf.Min(100, currentSuspicion + amount);
        UpdateUI();
    }

    public void SetAct(int act)
    {
        currentAct = Mathf.Clamp(act, 1, 3);
        Debug.Log($"Entered Act: {currentAct}");
        OnActChanged?.Invoke(currentAct);
    }

    public void SetReunionArea(bool isInArea)
    {
        IsInReunionArea = isInArea;

        if (IsInReunionArea)
        {
            // Immediately reset when entering
            currentSuspicion = 0;
            UpdateUI();
            NPCStateManager.Instance.SetMaxSuspicion(false);
        }

        // Always restart coroutine when state changes
        if (suspicionCoroutine != null)
        {
            StopCoroutine(suspicionCoroutine);
        }
        suspicionCoroutine = StartCoroutine(UpdateSuspicion());
    }

    void TriggerAlert()
    {
        Debug.Log("Suspicion maxed! NPCs are alerted.");
        NPCStateManager.Instance.SetMaxSuspicion(true);
        currentSuspicion = 100f;
        UpdateUI();

        // Alert all NPCs
        NPCStateManager.Instance.AlertAllNPCs();

        // Start cooldown coroutine
        StartCoroutine(SuspensionCooldown());
    }

    private IEnumerator SuspensionCooldown()
    {
        yield return new WaitForSeconds(10f); // 10 second alert duration

        if (currentSuspicion < 100f)
        {
            NPCStateManager.Instance.SetMaxSuspicion(false);
        }
    }

    public void ResetSuspicion()
    {
        currentSuspicion = 0f;
        UpdateUI();

        // Restart the coroutine if it's not running
        if (suspicionCoroutine == null)
        {
            suspicionCoroutine = StartCoroutine(UpdateSuspicion());
        }
    }

    public void ResetFromCaught()
    {
        currentSuspicion = 0f;
        _lastReunionExitTime = Time.time; // Start cooldown
        UpdateUI();

        if (suspicionCoroutine == null)
        {
            suspicionCoroutine = StartCoroutine(UpdateSuspicion());
        }
    }
}