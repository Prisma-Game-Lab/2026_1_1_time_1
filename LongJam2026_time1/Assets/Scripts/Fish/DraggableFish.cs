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
        if (eventData.button != PointerEventData.InputButton.Left) return;

        parentAfterDrag = transform.parent;

        if (draggingContainer != null)
            transform.SetParent(draggingContainer);

        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }
}