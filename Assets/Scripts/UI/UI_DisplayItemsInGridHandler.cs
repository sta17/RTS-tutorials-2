using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_DisplayItemsInGridHandler : MonoBehaviour, I_UI_IconItemListener
{
    [SerializeField] protected List<UI_Slot_Item> itemSlots;
    [SerializeField] protected UnitController displayedUnit;
    [SerializeField] protected UI_PersonalInventorySystem personalInventorySystem;
    [SerializeField] protected UI_TooltipPopup tooltipPopup;
    [SerializeField] private UI_DragableIcon MoveableIcon2;

    public PlayerManager player;

    [SerializeField] private UIManager UIManager;

    [SerializeField] protected Sprite disabledIcon;

    [SerializeField] protected bool isEquiptment;

    public void Setup()
    {
        itemSlots = new List<UI_Slot_Item>();
        itemSlots = this.gameObject.GetComponentsInChildren<UI_Slot_Item>().ToList();

        //Debug.Log("gameObject.name: " + gameObject.name);
        //Debug.Log("itemSlots.Count: " + itemSlots.Count);

        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].SetSlotNumber(i);
            itemSlots[i].SetListener(this);
            itemSlots[i].SetTooltipPopup(tooltipPopup);
            itemSlots[i].setMoveableIcon(MoveableIcon2);
        }
    }

    public void Add(INGAME_Item_Data item, int slotNumber)
    {
        //Debug.Log("gameObject.name: " + gameObject.name);
        //Debug.Log("slotNumber: "+ slotNumber);
        //Debug.Log("itemSlots.Count: " + itemSlots.Count);
        bool outcome = itemSlots[slotNumber].Add((I_UI_SlotInterfacer)item);
    }

    public void ClearSlots()
    {
        foreach (var slot in itemSlots)
        {
            slot.ClearSlot();
        }
    }

    public void Remove(int slot)
    {
        itemSlots[slot].ClearSlot();
        var actionSlot = itemSlots[slot];
        itemSlots.RemoveAt(slot);
        itemSlots.Add(actionSlot);
        itemSlots[^1].SetSlotNumber(itemSlots.Count - 1);
    }

    public void UpdateDisplay(UnitController unit)
    {
        ClearSlots();

        if (unit != null)
        {
            if (isEquiptment)
            {
                displayedUnit = unit;
                personalInventorySystem = displayedUnit.GetpersonalInventoryEquiptmentSystem();

                for (int i = 0; i < displayedUnit.GetpersonalInventoryEquiptmentSystem().GetSize(); i++)
                {
                    if (displayedUnit.GetpersonalInventoryEquiptmentSystem().IsSlotFull(i))
                    {
                        Add(displayedUnit.GetpersonalInventoryEquiptmentSystem().GetItem(i), i);
                    }
                }

                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (i > (displayedUnit.GetpersonalInventoryEquiptmentSystem().GetSize() - 1))
                    {
                        itemSlots[i].SetDisabledIcon(disabledIcon);
                    }
                }
            }
            else
            {
                displayedUnit = unit;
                personalInventorySystem = displayedUnit.GetPersonalInventorySystem();

                for (int i = 0; i < displayedUnit.GetPersonalInventorySystem().GetSize(); i++)
                {
                    if (displayedUnit.GetPersonalInventorySystem().IsSlotFull(i))
                    {
                        Add(displayedUnit.GetPersonalInventorySystem().GetItem(i), i);
                    }
                }

                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (i > (displayedUnit.GetPersonalInventorySystem().GetSize() - 1))
                    {
                        itemSlots[i].SetDisabledIcon(disabledIcon);
                    }
                }
            }
        }
    }

    public void iconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj)
    {
        personalInventorySystem.Use(slotNumber);
        RaiseNeedToUpdateUI();
    }

    public void SetPlayerListener(PlayerManager player)
    {
        this.player = player;
    }

    public void SetUI_TooltipPopup(UI_TooltipPopup tooltipPopup)
    {
        this.tooltipPopup = tooltipPopup;
    }

    public void setMoveableIcon(UI_DragableIcon MoveableIcon2)
    {
        this.MoveableIcon2 = MoveableIcon2;
    }

    public void RaiseNeedToUpdateUI()
    {
        UIManager.UpdateUI(displayedUnit, true);
    }

    public void SwapSlots(UI_Slot_General from, UI_Slot_General to)
    {
        var fromSlotNumber = from.GetSlotNumber();
        var toSlotNumber = to.GetSlotNumber();

        if (toSlotNumber < personalInventorySystem.GetSize())
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
            RaiseNeedToUpdateUI();
        }
    }

    public void RemoveFromInventory(int slot)
    {
        personalInventorySystem.RemoveAt(slot);
        RaiseNeedToUpdateUI();
    }

    public I_Selectable GetDisplayedObject()
    {
        return displayedUnit;
    }

public void SetUIManager(UIManager UIManager)
    {
        this.UIManager = UIManager;
    }
}
