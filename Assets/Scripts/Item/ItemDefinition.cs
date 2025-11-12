using UnityEngine;

namespace ST07.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("아이템 이름")]
        public string itemName = "New Item";
        
        [Tooltip("아이템 설명")]
        [TextArea(3, 5)]
        public string description = "";
        
        [Tooltip("아이템 아이콘")]
        public Sprite icon;
        
        [Header("Weight")]
        [Tooltip("아이템 무게 (kg)")]
        public float weight = 0f;

        [Header("Stack")]
        [Tooltip("최대 스택 크기 (1 = 스택 불가)")]
        [Min(1)]
        public int maxStack = 1;
    }
}

