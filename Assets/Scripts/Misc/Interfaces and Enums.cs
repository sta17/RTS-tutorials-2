using UnityEngine;

#region Online Resources

// https://coffeebraingames.wordpress.com/2017/10/15/multi-scene-development-in-unity/

// https://stackoverflow.com/questions/1759352/how-to-mark-a-method-as-obsolete-or-deprecated

// [System.Obsolete("Possible Removal")]
// if placed above a method will produce a warning for all uses.

#endregion

#region Interfaces

public interface I_UI_Slot
{
    void Add(I_UI_SlotInterfacer obj);
    void ClearSlot();
    void Use();
}

public interface I_UI_SlotInterfacer
{
    Sprite getIcon();
    void iconInteract();
    int getID();
    string GetColouredName();
    string GetTooltipInfoText();
}

public interface I_UI_Item_SlotInterfacer: I_UI_SlotInterfacer
{
    ItemSlotTypes getItemSlotTypes();
}

public interface I_UI_IconListener
{
    void iconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj);
    void SwapSlots(UI_Slot_General from, UI_Slot_General to);
}

public interface I_UI_IconItemListener: I_UI_IconListener, I_UI_PanelInteractable
{
    void RemoveFromInventory(int slot);
    I_Selectable GetDisplayedObject();
}

public interface I_Selectable
{
    void SetSelected(bool isSelected);
    bool GetSelectedStatus();
    SelectableTypes GetSelectableType();
    int getSelectionID();
    FactionManager getFaction();
}

public interface I_StatsDisplay
{
    void UpdateDisplay(I_Selectable selectedObject);
    void ClearStats();
}

public interface I_UI_PanelInteractable
{
    void SetPlayerListener(PlayerManager player);
    void RaiseNeedToUpdateUI();
}

public interface I_Modifier
{
    void AddValue(ref int baseValue);
}


#endregion

#region Enums

public enum SelectableTypes
{
    Unit,
    Item
}

public enum ItemPickupTypes
{
    NoneStackable,
    Pickup,
    Stackable
}

public enum ItemSlotTypes
{
    Generic,
    Head,
    Shoulder,
    Chest,
    Wrist,
    Hands,
    Waist,
    Legs,
    Feet,
    Neck,
    Back,
    Finger,
    Trinket,
    OneHand,
    TwoHand,
    MainHand,
    OffHand
}

public enum Attributes
{
    Agility,
    Intellect,
    Stamina,
    Strength,
    Finesse,
    Dexterity,
    Wizdom,
    Constitution,
    Intelligence,
    Vitality,
    Knowledge,
    Willpower,
    Mind,
    Body,
    Spirit
}

[System.Serializable]
public class AttributeAmount {
    public string Name;
    public Attributes Attribute;
    public float number;
}

#endregion