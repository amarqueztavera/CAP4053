using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))] // Forces collider component
public class PuzzleTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string puzzleSceneName;

    void OnMouseDown()
    {
        if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
        {
            Debug.LogError("Add PhysicsRaycaster to Main Camera!");
            return;
        }

        if (!string.IsNullOrEmpty(puzzleSceneName))
        {
            Debug.Log($"Attempting to load {puzzleSceneName}");
            PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
        }
    }
}