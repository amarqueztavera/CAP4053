using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class FinalChoiceManager : MonoBehaviour
{
    [Header("Weapon Buttons")]
    public Button[] weaponButtons;

    [Header("Suspect Buttons")]
    public Button[] suspectButtons;

    [Header("Submit Button")]
    public Button submitButton;

    private int selectedWeaponIndex = -1;
    private int selectedSuspectIndex = -1;

    [Header("Correct Answers")]
    public int correctWeaponIndex = 1;
    public int correctSuspectIndex = 2;

    public TextMeshProUGUI resultText;

    void Start()
    {
        // Disable the submit button until both selections are made
        submitButton.interactable = false;
    }

    // Public wrappers for Unity's OnClick event (no parameters allowed in UI)
    public void SelectWeapon0() => SelectWeapon(0);
    public void SelectWeapon1() => SelectWeapon(1);
    public void SelectWeapon2() => SelectWeapon(2);
    public void SelectWeapon3() => SelectWeapon(3);

    public void SelectSuspect0() => SelectSuspect(0);
    public void SelectSuspect1() => SelectSuspect(1);
    public void SelectSuspect2() => SelectSuspect(2);
    public void SelectSuspect3() => SelectSuspect(3);

    void SelectWeapon(int index)
    {
        selectedWeaponIndex = index;
        HighlightSelection(weaponButtons, index);
        CheckSelections();
    }

    void SelectSuspect(int index)
    {
        selectedSuspectIndex = index;
        HighlightSelection(suspectButtons, index);
        CheckSelections();
    }

    void HighlightSelection(Button[] buttons, int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var targetImage = buttons[i].targetGraphic as Image;
            if (targetImage != null)
            {
                targetImage.color = (i == selectedIndex) ? Color.green : Color.white;
            }
        }
    }

    void CheckSelections()
    {
        submitButton.interactable = selectedWeaponIndex != -1 && selectedSuspectIndex != -1;
    }

    public void SubmitChoice()
    {
        Debug.Log("Submit pressed");

        bool weaponCorrect = selectedWeaponIndex == correctWeaponIndex;
        bool suspectCorrect = selectedSuspectIndex == correctSuspectIndex;

        int score = 0;
        if (weaponCorrect) score++;
        if (suspectCorrect) score++;

        Debug.Log("Score: " + score);

        if (resultText != null)
        {
            resultText.text = score + "/2";
            Debug.Log("Updated resultText to: " + score + "/2");
        }
        else
        {
            Debug.LogWarning("resultText is NOT assigned!");
        }

        submitButton.interactable = false;

        StartCoroutine(SwapSceneAfterDelay(score));
    }



    IEnumerator SwapSceneAfterDelay(int score)
    {
        yield return new WaitForSeconds(2f); // Delay to show result

        if (score == 2)
        {
            SceneManager.LoadScene("End Good");
        }
        else if (score == 1)
        {
            SceneManager.LoadScene("End Neutral");
        }
        else
        {
            SceneManager.LoadScene("End Bad");
        }
    }


    public void GoToClueReview()
    {
        SceneManager.LoadScene("End Clue Review");
    }
}
