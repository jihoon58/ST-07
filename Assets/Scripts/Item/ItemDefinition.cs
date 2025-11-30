using UnityEngine;

namespace ST07.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("아이템 이름")]
        public string itemName = "New Item";
        
        [Tooltip("아이템 아이콘")]
        public Sprite icon;
        
        [Header("Weight")]
        [Tooltip("아이템 무게 (kg)")]
        public float weight = 0f;

        [Header("Stack")]
        [Tooltip("최대 스택 크기 (1 = 스택 불가)")]
        [Range(1, 99)]
        public int maxStack = 1;
    }
}

