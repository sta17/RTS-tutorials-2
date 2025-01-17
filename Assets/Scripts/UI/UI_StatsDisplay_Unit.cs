using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StatsDisplay_Unit : MonoBehaviour, I_StatsDisplay
{
    [SerializeField]
    private SO_Unit_Data currentUnitStats;

    [SerializeField]
    private UnitController currentUnitController;

    public Image icon;

    public TextMeshProUGUI unitName;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI range;
    public TextMeshProUGUI attackSpeed;

    public void UpdateDisplay(I_Selectable selectedObject)
    {
        var newUnit = (UnitController) selectedObject;

        currentUnitController = newUnit;
        currentUnitStats = currentUnitController.getUnitStatsSO();

        icon.sprite = currentUnitStats.icon;
        icon.enabled = true;

        unitName.text = currentUnitStats.unitName.ToString();
        hp.text = currentUnitStats.unitMaxHealth.ToString();
        damage.text = currentUnitStats.attackDamage.ToString();
        range.text = currentUnitStats.attackRange.ToString();
        attackSpeed.text = currentUnitStats.attackSpeed.ToString();
    }

    public void Update()
    {
        if (currentUnitController != null)
        {
            hp.text = currentUnitController.getCurrentHealth().ToString() + " of " + currentUnitStats.unitMaxHealth.ToString();
        }
    }

    public void ClearStats()
    {
        if (currentUnitController != null)
        {
            currentUnitController = null;
            currentUnitStats = null;

            icon.sprite = null;
            icon.enabled = false;

            unitName.text = "";
            hp.text = "";
            damage.text = "";
            range.text = "";
            attackSpeed.text = "";
        }
    }
}
