using UnityEngine;
using System;

[Serializable]
public class UI_PersonalInventorySystem
{
    [SerializeField] private int inventorySize = 1;
    [SerializeField] INGAME_Item_Data[] itemSlots;
    [SerializeField] bool[] isitemSlotsFull;

    public UI_PersonalInventorySystem()
    {
        itemSlots = new INGAME_Item_Data[inventorySize];
        isitemSlotsFull = new bool[inventorySize];
    }

    public UI_PersonalInventorySystem(int newInventorySize)
    {
        inventorySize = newInventorySize;
        itemSlots = new INGAME_Item_Data[newInventorySize];
        isitemSlotsFull = new bool[newInventorySize];
    }

    public bool Add(INGAME_Item_Data item)
    {
        for (int i = 0; i < itemSlots.Length; i += 1) {
            if (item.getItemPickupTypes() == ItemPickupTypes.Stackable) {
                if (isitemSlotsFull[i] == true) {
                    if (item.getID() == itemSlots[i].getID()) {
                        itemSlots[i].SetItemCurrentAmount(itemSlots[i].GetItemCurrentAmount() + 1);
                        return true;
                    }
                    
                } else if (isitemSlotsFull[i] == false) {
                    itemSlots[i] = item;
                    isitemSlotsFull[i] = true;
                    return true;
                }
            } else {
                if (isitemSlotsFull[i] == false) {
                    itemSlots[i] = item;
                    isitemSlotsFull[i] = true;
                    return true;
                }
            }
            
        }
        return false;
    }

    public bool Add(INGAME_Item_Data item,int SlotNumber)
    {
        if (isitemSlotsFull.Length > SlotNumber && !IsSlotFull(SlotNumber))
        {
            itemSlots[SlotNumber] = item;
            isitemSlotsFull[SlotNumber] = true;
            return true;
        }
        return false;
    }

    public bool IsSlotFull(int slot)
    {
        if (isitemSlotsFull[slot] == true)
        {
            return true;
        }
        return false;
    }

    public INGAME_Item_Data GetItem(int slot)
    {
        return itemSlots[slot];
    }

    public void Swap(int slotOne,int SlotTwo)
    {
        (itemSlots[SlotTwo], itemSlots[slotOne]) = (itemSlots[slotOne], itemSlots[SlotTwo]);
    }

    public void RemoveAt(int slotNumber)
    {
        itemSlots[slotNumber] = null;
        isitemSlotsFull[slotNumber] = false;
    }

    public bool Use(int slot)
    {
        var result = itemSlots[slot].Use(out bool AmountBelowOne);

        if (AmountBelowOne)
        {
            RemoveAt(slot);
        }
        return result;
    }

    public INGAME_Item_Data[] GetItemSlots()
    {
        return itemSlots;
    }

    public int GetSize()
    {
        return inventorySize;
    }
}
