using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public string puzzleSceneName;

    void OnMouseDown()
    {
        Debug.Log("puzzle clicked");
        PuzzleSceneSwapper.Instance.LoadPuzzle(puzzleSceneName);

    }
}
