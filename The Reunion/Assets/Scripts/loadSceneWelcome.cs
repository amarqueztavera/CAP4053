using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class loadSceneWelcome : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "Map";
    [SerializeField] private string uiSceneName = "GameUI";
    void OnEnable()
    {
        //SceneManager.LoadScene("Map", LoadSceneMode.Single);

        
        StartCoroutine(LoadScenesAdditively());
    }

   /* public void LoadGameScenes()
    {
        StartCoroutine(LoadScenesAdditively());
    }*/

    private IEnumerator LoadScenesAdditively()
    {
        // Load the Map and UI scenes ADDITIVELY
        AsyncOperation loadMap = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Additive);
        AsyncOperation loadUI = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        // Wait for BOTH scenes to finish loading
        while (!loadMap.isDone || !loadUI.isDone)
        {
            yield return null;
        }

        // Set the Map scene as the ACTIVE SCENE (critical for lighting/scripts)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapSceneName));

        // Unload the Main Menu scene
        SceneManager.UnloadSceneAsync("welcome_story");
    }

}