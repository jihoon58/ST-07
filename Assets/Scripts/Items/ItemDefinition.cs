using UnityEngine;

namespace ST07.Items
{
    public enum ItemType
    {
        Food,
        Scrap,
        WaterBottle,
        Battery,
        Fuel,
        ResearchData,
        Weapon,
        Other
    }

    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "ST07/Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        public string itemId = System.Guid.NewGuid().ToString();
        public string displayName = "Item";
        public ItemType itemType = ItemType.Other;
        [Min(0f)] public float weight = 1f;
        [Min(1)] public int maxStack = 1;
        public Sprite icon;
    }
}



