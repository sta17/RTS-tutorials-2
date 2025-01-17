using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Hero : UI_DisplayItemsInGridHandler
{

    [SerializeField] private UI_Slot_Item helm;
    [SerializeField] private UI_Slot_Item chest;
    [SerializeField] private UI_Slot_Item legs;
    [SerializeField] private UI_Slot_Item boots;
    [SerializeField] private UI_Slot_Item hands;
    [SerializeField] private UI_Slot_Item mainHand;
    [SerializeField] private UI_Slot_Item offHands;

    [SerializeField] private GameObject InventoryGrid;

    public Dictionary<GameObject, UI_Slot_Item> slotsOnInterface = new();
    public GameObject[] slots;

    public void Start()
    {
        itemSlots = InventoryGrid.GetComponentsInChildren<UI_Slot_Item>().ToList();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].SetSlotNumber(i);
            itemSlots[i].SetListener(this);
        }
    }

    public void IconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj)
    {
        if (slotNumber > -1)
        {
            personalInventorySystem.Use(slotNumber);
            RaiseNeedToUpdateUI();
        }
    }

    public void SwapSlots(UI_Slot_Item from, UI_Slot_Item to)
    {
        var fromSlotNumber = from.GetSlotNumber();
        var toSlotNumber = to.GetSlotNumber();

        if (toSlotNumber < personalInventorySystem.GetSize() && toSlotNumber > -1)
        {
            DoSwapSlots(from, to);
        }
        else if (toSlotNumber <= -1)
        {
            var fromItem = personalInventorySystem.GetItem(fromSlotNumber);

            if (fromItem.getItemSlotTypes() == to.slotType)
            {
                DoSwapSlots( from,  to);
            }
        }
    }

    private void DoSwapSlots(UI_Slot_Item from, UI_Slot_Item to)
    {
        var fromSlotNumber = from.GetSlotNumber();
        var toSlotNumber = to.GetSlotNumber();

        if (toSlotNumber < 0 | fromSlotNumber < 0) {
            if (toSlotNumber < 0) {

                var fromItem = personalInventorySystem.GetItem(fromSlotNumber);
                personalInventorySystem.RemoveAt(fromSlotNumber);

                if (!to.IsEmpty())
                {
                    var toItem = personalInventorySystem.GetItem(toSlotNumber);

                    personalInventorySystem.RemoveAt(toSlotNumber);

                    personalInventorySystem.Add(toItem, fromSlotNumber);
                }

                personalInventorySystem.Add(fromItem, toSlotNumber);

            } else {
                
            }
        } else
        {
            var fromItem = personalInventorySystem.GetItem(fromSlotNumber);
            personalInventorySystem.RemoveAt(fromSlotNumber);

            if (!to.IsEmpty())
            {
                var toItem = personalInventorySystem.GetItem(toSlotNumber);

                personalInventorySystem.RemoveAt(toSlotNumber);

                personalInventorySystem.Add(toItem, fromSlotNumber);
            }
            personalInventorySystem.Add(fromItem, toSlotNumber);
        }
        RaiseNeedToUpdateUI();
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;

        if (MouseData.interfaceMouseIsOver != null && MouseData.slotHoveredOver != null)
        {
            // gets the inventory slot from the Dictionary of the User Interface.
            UI_Slot_Item mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            mouseHoverSlotData.SetTooltipPopup(tooltipPopup);
            mouseHoverSlotData.DisplayToolTip();
        }
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UI_Inventory_Hero>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.getID() >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].item.getIcon();
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            UI_Slot_Item mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            //SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}

public static class MouseData
{
    public static UI_Inventory_Hero interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}