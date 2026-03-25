using UnityEngine;
using UnityEngine.EventSystems;

public class FishSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DraggableFish draggable = dropped.GetComponent<DraggableFish>();

        if (draggable != null)
        {
            // If the slot is occupied, swap them
            if (transform.childCount > 0)
            {
                Transform existingFish = transform.GetChild(0);
            
                // Send the existing fish to the new fish's ORIGINAL home
                existingFish.SetParent(draggable.parentAfterDrag);
                existingFish.localPosition = Vector3.zero;
            }

            // UPDATE the draggable's destination to THIS slot
            draggable.parentAfterDrag = transform;
        
            // Save the new layout
            FindObjectOfType<TeamSelectionManager>().RequestSave();
        }
    }
}