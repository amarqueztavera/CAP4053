using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Critical for IEnumerator

public class FullscreenTextPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dimmedBackground;
    [SerializeField] private GameObject fullscreenPanel;
    [SerializeField] private TMP_Text fullscreenText;
    [SerializeField] private Button closeButton;
    
    [SerializeField] private TMP_Text smallText;
    
    [Header("Settings")]
    [SerializeField] private Color dimColor = new Color(0, 0, 0, 0.7f);
    [SerializeField] private float animationDuration = 0.3f;

    private void Awake()
    {
        dimmedBackground.SetActive(false);
        fullscreenPanel.SetActive(false);
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void TogglePopup()
    {
        if (fullscreenPanel.activeSelf) ClosePopup();
        else OpenPopup();
    }

    public void OpenPopup()
    {
        // Activate objects FIRST
        dimmedBackground.SetActive(true);
        fullscreenPanel.SetActive(true);
        
        // THEN start animation
        fullscreenText.text = smallText.text;
        StartCoroutine(AnimatePopup(true));
    }

    public void ClosePopup()
    {
        StartCoroutine(AnimatePopup(false));
    }

    // Add this coroutine method
    private IEnumerator AnimatePopup(bool show)
    {
        dimmedBackground.SetActive(true);
        fullscreenPanel.SetActive(true);
        
        float elapsed = 0f;
        Color startColor = show ? Color.clear : dimColor;
        Color endColor = show ? dimColor : Color.clear;
        Vector3 startScale = show ? Vector3.one * 0.8f : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.one * 0.8f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            
            dimmedBackground.GetComponent<Image>().color = Color.Lerp(startColor, endColor, t);
            fullscreenPanel.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        if (!show)
        {
            dimmedBackground.SetActive(false);
            fullscreenPanel.SetActive(false);
        }
    }
}