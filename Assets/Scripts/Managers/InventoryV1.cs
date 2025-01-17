using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryV1 : MonoBehaviour
{
    public List<MAP_Interactable_Item> items;
    public int itemSlotNumber;

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;

    public void Start()
    {
        items = new List<MAP_Interactable_Item>();
    }

    public bool Add(MAP_Interactable_Item item)
    {
        if(items.Count < itemSlotNumber)
        {
            items.Add(item);

            if (OnItemChangedCallBack != null)
            {
                OnItemChangedCallBack.Invoke();
            }

            return true;
        } else
        {
            return false;
        }
    }

    public void Remove(MAP_Interactable_Item item)
    {
        items.Remove(item);

        if (OnItemChangedCallBack != null)
        {
            OnItemChangedCallBack.Invoke();
        }
    }
}
