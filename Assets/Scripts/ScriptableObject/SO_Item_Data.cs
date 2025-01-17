using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "Items/Item")]
public class SO_Item_Data : ScriptableObject
{
    public string itemName;
    [TextArea(4,4)]
    public string itemDescriptions;
    public ItemPickupTypes type;
    public ItemSlotTypes slotType;
    public Sprite icon = null;
    public int itemAmount = 1;

    public Item data = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    [SerializeField] private string Name;
    [TextArea(15, 20)]
    [SerializeField] private string description;
    public int Id = -1;

    //[SerializeField] private ItemType type;

    [SerializeField] private SO_Rarity rarity;
    [SerializeField] private bool isUseable = false;
    [SerializeField] private string useText = "Fill In";

    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(SO_Item_Data item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        rarity = item.data.rarity;
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }

    public string ItemName { get { return Name; } }

    public string ColouredName
    {
        get
        {
            var rarityColour = Color.black;
            if (rarity != null)
            {
                rarityColour = rarity.TextColour;
            }
            string hexColour = ColorUtility.ToHtmlStringRGB(rarityColour);
            return $"<color=#{hexColour}>{Name}</color>";
        }
    }

    public string TooltipInfoText
    {
       get
       {
            StringBuilder builder = new StringBuilder();

            var rarityName = "Common";
            if (rarity != null)
            {
                rarityName = rarity.Name;
            }

            builder.Append(rarityName).AppendLine();
            builder.Append("Description").AppendLine();
            builder.Append(description).AppendLine();
            if (isUseable)
            {
                builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();

            }
            return builder.ToString();
        }
    }
}

[System.Serializable]
public class ItemBuff : I_Modifier
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}