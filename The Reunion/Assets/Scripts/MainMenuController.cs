using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "welcome_story";
    //[SerializeField] private string uiSceneName = "GameUI";

    public void LoadGameScenes()
    {
        StartCoroutine(LoadScenesAdditively());
        
    }

    private IEnumerator LoadScenesAdditively()
    {
        // Load the Map and UI scenes ADDITIVELY
        AsyncOperation loadMap = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Single);
        //AsyncOperation loadUI = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        // Wait for BOTH scenes to finish loading
        while (!loadMap.isDone /*|| !loadUI.isDone)*/)
        {
            yield return null;
        }

        // Set the Map scene as the ACTIVE SCENE (critical for lighting/scripts)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapSceneName));

        // Unload the Main Menu scene
        SceneManager.UnloadSceneAsync("Main Menu");

        //SceneManager.LoadSceneAsync("welcome_story", LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapSceneName));
        //SceneManager.UnloadSceneAsync("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
