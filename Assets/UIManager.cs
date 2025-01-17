using UnityEngine;

public class UIManager : MonoBehaviour
{

    public PlayerManager player;

    [Header("UI")]
    [SerializeField] private I_Selectable displayedObject;
    [SerializeField] private UI_StatsDisplay_Unit StatsPanelUnits; 
    [SerializeField] private UI_StatsDisplay_Item StatsPanelItems;
    [SerializeField] private GameObject StatsPanelHero;
    [SerializeField] private GameObject StatsPanelShared;
    [SerializeField] private UI_DisplayUnitsInGridHandler DisplayUnitRow;
    [SerializeField] private UI_DisplayUnitsInGridHandler DisplayHeroColumn;
    [SerializeField] private UI_DisplayItemsInGridHandler DisplayItems;
    [SerializeField] private UI_DisplayItemsInGridHandler DisplayInventoryItems;
    [SerializeField] private UI_DisplayItemsInGridHandler DisplayInventoryEquiptmentItems;
    [SerializeField] private UI_Inventory_Hero characterInventoryUI;
    [SerializeField] private UI_TooltipPopup tooltipPopup;
    [SerializeField] private UI_DragableIcon UI_DragableIconV2;


    // Start is called before the first frame update
    void Start()
    {
        DisplayItems.SetPlayerListener(player);
        DisplayItems.SetUIManager(this);
        DisplayItems.SetUI_TooltipPopup(tooltipPopup);
        DisplayItems.setMoveableIcon(UI_DragableIconV2);
        DisplayItems.Setup();

        DisplayInventoryItems.SetPlayerListener(player);
        DisplayInventoryItems.SetUIManager(this);
        DisplayInventoryItems.SetUI_TooltipPopup(tooltipPopup);
        DisplayInventoryItems.setMoveableIcon(UI_DragableIconV2);
        DisplayInventoryItems.Setup();

        DisplayInventoryEquiptmentItems.SetPlayerListener(player);
        DisplayInventoryEquiptmentItems.SetUIManager(this);
        DisplayInventoryEquiptmentItems.SetUI_TooltipPopup(tooltipPopup);
        DisplayInventoryEquiptmentItems.setMoveableIcon(UI_DragableIconV2);
        DisplayInventoryEquiptmentItems.Setup();

        DisplayHeroColumn.SetPlayerListener(player);
        DisplayHeroColumn.SetUI_TooltipPopup(tooltipPopup);

        DisplayUnitRow.SetPlayerListener(player);
        DisplayUnitRow.SetUI_TooltipPopup(tooltipPopup);

        UI_DragableIconV2.SetCam(player.getCamera());

        characterInventoryUI.Start();
    }

    #region CharacterInventoryUI

    public bool IsCharacterInventoryWindowActive()
    {
        return characterInventoryUI.gameObject.activeSelf;
    }

    public void CharacterInventoryWindowSetActive(bool onOroff)
    {
        characterInventoryUI.gameObject.SetActive(onOroff);
    }

    public void CharacterInventoryWindowUpdateDisplay(UnitController unit)
    {
        characterInventoryUI.UpdateDisplay(unit);
    }

    #endregion

    #region StatsPanel

    public void SwitchPanel(bool IsItem)
    {
        StatsPanelUnits.gameObject.SetActive(!IsItem);
        StatsPanelItems.gameObject.SetActive(IsItem);
        StatsPanelShared.SetActive(true);
    }

    public void ClearStats()
    {
        StatsPanelUnits.ClearStats();
        StatsPanelItems.ClearStats();
        StatsPanelShared.SetActive(false);
        StatsPanelHero.SetActive(false);
        StatsPanelUnits.gameObject.SetActive(false);
        StatsPanelItems.gameObject.SetActive(false);
    }

    public void StatsPanelItemsUpdateDisplay(I_Selectable selectedObject)
    {
        StatsPanelItems.UpdateDisplay(selectedObject);
    }

    public void StatsPanelUnitsUpdateDisplay(I_Selectable selectedObject)
    {
        StatsPanelUnits.UpdateDisplay(selectedObject);
    }

    #endregion

    #region DisplayItemsInGridHandler

    public void ClearSlots()
    {
        DisplayItems.ClearSlots();
        DisplayInventoryItems.ClearSlots();
        DisplayInventoryEquiptmentItems.ClearSlots();
    }

    public void DisplayItemsUpdateDisplay(UnitController unit)
    {
        DisplayItems.UpdateDisplay(unit);
        DisplayInventoryItems.UpdateDisplay(unit);
        DisplayInventoryEquiptmentItems.UpdateDisplay(unit);
    }

    public void DisplayHeroColumnAdd(UnitController unit,int slot)
    {
        DisplayHeroColumn.Add(unit, slot);
    }

    #endregion

    #region DisplayUnitsInGridHandler

    public void DeselectUnits()
    {
        DisplayUnitRow.DeselectUnits();
    }

    public void DisplayUnitRowAdd(UnitController unit,int slot)
    {
        DisplayUnitRow.Add(unit, slot);
    }

    public void DisplayUnitRowRemove(int slot)
    {
        DisplayUnitRow.Remove(slot);
    }

    #endregion

    #region General

    public I_Selectable GetdisplayedObject()
    {
        return displayedObject;
    }

    public void UpdateUI(I_Selectable obj, bool isPlayerUnit = false)
    {
        displayedObject = obj;
        displayedObject.SetSelected(true);

        if (obj.GetSelectableType() == SelectableTypes.Item)
        {
            SwitchPanel(true);
            StatsPanelItemsUpdateDisplay(displayedObject);
        }
        else
        {
            SwitchPanel(false);
            StatsPanelUnitsUpdateDisplay(displayedObject);
        }

        if (isPlayerUnit)
        {
            DisplayItemsUpdateDisplay((UnitController)obj);
        }
    }

    public void DisplayHideInventoryWindow()
    {
        if (IsCharacterInventoryWindowActive())
        {
            CharacterInventoryWindowSetActive(false);
        }
        else
        {
            CharacterInventoryWindowUpdateDisplay((UnitController)displayedObject);
            CharacterInventoryWindowSetActive(true);

        }
    }

    public void ClearDisplayedObject()
    {
        if (displayedObject != null)
        {
            ClearSlots();
            displayedObject.SetSelected(false);
            displayedObject = null;
        }
    }

    #endregion

}
