using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickableClue : MonoBehaviour
{
    [Header("Clue Settings")]
    public string clueID; // Just type the clue ID/name here

    private void Start()
    {
        // If already collected in past, destroy this clue
        if (SaveSystem.IsClueCollected(clueID))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Objects"); // Make sure clue is on "Objects" layer

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                TryCollectClue();
            }
        }
    }

    private void TryCollectClue()
    {
        Clue clueFromDB = ClueDatabase.Instance.GetClueByName(clueID);
        if (clueFromDB == null)
        {
            Debug.LogError($"Clue '{clueID}' not found in database!");
            return;
        }

        if (InventoryManager.Instance.AddClue(clueFromDB))
        {
            Debug.Log($"Clue '{clueID}' added to inventory.");
            SaveSystem.MarkClueCollected(clueID);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Clue '{clueID}' could not be added (possibly already in inventory).");
        }
    }
}
