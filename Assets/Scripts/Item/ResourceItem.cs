using ST07.Items;
using ST07.Player;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // public List<String> Description = new List<String>
    // {
    //     "음식입니다",
    //     "스크랩입니다",
    //     "물입니다",
    //     "배터리입니다",
    //     "연료입니다",
    //     "연구자료입니다"
    // };

    [CreateAssetMenu(fileName = "New Resource Item", menuName = "Items/Resource Item")]
    public class ResourceItem : ItemDefinition
    {
        [Header("Resource Type")]
        public ItemType itemType;


        // ScriptableObject 에서는 Start 대신 OnEnable 이 더 안전함
        private void OnEnable()
        {
            // 인스팩터 창에서 설정 안 해 둔 애들만 기본값 99로
            if (maxStack <= 0)
                maxStack = 99;
        }

        


        // public void ApplyEffect(PlayerStats player)
        // {
        //     if (player == null) return;

        //     switch (itemType)
        //     {
        //         case ItemType.Food:
        //             // Food 타입이면 체력 회복
        //             player.Heal(healAmount);
        //             break;

        //             // 나중에 다른 타입들도 여기서 처리 가능
        //             // case ItemType.Water:
        //             //     player.Drink(thirstRecover);
        //             //     break;
        //     }
        // }

        // public string GetDescription()
        // {
        //     switch (itemType)
        //     {
        //         case ItemType.Food:
        //             return Description[0];
        //         case ItemType.Scrap:
        //             return Description[1];
        //         case ItemType.Water:
        //             return Description[2];
        //         case ItemType.Battery:
        //             return Description[3];
        //         case ItemType.Fuel:
        //             return Description[4];
        //         case ItemType.ResearchData:
        //             return Description[5];
        //     }
        //     return "";
        // }
    }
}
