using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
    //    StartCoroutine(LoadEssentialScenes());
    //}

    //IEnumerator LoadEssentialScenes()
    //{
    //    // Load UI if not loaded
    //    if (!SceneManager.GetSceneByName(persistentUIScene).isLoaded)
    //    {
    //        yield return SceneManager.LoadSceneAsync(persistentUIScene, LoadSceneMode.Additive);
    //    }

    //    // Load map if not loaded
    //    if (!SceneManager.GetSceneByName(mapScene).isLoaded)
    //    {
    //        yield return SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);
    //        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapScene));
    //    }
    //}

    public void LoadPuzzleScene(string puzzleSceneName)
    {
        if (SceneManager.GetActiveScene().name == puzzleSceneName) return;
        StartCoroutine(SwapToPuzzleScene(puzzleSceneName));
    }

    private IEnumerator SwapToPuzzleScene(string puzzleScene)
    {
        // Load new puzzle scene
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(puzzleScene, LoadSceneMode.Additive);
        loadOp.allowSceneActivation = true;


        // Unload current scene if it's the map
        if (SceneManager.GetSceneByName(mapScene).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(mapScene);
        }

        

        while (!loadOp.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(puzzleScene));
        _currentPuzzleScene = puzzleScene;
    }

    public void ReturnToMap()
    {
        StartCoroutine(SwapBackToMap());
    }

    private IEnumerator SwapBackToMap()
    {
        // Unload puzzle scene
        if (!string.IsNullOrEmpty(_currentPuzzleScene))
        {
            yield return SceneManager.UnloadSceneAsync(_currentPuzzleScene);
        }

        // Reload map scene
        yield return SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapScene));
    }
}