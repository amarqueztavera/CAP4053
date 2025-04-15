using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering; 

public class DragAndDrop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject SelectedPiece; 
    int OIL =  1; 
    void Start()
    {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.transform.CompareTag("Puzzle"))
            {
                if(!hit.transform.GetComponent<PiecesScript>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    SelectedPiece.GetComponent<PiecesScript>().Selected = true;   
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                } 
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(SelectedPiece != null)
            {
                SelectedPiece.GetComponent<PiecesScript>().Selected = false;
                SelectedPiece = null;  
            }     
        } 
        if(SelectedPiece !=  null) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  
            SelectedPiece.transform.position = new Vector3(mousePos.x, mousePos.y, 0); 
        } 
    }
}
