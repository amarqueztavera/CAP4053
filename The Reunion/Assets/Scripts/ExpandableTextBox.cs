using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro

public class ExpandableTextBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentRect;
    [SerializeField] private TMP_Text textComponent; // or Text for legacy Text
    [SerializeField] private Scrollbar scrollbar;
    
    [Header("Size Settings")]
    [SerializeField] private Vector2 collapsedSize;
    [SerializeField] private Vector2 expandedSize;
    
    [Header("Background Colors")]
    [SerializeField] private Color collapsedColor;
    [SerializeField] private Color expandedColor;
    
    private bool isExpanded = false;
    private Image backgroundImage;

    private void Awake()
    {
        if (scrollRect == null) scrollRect = GetComponentInChildren<ScrollRect>();
        if (contentRect == null) contentRect = scrollRect.content;
        if (textComponent == null) textComponent = GetComponentInChildren<TMP_Text>();
        if (scrollbar == null) scrollbar = GetComponentInChildren<Scrollbar>();
        
        backgroundImage = GetComponent<Image>();
        
        // Initial setup
        CollapseTextBox();
        UpdateScrollbarVisibility();
    }

    public void ToggleTextBox()
    {
        if (isExpanded) CollapseTextBox();
        else ExpandTextBox();
    }

    private void ExpandTextBox()
    {
        isExpanded = true;
        GetComponent<RectTransform>().sizeDelta = expandedSize;
        backgroundImage.color = expandedColor;
        scrollRect.verticalScrollbar = scrollbar; // Re-enable scrollbar
        UpdateScrollbarVisibility();
    }

    private void CollapseTextBox()
    {
        isExpanded = false;
        GetComponent<RectTransform>().sizeDelta = collapsedSize;
        backgroundImage.color = collapsedColor;
        scrollRect.verticalNormalizedPosition = 1; // Reset scroll to top
        UpdateScrollbarVisibility();
    }

    private void UpdateScrollbarVisibility()
    {
        if (scrollbar == null) return;
        
        // Calculate if text overflows
        bool textOverflows = textComponent.preferredHeight > contentRect.rect.height;
        
        // Only show scrollbar when expanded AND text overflows
        scrollbar.gameObject.SetActive(isExpanded && textOverflows);
        scrollRect.verticalScrollbarVisibility = 
            isExpanded ? ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport 
                      : ScrollRect.ScrollbarVisibility.Permanent;
    }

    // Call this whenever you update the text
    public void UpdateText(string newText)
    {
        textComponent.text = newText;
        UpdateScrollbarVisibility();
        
        // Rebuild layout to get proper text dimensions
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        Canvas.ForceUpdateCanvases();
    }
}