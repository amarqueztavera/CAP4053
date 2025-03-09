using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class PuzzleSceneSwapper : MonoBehaviour
{
    public static PuzzleSceneSwapper Instance;

    [Header("Core Scenes")]
    public string persistentUIScene = "GameUI";
    public string mapScene = "Map";

    private string _currentGameScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScenes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeScenes()
    {
        // Load persistent UI if not already loaded
        if (!SceneManager.GetSceneByName(persistentUIScene).isLoaded)
        {
            SceneManager.LoadScene(persistentUIScene, LoadSceneMode.Additive);
        }

        // Load initial map
        LoadMap();
    }

    public void LoadPuzzle(string puzzleSceneName)
    {
        StartCoroutine(SwapScenes(puzzleSceneName));
    }

    public void LoadMap()
    {
        StartCoroutine(SwapScenes(mapScene));
    }

    private IEnumerator SwapScenes(string newScene)
    {
        // Unload current scene
        if (_currentGameScene != null && _currentGameScene != persistentUIScene)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_currentGameScene);
            while (!unloadOp.isDone) yield return null;
        }

        // Load new scene
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        while (!loadOp.isDone) yield return null;

        _currentGameScene = newScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
    }
}