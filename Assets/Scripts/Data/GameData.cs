using System;
using System.Collections.Generic;
using UnityEngine;

// 안씀 삭제해애함 
// HERE

// GameData를 Data.Player.cs와 Data.Building.cs와 Data.Inventory.cs로 나누어서 관리할 예정
// HERE
namespace ST07.Data
{
    public class GameData
    {
        #region BuildingData
        [Serializable]
        public class BuildingTypeData{
            public String buildingType;
            public List<BuildingIndexData> buildingIndexList;
        }

        [Serializable]
        public class BuildingIndexData{
            public int buildingIndex;
            public List<BuildingItemData> itemDataList;
        }
        
        [Serializable]
        public class BuildingItemData
        {
            [Tooltip("아이템 이름")]
            public string itemName;
            
            [Tooltip("아이템 개수")]
            public int itemCount;
            
            [Tooltip("아이템 위치 (x, y)")]
            public Vector2 itemPosition;
        }

        [Serializable]
        public class BuildingData
        {
            public List<BuildingTypeData> buildingTypeList;

            public List<BuildingItemData> getBuildingItemData(String type, int index){
                foreach (var typeList in buildingTypeList){
                    if (typeList.buildingType == type){
                        foreach (var indexList in typeList.buildingIndexList){
                            if (indexList.buildingIndex == index){
                                return indexList.itemDataList;
                            }
                        }
                    }
                }
                return null;
            }
        
            // public void s
        }


        #endregion
    }
}
