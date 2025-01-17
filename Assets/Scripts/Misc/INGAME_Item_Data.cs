using UnityEngine;

[System.Serializable]
public class INGAME_Item_Data : I_UI_Item_SlotInterfacer
{
    #region Variables and Constructors

    [Header("Item Data")]
    [SerializeField] private SO_Item_Data item;
    [SerializeField] private int itemID;
    [SerializeField] private ItemPickupTypes itemType;
    [SerializeField] private int itemAmount = 1;

    public INGAME_Item_Data()
    {
        
    }
    public INGAME_Item_Data(int newItemAmount, ItemPickupTypes newItemType, SO_Item_Data newItem)
    {
        itemAmount = newItemAmount;
        itemType = newItemType;
        item = newItem;
    }

    #endregion

    #region Misc

    public void iconInteract()
    {
        bool AmountBelowOne;
        var result = Use(out AmountBelowOne);
    }

    public bool Use(out bool AmountBelowOne)
    {
        itemAmount -= 1;

        if (itemAmount < 1)
        {
            AmountBelowOne = true;
        } 
        else
        {
            AmountBelowOne = false;
        }

        return true;
    }

    public bool Use()
    {
        itemAmount -= 1;
        return true;
    }

    #endregion

    #region Getters and Setters

    public ItemSlotTypes getItemSlotTypes()
    {
        return item.slotType;
    }

    public ItemPickupTypes getItemPickupTypes() {
        return itemType;
    }

    public void SetItemCurrentAmount(int amount)
    {
        itemAmount = amount;
    }

    public int GetItemCurrentAmount()
    {
        return itemAmount;
    }

    public Sprite getIcon()
    {
        return item.icon;
    }

    public int getID()
    {
        return itemID;
    }

    public ItemPickupTypes getType()
    {
        return item.type;
    }

    public ItemSlotTypes getItemSlotType()
    {
        return item.slotType;
    }

    public string getItemName()
    {
        return item.itemName;
    }

    public SO_Item_Data getItemStats()
    {
        return item;
    }

    public string GetColouredName()
    {
        return item.data.ColouredName;
    }

    public string GetTooltipInfoText()
    {
        return item.data.TooltipInfoText;
    }

    #endregion

}
