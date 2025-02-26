using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject inventory;
    public GameObject settingsMenuUI;
    public static bool isPaused;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenuUI.SetActive(false);
        inventory.SetActive(false);

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
        settingsMenuUI.SetActive(false);
        inventory.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("resume button");
        pauseMenu.SetActive(false);
        settingsMenuUI.SetActive(false);
        inventory.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;   
    }

    public void OpenSettings()
    {
        Debug.Log("settings button");
        pauseMenu.SetActive(false);
        settingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void BackToPauseMenu()
    {
        pauseMenu.SetActive(true);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = true;
    }

    public void GoToMainMenu()
    {
        Debug.Log("main menu button");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); //scene needs to be in build settings
    }
}
