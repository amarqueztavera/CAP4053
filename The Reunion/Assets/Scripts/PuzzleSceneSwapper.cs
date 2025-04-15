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
        //// Reset NPCs before unloading puzzle
        //NPCStateManager.Instance.ResetAllNPCs();

        StartCoroutine(UnloadPuzzleAndReturn());

    }

    private IEnumerator UnloadPuzzleAndReturn()
    {
        Debug.Log("Returning to game");
        if (!string.IsNullOrEmpty(_currentPuzzleScene))
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_currentPuzzleScene);
            yield return new WaitUntil(() => unloadOp.isDone);
        }

        Scene mapSceneRef = SceneManager.GetSceneByName(mapScene);
        if (mapSceneRef.isLoaded)
        {
            // Reactivate map scene objects
            foreach (GameObject obj in mapSceneRef.GetRootGameObjects())
            {
                obj.SetActive(true);
            }

            // Wait one frame to ensure scene is fully loaded
            yield return null;

            // Force reset NPCs after scene is ready
            NPCStateManager.Instance.ResetAllNPCs();

            // Explicitly restart NPC patrols
            var npcs = FindObjectsByType<DELETE>(FindObjectsSortMode.None);
            foreach (var npc in npcs)
            {
                if (npc.isActiveAndEnabled)
                {
                    npc.ForceReset();
                }
            }
        }
    }
}