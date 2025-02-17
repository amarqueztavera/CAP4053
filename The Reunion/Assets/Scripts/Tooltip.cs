using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipUI; // Assign a UI Text element in Unity
    private string description;

    public void SetDescription(string text)
    {
        description = text;
    }

    public void OnPointerEnter()
    {
        tooltipUI.SetActive(true);
        tooltipUI.GetComponent<Text>().text = description;
    }

    public void OnPointerExit()
    {
        tooltipUI.SetActive(false);
    }
}
