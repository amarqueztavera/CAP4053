using UnityEngine;
using UnityEngine.EventSystems;

public class QuickExit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("BUTTON CLICKED");
        if (PuzzleSceneSwapper.Instance != null)
        {
            Debug.Log("Exiting scene and returning to main game.");
            PuzzleSceneSwapper.Instance.ReturnToMap();
        }
        else
        {
            Debug.LogError("PuzzleSceneSwapper.Instance is NULL! Ensure PuzzleSceneSwapper exists in the scene.");
        }
    }
}
