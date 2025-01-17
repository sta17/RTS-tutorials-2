using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Unit/UnitStats")]
public class SO_Unit_Data : ScriptableObject
{
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float armor;
    public float manaMax;
    public float manaregen;
    public float unitMaxHealth;
    public float hpregen;
    [SerializeField] protected bool usePersonalEquipment = false;
    [SerializeField] protected bool displayCharacterInUI = false;
    public bool usesMana = false;
    public float mainAttackCooldown;

    public Sprite icon;
    public string unitName;
    public string unitTooltip;

    public string ColouredName
    {
        get
        {
            string hexColour = ColorUtility.ToHtmlStringRGB(Color.black);
            return $"<color=#{hexColour}>{unitName}</color>";
        }
    }

    public string TooltipInfoText
    {
        get
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Description").AppendLine();
            builder.Append(unitTooltip).AppendLine();
            return builder.ToString();
        }
    }

    public bool getUsePersonalEquipment {
        get {
            return usePersonalEquipment;
        }
    }

    public bool getDisplayCharacterInUI {
        get {
            return displayCharacterInUI;
        }
    }

}