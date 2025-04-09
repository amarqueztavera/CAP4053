using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))] // Forces collider component
public class PuzzleTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string puzzleSceneName;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Objects");

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, layerMask);

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                Debug.Log("Hit layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));


                if (hit.collider.gameObject == gameObject && !string.IsNullOrEmpty(puzzleSceneName))
                {
                    Debug.Log($"Clicked {name}! Loading {puzzleSceneName}");
                    PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
                }
            }
            else
            {
                Debug.Log("No collider hit.");
            }
        }
    }


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

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Debug.Log($"Clicked {name}! Loading {puzzleSceneName}");
    //    if (!string.IsNullOrEmpty(puzzleSceneName))
    //    {
    //        PuzzleSceneSwapper.Instance.LoadPuzzleScene(puzzleSceneName);
    //    }
    //}
}