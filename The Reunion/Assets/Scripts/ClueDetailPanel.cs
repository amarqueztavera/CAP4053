using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClueDetailPanel : MonoBehaviour
{
    public Image clueImage;
    public TMP_Text clueNameText;
    public TMP_Text clueDescText;
    public Button closeButton;

    public static ClueDetailPanel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false); // Hide panel on load

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
    }

    public void ShowClueDetails(Sprite clueSprite, string clueName, string clueDescription)
    {
        clueImage.sprite = clueSprite;
        clueNameText.text = clueName;
        clueDescText.text = clueDescription;

        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
