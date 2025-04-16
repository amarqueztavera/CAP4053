using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class welcomeSwap : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "Map";
    [SerializeField] private string uiSceneName = "GameUI";
    
    private bool scenesLoaded = false; // Flag to prevent duplicate loading
    
    void OnEnable()
    {
        if (!scenesLoaded)
        {
            // Clear any saved player preferences (game history/save data)
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            StartCoroutine(LoadScenesAdditively());
            scenesLoaded = true; // Mark as loaded
        }
    }

    private IEnumerator LoadScenesAdditively()
    {
        // Check if scenes are already loaded to prevent duplicates
        if (!SceneManager.GetSceneByName(mapSceneName).isLoaded && 
            !SceneManager.GetSceneByName(uiSceneName).isLoaded)
        {
            // Load the Map and UI scenes additively
            AsyncOperation loadMap = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Additive);
            AsyncOperation loadUI = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

            // Wait for both scenes to finish loading
            while (!loadMap.isDone || !loadUI.isDone)
            {
                yield return null;
            }

            // Set the Map scene as the active scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapSceneName));
            
            // Initialize components in new scenes
            InitializeNewScenes();
        }

        // Unload the welcome story scene
        yield return SceneManager.UnloadSceneAsync("welcome_story");
    }

    private void InitializeNewScenes()
    {
        // Add any initialization logic for your new scenes here
    }
}