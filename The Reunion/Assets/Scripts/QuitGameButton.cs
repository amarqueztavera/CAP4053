using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        // Get the Button component
        button = GetComponent<Button>();
        
        // Add the quit function to the button's onClick event
        if (button != null)
        {
            button.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("QuitGameButton script is not attached to a GameObject with a Button component!");
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // If running in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If running in a built application
        Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        // Clean up by removing the listener when the script is destroyed
        if (button != null)
        {
            button.onClick.RemoveListener(QuitGame);
        }
    }
}