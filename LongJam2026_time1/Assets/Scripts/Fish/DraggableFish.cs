using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableFish : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;
    private Transform draggingContainer;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameObject container = GameObject.Find("DraggingContainer");
        if (container != null) draggingContainer = container.transform;
        
        
        parentAfterDrag = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Update the fallback to whatever slot it is currently sitting in
        parentAfterDrag = transform.parent;

        if (draggingContainer != null) 
        {
            transform.SetParent(draggingContainer);
        }
        
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Use screen position directly
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Return to the last valid parentAfterDrag (Slot or Original)
        transform.SetParent(parentAfterDrag);
        
        // Reset local position so it centers on the Slot anchor
        transform.localPosition = Vector3.zero;

        canvasGroup.blocksRaycasts = true;
    }
}