using UnityEngine;

public class MAP_Interactable_Item : MonoBehaviour, I_Selectable
{
    #region Variables, Constructors and Setup

    [Header("Item Properties")]
    [SerializeField] private INGAME_Item_Data item;

    [Header("Selection and Interaction")]
    [SerializeField] private SelectableTypes selectionType = SelectableTypes.Item;
    [SerializeField] private bool selected = false;
    [SerializeField] private Canvas SelectionCircle;
    [SerializeField] private float InteractRange;
    [SerializeField] private int selectionID;

    void OnValidate()
    {
        if (item != null)
        {
            this.gameObject.name = item.getItemName();
        }
    }

    void Start()
    {
        //item = new ItemState();
    }

    #endregion

    #region Getters and Setters
    public void SetSelected(bool isSelected)
    {
        selected = isSelected;
        SelectionCircle.gameObject.SetActive(isSelected);
    }

    public bool GetSelectedStatus()
    {
        return selected;
    }

    public float getInteractRange()
    {
        return InteractRange;
    }
    public Sprite getIcon()
    {
        return item.getIcon();
    }

    public int getPlayerOwner()
    {
        return -1;
    }

    public SelectableTypes GetSelectableType()
    {
        return selectionType;
    }

    [System.Obsolete("Possible Removal")]
    public int getSelectionID()
    {
        return selectionID;
    }

    public FactionManager getFaction()
    {
        return null;
    }

    public ItemPickupTypes getType()
    {
        return item.getType();
    }

    public INGAME_Item_Data getItem()
    {
        return item;
    }

    #endregion

    #region Misc

    public void ModelCleanUp()
    {
        Destroy(this.gameObject);
    }

    public bool HandlePickupType(UnitController unit)
    {
        //Debug.Log("Item Picked up");
        item.Use();
        return true;
    }

    #endregion

}