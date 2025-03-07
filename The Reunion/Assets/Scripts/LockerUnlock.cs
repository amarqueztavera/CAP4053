using UnityEngine;
using UnityEngine.UI;

public class LockerUnlock : MonoBehaviour
{
    public InputField inputField1;
    public InputField inputField2;
    public InputField inputField3;
    public Text resultText;
    public GameObject locker; // Reference to the locker

    private string correctCode1 = "08";
    private string correctCode2 = "09";
    private string correctCode3 = "11";

    public void CheckCode()
    {
        // Normalize input by removing leading zeros
        string enteredCode1 = inputField1.text.TrimStart('0');
        string enteredCode2 = inputField2.text.TrimStart('0');
        string enteredCode3 = inputField3.text.TrimStart('0');

        // Normalize correct codes as well
        string normalizedCode1 = correctCode1.TrimStart('0');
        string normalizedCode2 = correctCode2.TrimStart('0');
        string normalizedCode3 = correctCode3.TrimStart('0');

        // Compare normalized values
        if (enteredCode1 == normalizedCode1 && enteredCode2 == normalizedCode2 && enteredCode3 == normalizedCode3)
        {
            resultText.text = "Locker Unlocked!";
            resultText.color = Color.green;
            UnlockLocker();
        }
        else
        {
            resultText.text = "Incorrect Code!";
            resultText.color = Color.red;
        }
    }


    void UnlockLocker()
    {
        locker.SetActive(false); // Hide the locker when unlocked
        Debug.Log("Locker has been unlocked!");
    }
}
