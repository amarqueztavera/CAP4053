using UnityEngine;
using UnityEngine.Rendering;

public class PiecesScript : MonoBehaviour
{
    private Vector3 RightPosition;
    public bool InRightPosition; 
    public bool Selected;
    
    // Reference to DragAndDrop (set in Start)
    private DragAndDrop dragAndDropManager;

    void Start()
    { 
        RightPosition = transform.position;
        transform.position = new Vector3(Random.Range(27.5f, 33f), Random.Range(10.5f, 5.5f));
        
        // Find the DragAndDrop manager in the scene
        dragAndDropManager = FindObjectOfType<DragAndDrop>();
        if (dragAndDropManager == null)
        {
            Debug.LogError("DragAndDrop manager not found in scene!");
        }
    }

    void Update()  
    {
        if (Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            if (!Selected && !InRightPosition) // Only trigger once
            {
                transform.position = RightPosition; 
                InRightPosition = true;
                GetComponent<SortingGroup>().sortingOrder = 0;  
                
                // Notify DragAndDrop that this piece is now correctly placed
                if (dragAndDropManager != null)
                {
                    dragAndDropManager.PieceSnappedIntoPlace();
                }
            }
        }     
    }
}