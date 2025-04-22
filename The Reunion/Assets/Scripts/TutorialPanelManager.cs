using UnityEngine;

public class TutorialPanelManager : MonoBehaviour
{
    public static TutorialPanelManager Instance;

    public GameObject tutorialPanel;

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

        tutorialPanel.SetActive(false); // Hide panel on load
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
