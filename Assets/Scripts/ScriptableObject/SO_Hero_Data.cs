using UnityEngine;

[CreateAssetMenu(menuName = "Unit/HeroStats")]
public class SO_Hero_Data : SO_Unit_Data
{
    public string heroTitle;

    public AttributeAmount[] Attributes;

    public AttributeAmount[] AttributesGainPerLevel;

    public SO_Hero_Data()
    {
        usePersonalEquipment = true;
        usesMana = true;
        displayCharacterInUI = true;
    }
}
