using System;
using ST07.Items;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
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

        public void Start()
        {
            maxStack = 99;
        }
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
