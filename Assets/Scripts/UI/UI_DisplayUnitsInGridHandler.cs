using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_DisplayUnitsInGridHandler : MonoBehaviour, I_UI_IconListener
{
    [SerializeField] protected List<UI_Slot_General> selectedUnits;
    [SerializeField] protected bool updateUiOnly = true;
    [SerializeField] protected PlayerManager player;
    [SerializeField] protected UI_TooltipPopup tooltipPopup;

    void Awake()
    {
        selectedUnits = new List<UI_Slot_General>();
        selectedUnits = this.gameObject.GetComponentsInChildren<UI_Slot_General>().ToList();

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].SetSlotNumber(i);
            selectedUnits[i].SetListener(this);
        }
    }

    public void Add(I_UI_SlotInterfacer unit,int slotNumber)
    {
        var listSlot = selectedUnits[slotNumber];
        listSlot.Add(unit);
    }

    public void DeselectUnits()
    {
        foreach (var slot in selectedUnits)
        {
            slot.ClearSlot();
        }
    }

    public void Remove(int slot)
    { 
        selectedUnits[slot].ClearSlot();
        var actionSlot = selectedUnits[slot];
        selectedUnits.RemoveAt(slot);
        selectedUnits.Add(actionSlot);
        selectedUnits[selectedUnits.Count-1].SetSlotNumber(selectedUnits.Count - 1);
    }

    public void iconInteractedWith(int slotNumber, I_UI_SlotInterfacer obj)
    {
        player.SelectUnit((UnitController)obj, updateUiOnly: updateUiOnly);
    }

    public void SwapSlots(UI_Slot_General from, UI_Slot_General to)
    {
        throw new System.NotImplementedException();
    }
    public void SetPlayerListener(PlayerManager player)
    {
        this.player = player;
    }

    public void SetUI_TooltipPopup(UI_TooltipPopup tooltipPopup)
    {
        this.tooltipPopup = tooltipPopup;
    }
}
