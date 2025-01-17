using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Slot_Item : UI_Slot_General, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public Button removeButton;
    public TextMeshProUGUI stackText;
    public GameObject stackTextGameObject;
    [SerializeField] private UI_DragableIcon MoveableIcon2;
    public ItemSlotTypes slotType;
    public INGAME_Item_Data item = new();
    public delegate void SlotUpdated(UI_Slot_Item _slot);
    [SerializeField] public SlotUpdated OnAfterUpdate;
    [SerializeField] public SlotUpdated OnBeforeUpdate;

    public new bool Add(I_UI_SlotInterfacer newObj) {
        var tempNewObj = (I_UI_Item_SlotInterfacer)newObj;
        //if (ItemSlotTypes.Generic == tempNewObj.getItemSlotTypes())
        //{
        objInSlot = newObj;
        item = (INGAME_Item_Data)objInSlot;

        icon.sprite = objInSlot.getIcon();
        icon.enabled = true;
        icon.color = Color.white;
        button.interactable = true;


        removeButton.interactable = true;

        if (item.GetItemCurrentAmount() > 1) {
            stackText.text = item.GetItemCurrentAmount().ToString();
            stackTextGameObject.SetActive(true);
        }

        return true;
        //} 
        //else if (slotType == tempNewObj.getItemSlotTypes())
        //{
        //objInSlot = newObj;

        //icon.sprite = objInSlot.getIcon();
        //icon.enabled = true;
        //button.interactable = true;
        //return true;
        //}
        //return false;
    }

    public void SetIcon(Sprite newIcon) {
        icon.sprite = newIcon;
        icon.enabled = true;
        icon.color = Color.white;
    }

    public void SetDisabledIcon(Sprite newIcon) {
        icon.sprite = newIcon;
        icon.enabled = true;
        icon.color = Color.gray;
        stackTextGameObject.SetActive(false);
    }

    public void setMoveableIcon(UI_DragableIcon MoveableIcon2) {
        this.MoveableIcon2 = MoveableIcon2;
    }

    public new void ClearSlot() {
        objInSlot = null;

        icon.sprite = null;
        icon.enabled = false;
        button.interactable = false;

        removeButton.interactable = false;
        stackTextGameObject.SetActive(false);
    }

    public void UpdateSlot(INGAME_Item_Data _item, int _amount) {
        OnBeforeUpdate?.Invoke(this);
        item = _item;
        _item.SetItemCurrentAmount(_amount);
        OnAfterUpdate?.Invoke(this);
    }

    public void RemoveItem() {
        UpdateSlot(new INGAME_Item_Data(), 0);
    }

    public new void Use() {
        //Debug.Log("slot used");
        if (objInSlot != null) {
            Listener.iconInteractedWith(SlotNumber, objInSlot);
        }
    }

    public void OnRemoveButton() {
        //inventory.Remove(item);
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            //get icon
            GameObject eventGameObject = eventData.pointerDrag;
            UI_Slot_Item fromItemSlot = eventGameObject.GetComponent<UI_Slot_Item>();
            UI_Slot_Item toItemSlot = this;
            if (fromItemSlot != null) {
                var fromItemType = fromItemSlot.GetslotType();
                var toItemType = toItemSlot.GetslotType();

                if ((fromItemType != ItemSlotTypes.Generic) | (toItemType != ItemSlotTypes.Generic)) {
                    var fromIngameItem = (INGAME_Item_Data)fromItemSlot.GetObjInSlot();
                    var listener = (I_UI_IconItemListener)fromItemSlot.GetListener();
                    var displayedUnit = (UnitController)listener.GetDisplayedObject();

                    bool result = false;

                    // Is it inventory line item.
                    if (toItemType == ItemSlotTypes.Generic) {

                        var inv = displayedUnit.GetPersonalInventorySystem();
                        result = inv.Add(fromIngameItem, toItemSlot.GetSlotNumber());

                        // Not Generic and types match
                    } else if (toItemType == fromIngameItem.getItemSlotType()) {

                        var inv = displayedUnit.GetpersonalInventoryEquiptmentSystem();
                        result = inv.Add(fromIngameItem, toItemSlot.GetSlotNumber());

                    }

                    if (result) {
                        // remove item from slot now
                        fromItemSlot.NotifyListenerDragDrop();
                    }
                } else {
                    // if empty do move item script
                    Listener.SwapSlots(from: fromItemSlot, to: toItemSlot);
                }
            }
        }

    }

    public void NotifyListenerDragDrop() {
        var IconListener = (I_UI_IconItemListener)Listener;
        IconListener.RemoveFromInventory(SlotNumber);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (MoveableIcon2 != null) {
            MoveableIcon2.SetSlot(this);
            MoveableIcon2.OnBeginDrag(eventData);
        }

    }

    public void OnEndDrag(PointerEventData eventData) {
        if (MoveableIcon2 != null) {
            MoveableIcon2.OnEndDrag(eventData);
        }

    }

    public ItemSlotTypes GetslotType() {
        return slotType;
    }

    public void OnDrag(PointerEventData eventData) {
        if (MoveableIcon2 != null) {
            MoveableIcon2.OnDrag(eventData);
        }

    }

}
