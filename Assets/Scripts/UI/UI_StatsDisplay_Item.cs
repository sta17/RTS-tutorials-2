using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StatsDisplay_Item : MonoBehaviour, I_StatsDisplay
{
    [SerializeField]
    private SO_Item_Data currentItemStats;

    public Image icon;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI type;

    public void UpdateDisplay(I_Selectable selectedObject)
    {
        var newItem = (MAP_Interactable_Item) selectedObject;

        currentItemStats = newItem.getItem().getItemStats();

        icon.sprite = currentItemStats.icon;
        icon.enabled = true;

        itemName.text = currentItemStats.itemName;
        amount.text = currentItemStats.itemAmount.ToString();
        type.text = currentItemStats.type.ToString();
    }

    public void ClearStats()
    {
        if (currentItemStats != null)
        {
            currentItemStats = null;

            icon.sprite = null;
            icon.enabled = false;

            itemName.text = "";
            amount.text = "";
            type.text = "";
        }
    }
}
