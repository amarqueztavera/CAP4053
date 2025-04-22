using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneClickLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadSceneAdditive()
    {
        if (!string.IsNullOrEmpty(sceneToLoad) && Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
            Debug.Log("Loading scene: " + sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene not valid or not included in build: " + sceneToLoad);
        }
    }
}

