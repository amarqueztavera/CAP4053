using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMapButton : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "Map";  // Set this in Inspector

    public void ReturnToMap()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Returning to Map from: " + currentScene.name);

        // If we're already in the map scene, no action is needed
        if (currentScene.name == mapSceneName)
        {
            Debug.Log("Already in the map scene. No return needed.");
            return;
        }

        StartCoroutine(ReturnRoutine(currentScene));
    }

    private System.Collections.IEnumerator ReturnRoutine(Scene currentScene)
    {
        // Unload the current active puzzle/tutorial scene
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        yield return new WaitUntil(() => unloadOp.isDone);

        // Reload or reactivate the map scene
        Scene mapScene = SceneManager.GetSceneByName(mapSceneName);

        if (!mapScene.isLoaded)
        {
            AsyncOperation loadMap = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadMap.isDone);
            mapScene = SceneManager.GetSceneByName(mapSceneName); // re-fetch after load
        }

        // Reactivate all root objects in map scene
        foreach (GameObject obj in mapScene.GetRootGameObjects())
        {
            obj.SetActive(true);
        }

        SceneManager.SetActiveScene(mapScene);
        Debug.Log("Returned to map and reactivated objects.");
    }
}

