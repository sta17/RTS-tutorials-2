using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum PlayerServerTypes
{
    HumanHost,
    HumanClient
}

[Serializable]
public class PlayerManager : FactionManager
{
    #region variables

    [Header("Selection")]
    private RaycastHit hit;
    [SerializeField] private List<UnitController> selectedUnits = new List<UnitController>();
    [SerializeField] private bool isDragging = false;
    private Vector3 mousePositon;

    [Header("UI")]
    //[SerializeField] private ISelectable displayedObject;
    [SerializeField] private UIManager UIManager;
    private List<UnitController> characterDisplayList = new List<UnitController>();

    [Header("Misc")]
    [SerializeField] private Camera cam;
    //[SerializeField] private PlayerServerTypes ServerType = PlayerServerTypes.HumanHost;

    #endregion

    #region Constructors Start and Awake

    public void Start()
    {
        playerType = PlayerTypes.Human;
    }

    #endregion

    #region Mouse and Keyboard Actions

    void Update()
    {
        //Detect if mouse is down
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            LeftMouseButtonUP();
        }
        else if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            RightMouseButtonDown();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            IKeyButtonDown();

        }
    }

    private void LeftMouseButtonDown()
    {
        mousePositon = Input.mousePosition;
        //Create a ray from the camera to our space
        var camRay = cam.ScreenPointToRay(Input.mousePosition);

        //Shoot that ray and get the hit data
        if (!Helpers.PointerIsOverUI() && Physics.Raycast(camRay, out hit))
        {
            if (hit.transform.CompareTag("UI"))
            {
                return;
            }else if (hit.transform.CompareTag("Unit"))
            {
                SelectUnit(hit.transform.GetComponent<UnitController>());
            }
            else if (hit.transform.CompareTag("Item"))
            {
                SelectUnit(hit.transform.GetComponent<MAP_Interactable_Item>());
            }
            else
            {
                isDragging = true;
            }
        }
    }

    private void LeftMouseButtonUP()
    {
        if (isDragging)
        {
            ClearUI(fullClear: !Input.GetKey(KeyCode.LeftShift));
            foreach (var selectableObject in FindObjectsOfType<UnitController>())
            {
                if (IsWithinSelectionBounds(selectableObject.transform))
                {
                    if (selectableObject.transform.CompareTag("Unit"))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<UnitController>(), isMultiSelect: true);
                    }
                }
            }
            isDragging = false;
        }
    }

    private void RightMouseButtonDown()
    {
        var camRay = cam.ScreenPointToRay(Input.mousePosition);
        //Shoot that ray and get the hit data
        if (Physics.Raycast(camRay, out hit))
        {
            if (UIManager.GetdisplayedObject().getFaction() != null)
            {
                if (UIManager.GetdisplayedObject().getFaction().playerID == playerID)
                {
                    if (hit.transform.CompareTag("Ground"))
                    {
                        foreach (var selectableObj in selectedUnits)
                        {
                            selectableObj.MoveUnit(hit.point);
                        }
                    }
                    else if (hit.transform.CompareTag("Unit"))
                    {
                        if (CanAttack(hit.transform))
                        {
                            foreach (var selectableObj in selectedUnits)
                            {
                                selectableObj.SetNewTarget(hit.transform);
                            }
                        }
                    }
                    else if (hit.transform.CompareTag("Item"))
                    {
                        foreach (var selectableObj in selectedUnits)
                        {
                            selectableObj.SetNewTarget(hit.transform);
                        }
                    }
                }
            }
        }
    }

    private void IKeyButtonDown()
    {
        UIManager.DisplayHideInventoryWindow();
    }

    private bool CanAttack(Transform transform)
    {
        var unit = hit.transform.GetComponent<UnitController>();

        if (unit.getFaction().playerID == playerID)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    #endregion

    #region Selection

    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePositon, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.blue);
        }

    }

    private void AddUnitToSelectionList(UnitController unit)
    {
        selectedUnits.Add(unit);
        UIManager.DisplayUnitRowAdd(unit, selectedUnits.Count);
        unit.SetSelected(true);
    }

    private void RemoveUnitFromSelectionList(UnitController unit)
    {
        UIManager.DisplayUnitRowRemove(selectedUnits.Count);
        selectedUnits = selectedUnits.Where(UnitController => UnitController.getSelectionID() != unit.getSelectionID()).ToList();
        //selectedUnits.Remove(unit);
        unit.SetSelected(false);
    }

    private bool IsPlayerUnit(I_Selectable obj)
    {
        if (obj.getFaction() == null)
        {
            return false;
        }
        else if ((obj.GetSelectableType() == SelectableTypes.Item) | (obj.getFaction().playerID != playerID))
        {
            return false;
        }
        else if ((obj.GetSelectableType() == SelectableTypes.Unit) && (obj.getFaction().playerID == playerID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SelectUnit(I_Selectable obj, bool isShiftclick = false, bool isMultiSelect = false, bool updateUiOnly = false)
    {
        isShiftclick = Input.GetKey(KeyCode.LeftShift);
        if (isMultiSelect | isShiftclick)
        {
            if (IsPlayerUnit(obj))
            {
                if (obj.GetSelectedStatus())
                {
                    RemoveUnitFromSelectionList((UnitController)obj);
                }
                else
                {
                    AddUnitToSelectionList((UnitController)obj);
                    if (UIManager.GetdisplayedObject() == null)
                    {
                        UIManager.UpdateUI(obj, true);
                    }
                }
            }
            else if (isShiftclick)
            {
                ClearUI(fullClear: true);
                UIManager.UpdateUI(obj);
            }
        }
        else
        {
            if (!updateUiOnly)
            {
                ClearUI(fullClear: true);

                if (IsPlayerUnit(obj))
                {
                    var tempunit = (UnitController)obj;
                    AddUnitToSelectionList(tempunit);
                    UIManager.UpdateUI(obj, true);
                }
                else
                {
                    UIManager.UpdateUI(obj);
                }
            }
            else
            {
                UIManager.UpdateUI(obj);
            }
        }
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        var viewportBounds = ScreenHelper.GetViewportBounds(cam, mousePositon, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(transform.position));
    }

    #endregion

    #region UI Handling

    public void ClearUI(bool fullClear = false, bool clearMainSelecton = false)
    {
        if (fullClear)
        {
            UIManager.DeselectUnits();

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].SetSelected(false);
            }
            selectedUnits.Clear();
        }

        if (selectedUnits.Count == 0 | clearMainSelecton | fullClear)
        {
            UIManager.ClearDisplayedObject();
        }

        UIManager.ClearStats();
    }

    public void AddToHeroQuickDisplay(UnitController unit)
    {
        characterDisplayList.Add(unit);
        UIManager.DisplayHeroColumnAdd(unit, characterDisplayList.Count -1);
        characterDisplayList.Add(unit);
    }

    #endregion

    #region Change Notification

    public void RaiseItemChangeNotification(UnitController unit)
    {
        if ((I_Selectable)unit == UIManager.GetdisplayedObject())
        {
            UIManager.UpdateUI(unit, true);
        }
    }

    public new void RaiseDeathChangeNotification(UnitController unit)
    {
        if (selectedUnits.Contains(unit))
        {
            RemoveUnitFromSelectionList(unit);
        }

        if ((I_Selectable)unit == UIManager.GetdisplayedObject())
        {
            ClearUI(clearMainSelecton: true);
        }
    }

    #endregion

    #region misc

    public Camera getCamera()
    {
        return cam;
    }

#endregion

}
