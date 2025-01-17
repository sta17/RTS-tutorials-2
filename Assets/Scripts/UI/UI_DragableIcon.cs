using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DragableIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] private GameObject IconFrame;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasgroup;
    [SerializeField] public UI_Slot_Item slot;
    [SerializeField] private Camera cam;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //canvasgroup = GetComponent<CanvasGroup>();
    }

    public void SetSlot(UI_Slot_Item _slot)
    {
        slot = _slot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasgroup.alpha = .9f;
        //canvasgroup.blocksRaycasts = false;

        icon.sprite = slot.GetObjInSlot().getIcon();
        IconFrame.SetActive(true);
        rectTransform.anchoredPosition = slot.GetGameObjectIcon().GetComponent<RectTransform>().anchoredPosition;
        rectTransform.position = slot.GetGameObjectIcon().GetComponent<RectTransform>().position;
        //slot.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //slot.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Save the current layer the dropped object is in,
        // and then temporarily place the object in the IgnoreRaycast layer to avoid hitting self with Raycast.
        int oldLayer = gameObject.layer;
        gameObject.layer = 0;

        //var screenRay = Camera.main.ScreenPointToRay(transform.position);
        var screenRay = cam.ScreenPointToRay(Input.mousePosition);

        // Perform Physics.Raycast from transform and see if any 3D object was under transform.position on drop.
        if (Physics.Raycast(screenRay, out RaycastHit hit))
        //if (Physics.SphereCast(screenRay.origin, 2, screenRay.direction, out hit, 1000))
        {
            if (hit.transform.CompareTag("Unit"))
            {
                var dropComponent = hit.transform.GetComponent<UnitController>();
                if (dropComponent != null)
                    dropComponent.OnDrop(eventData);
            }
        }

        // Reset the object's layer to the layer it was in before the drop.
        gameObject.layer = oldLayer;

        //canvasgroup.alpha = 1f;
        //canvasgroup.blocksRaycasts = true;
        IconFrame.SetActive(false);
        //slot.OnEndDrag(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //slot.OnDrop(slot);
            //slot.OnDrop(eventData);
        }
    }

    public void SetCam(Camera cam)
    {
        this.cam = cam;
    }


}