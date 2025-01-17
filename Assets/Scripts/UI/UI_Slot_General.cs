using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot_General : MonoBehaviour, I_UI_Slot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image icon;

    [SerializeField] protected GameObject GameObjectIcon;
    public Button button;

    [SerializeField] protected I_UI_SlotInterfacer objInSlot;

    [SerializeField] protected int SlotNumber;

    [SerializeField] protected I_UI_IconListener Listener;

    [SerializeField] protected UI_TooltipPopup tooltipPopup;

    public int GetSlotNumber()
    {
        return SlotNumber;
    }

    public I_UI_SlotInterfacer GetObjInSlot()
    {
        return objInSlot;
    }

    public void SetTooltipPopup(UI_TooltipPopup tooltipPopup)
    {
        this.tooltipPopup = tooltipPopup;
    }

    public void DisplayToolTip()
    {
        if (objInSlot != null)
        {
            if (tooltipPopup != null)
            {
                tooltipPopup.DisplayInfo(objInSlot);
            }
        }
    }

    public void HideToolTip()
    {
        if (tooltipPopup != null)
        {
            tooltipPopup.HideInfo();
        }
    }

    public void SetSlotNumber(int newSlotNumber)
    {
        SlotNumber = newSlotNumber;
    }

    public I_UI_IconListener GetListener()
    {
        return Listener;
    }

    public void SetListener(I_UI_IconListener newListener)
    {
        Listener = newListener;
    }

    public void Add(I_UI_SlotInterfacer newObj)
    {
        objInSlot = newObj;

        icon.sprite = objInSlot.getIcon();
        icon.enabled = true;
        button.interactable = true;
    }

    public void ClearSlot()
    {
        objInSlot = null;

        icon.sprite = null;
        icon.enabled = false;
        button.interactable = false;
    }

    public void Use()
    {
        objInSlot.iconInteract();
        Listener.iconInteractedWith(SlotNumber, objInSlot);
    }

    public bool IsEmpty()
    {
        if (objInSlot == null)
        {
            return true;
        }
        return false;
    }

    public GameObject GetGameObjectIcon()
    {
        return GameObjectIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objInSlot != null && tooltipPopup != null)
        {
            tooltipPopup.DisplayInfo(objInSlot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPopup != null)
        {
            tooltipPopup.HideInfo();
        }
    }
}