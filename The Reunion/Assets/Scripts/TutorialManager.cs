using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string instruction; // Instruction text to display.
        public KeyCode requiredKey; // The key that progresses the tutorial.
    }

    public Text instructionText;         // UI Text for displaying instructions.
    public TutorialStep[] steps;         // Array of tutorial steps.
    public GameObject skipButton;        // Assign a UI button for skipping the tutorial.

    private int currentStep = 0;         // Track the current tutorial step.

    void Start()
    {
        if (steps.Length > 0)
        {
            ShowCurrentStep();
        }
    }

    void Update()
    {
        // Ensure we have steps remaining before checking for input
        if (currentStep < steps.Length)
        {
            // Check if the player presses the correct key
            if (Input.GetKeyDown(steps[currentStep].requiredKey))
            {
                AdvanceTutorial();
            }
        }
    }

    void ShowCurrentStep()
    {
        if (currentStep < steps.Length)
        {
            instructionText.text = steps[currentStep].instruction;
        }
        else
        {
            EndTutorial();
        }
    }

    void AdvanceTutorial()
    {
        currentStep++;

        if (currentStep < steps.Length)
        {
            ShowCurrentStep();
        }
        else
        {
            EndTutorial();
        }
    }

    public void SkipTutorial()
    {
        EndTutorial();
    }

    void EndTutorial()
    {
        // Load the main game scene
        SceneManager.LoadScene("Map");  // Replace "MainGame" with your actual scene name.
    }
}