using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject inventory;
    public static bool isPaused;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        inventory.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        inventory.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("resume button");
        pauseMenu.SetActive(false);
        inventory.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;   
    }

    public void GoToSettings()
    {
        Debug.Log("settings button");
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Settings"); //scene needs to be in build settings
    }

    public void GoToMainMenu()
    {
        Debug.Log("main menu button");
        Time.timeScale = 1f;
        //SceneManager.LoadScene("MainMenu"); //scene needs to be in build settings
    }

   
}
