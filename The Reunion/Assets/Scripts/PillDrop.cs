using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PillDrop : MonoBehaviour, IDropHandler
{
    public List<GameObject> correctPills; // Assign multiple correct pills in the Inspector
    public string pillColor; // Assign "Red", "Blue", or "Green" in Inspector

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedPill = eventData.pointerDrag;

        if (droppedPill != null && correctPills.Contains(droppedPill)) // Check if dropped pill belongs to allowed list
        {
            droppedPill.transform.SetParent(transform);
            droppedPill.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            // Register the correctly placed pill
            PillManager.Instance.RegisterPill(droppedPill, pillColor);
        }
    }
}
