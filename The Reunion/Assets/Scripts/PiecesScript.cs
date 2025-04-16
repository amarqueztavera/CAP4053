using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering; 

public class PiecesScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 RightPosition;
    public bool InRightPosition; 
    public bool Selected;
    public int correctCount = 0;
    [Header("Clue Settings")]
    public string clueID = "Computer"; // The ID of the clue to be added
    void Start()
    { 
        RightPosition = transform.position;
        transform.position = new Vector3(Random.Range(27.5f, 33f ),  Random.Range(20.5f, 13.5f));

    }

    // Update is called once per frame
    void Update()  
    {
        if(Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            if(!Selected ) {
                if(InRightPosition == false)
                {
                    transform.position = RightPosition; 
                    InRightPosition = true;
                    GetComponent<SortingGroup>().sortingOrder = 0;  
                    correctCount++;
                    if(correctCount == 36)
                    {
                        Debug.Log("All pieces are in the right position!");
                        // Add code to handle the completion of the puzzle here
                        //yield return new WaitForSeconds(1.5f); // Adjust time as needed

                        SaveSystem.MarkPuzzleComplete(clueID); 
                        ClueEventManager.PuzzleCompleted(clueID); 
                        Clue clueFromDB = ClueDatabase.Instance.GetClueByName(clueID);
                        if (clueFromDB != null)
                        {
                            InventoryManager.Instance.AddClue(clueFromDB);
                            Debug.Log($"Clue '{clueID}' added from database.");
                        }
                        else {
                            Debug.LogError($"Clue '{clueID}' NOT found in database!");
                        }

                        // Force immediate save
                        PlayerPrefs.Save();

                        Debug.Log("Puzzle completed! Returning to map...");
                        PuzzleSceneSwapper.Instance.ReturnToMap();
                    }
                } 
            }
        }     
    }
} 



