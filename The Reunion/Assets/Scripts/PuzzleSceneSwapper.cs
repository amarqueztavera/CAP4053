using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class PuzzleSceneSwapper : MonoBehaviour
{
    public static PuzzleSceneSwapper Instance;

    [Header("Core Scenes")]
    public string persistentUIScene = "GameUI";
    public string mapScene = "Map";

    private string _currentPuzzleScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //InitializeScenes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //void InitializeScenes()
    //{
    //    // Load core scenes once
    //    if (!SceneManager.GetSceneByName(persistentUIScene).isLoaded)
    //        SceneManager.LoadScene(persistentUIScene, LoadSceneMode.Additive);

    //    if (!SceneManager.GetSceneByName(mapScene).isLoaded)
    //        SceneManager.LoadScene(mapScene, LoadSceneMode.Additive);
    //}

    public void LoadPuzzleScene(string puzzleSceneName)
    {
        if (string.IsNullOrEmpty(puzzleSceneName)) return;
        StartCoroutine(SwapToPuzzleScene(puzzleSceneName));
    }

    private IEnumerator SwapToPuzzleScene(string puzzleScene)
    {
        // Load puzzle scene additively
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(puzzleScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOp.isDone);

        // Set puzzle as active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(puzzleScene));
        _currentPuzzleScene = puzzleScene;

        // Disable Map scene objects
        Scene mapScene = SceneManager.GetSceneByName("Map");
        foreach (GameObject obj in mapScene.GetRootGameObjects())
        {
            Debug.Log("Map objects disabled");
            obj.SetActive(false);
        }
    }

    public void ReturnToMap()
    {
        Debug.Log("return");
        StartCoroutine(UnloadPuzzleAndReturn());
    }

    private IEnumerator UnloadPuzzleAndReturn()
    {
        Debug.Log("back to game");
        if (!string.IsNullOrEmpty(_currentPuzzleScene))
        {
            // Unload puzzle scene
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_currentPuzzleScene);
            yield return new WaitUntil(() => unloadOp.isDone);
        }

        // Reactivate persistent map scene
        Scene mapSceneRef = SceneManager.GetSceneByName(mapScene);
        if (mapSceneRef.isLoaded)
        {
            SceneManager.SetActiveScene(mapSceneRef);

            // Re-enable Map scene objects
            Scene mapScene = SceneManager.GetSceneByName("Map");
            foreach (GameObject obj in mapScene.GetRootGameObjects())
            {
                Debug.Log("Map objects re-enabled");
                obj.SetActive(true);
            }
        }
    }
}