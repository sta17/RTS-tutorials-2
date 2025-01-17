using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public List<MAP_Interactable_Item> items = new List<MAP_Interactable_Item>();
    public int itemSlotNumber;

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;

    public abstract bool Add(MAP_Interactable_Item item);

    public abstract void Remove(MAP_Interactable_Item item);

}
