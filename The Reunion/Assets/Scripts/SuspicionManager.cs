using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Runtime.CompilerServices;

public class SuspicionManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider suspicionMeter;
    public TMP_Text suspicionText;

    [Header("Suspicion Settings")]
    [Tooltip("Base suspicion increase per second")]
    public float baseSuspicionRate = 0.1f;
    public float suspicionDecreaseRate = 2f; // Rate when in reunion area
    public float currentSuspicion = 0f;

    [Header("Act Settings")]
    public int currentAct = 1; // 1, 2, or 3
    public float[] actMultipliers = { 1f, 1.5f, 2f }; // Multipliers for acts 1, 2, 3

    private bool isInReunionArea = false;
    private Coroutine suspicionCoroutine;

    void Start()
    {
        suspicionMeter.maxValue = 100;
        suspicionCoroutine = StartCoroutine(UpdateSuspicion());
    }

    IEnumerator UpdateSuspicion()
    {
        while (true)
        {
            float multiplier = actMultipliers[currentAct - 1];
            float rate = baseSuspicionRate * multiplier;

            // Debug logs to track values
            //Debug.Log($"Current Act: {currentAct}");
            //Debug.Log($"Multiplier: {multiplier}");
            //Debug.Log($"Rate: {rate}");

            if (isInReunionArea)
            {
                currentSuspicion = Mathf.Max(0, currentSuspicion - suspicionDecreaseRate * Time.deltaTime);
            }
            else
            {
                currentSuspicion = Mathf.Min(100, currentSuspicion + rate * Time.deltaTime);
            }

            UpdateUI();

            // Check for max suspicion
            if (currentSuspicion >= 100)
            {
                TriggerAlert();
                yield break;
            }

            yield return null;
        }
    }

    void UpdateUI()
    {
        suspicionMeter.value = currentSuspicion;
        suspicionText.text = $"Suspicion: {(int)currentSuspicion}%";
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
    }

    public void SetReunionArea(bool isInArea)
    {
        isInReunionArea = isInArea;
    }

    void TriggerAlert()
    {
        Debug.Log("Suspicion maxed! NPCs are alerted.");
        // Add NPC alert logic here (e.g., trigger AI search)
        StopCoroutine(suspicionCoroutine);
    }
}