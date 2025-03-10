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
            //Destroy(gameObject);
        }
    }

    //void InitializeScenes()
    //{
    //    // Ensure UI and Map are loaded first
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
        // 1. Load the puzzle scene first
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(puzzleScene, LoadSceneMode.Additive);
        loadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => loadOp.isDone);

        // 2. Set active scene AFTER loading
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(puzzleScene));
        _currentPuzzleScene = puzzleScene;

        // 3. Unload map scene only after puzzle is fully ready
        if (SceneManager.GetSceneByName(mapScene).isLoaded)
            yield return SceneManager.UnloadSceneAsync(mapScene);
    }

    public void ReturnToMap()
    {
        StartCoroutine(SwapBackToMap());
    }

    private IEnumerator SwapBackToMap()
    {
        // 1. Unload puzzle scene first
        if (!string.IsNullOrEmpty(_currentPuzzleScene))
            yield return SceneManager.UnloadSceneAsync(_currentPuzzleScene);

        // 2. Reload map scene
        if (!SceneManager.GetSceneByName(mapScene).isLoaded)
            yield return SceneManager.LoadSceneAsync(mapScene, LoadSceneMode.Additive);

        // 3. Reactivate map scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapScene));
    }
}