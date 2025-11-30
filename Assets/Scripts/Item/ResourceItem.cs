using UnityEngine;
namespace ST07.Items
{
    public enum ItemType
    {
        Food,
        Scrap,
        Water,
        Battery,
        Fuel,
        ResearchData
    }
    [CreateAssetMenu(fileName = "New Resource Item", menuName = "Items/Resource Item")]
    public class ResourceItem : ItemDefinition
    {
        [Header("Resource Type")]
        public ItemType itemType;
    }
}
