using System;
using System.Collections.Generic;
using UnityEngine;

namespace ST07.Data
{
    /// <summary>
    /// 건물 내부 아이템 위치 정보
    /// </summary>
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

    /// <summary>
    /// 건물 인덱스와 해당 건물의 아이템들
    /// </summary>
    [Serializable]
    public class BuildingIndexData
    {
        [Tooltip("건물 인덱스")]
        public int buildingIndex;
    }

    /// <summary>
    /// 건물 타입별 데이터 (home, CVS, Mart 등)
    /// </summary>
    [Serializable]
    public class BuildingTypeData
    {
        [Tooltip("건물 타입")]
        public string buildingType;
    }

    /// <summary>
    /// 전체 건물 데이터 (JSON 파일 구조)
    /// </summary>
    [Serializable]
    public class BuildingData
    {
        [Header("건물 데이터")]
        public BuildingTypeData typeData;
        public BuildingIndexData indexData;
        public BuildingItemData[] itemData;
    }

    /// <summary>
    /// 특정 건물의 데이터를 찾기 위한 헬퍼 클래스
    /// </summary>
    /// // 필요성 못느낌
    // public static class BuildingDataHelper
    // {
    //     /// <summary>
    //     /// 건물 타입과 인덱스로 해당 건물 데이터 찾기
    //     /// </summary>
    //     public static BuildingIndexData FindBuildingData(BuildingData houseData, string buildingType, string buildingIndex)
    //     {
    //         BuildingTypeData typeData = null;
            
    //         switch (buildingType.ToLower())
    //         {
    //             case "home":
    //                 typeData = houseData.home;
    //                 break;
    //             case "cvs":
    //                 typeData = houseData.CVS;
    //                 break;
    //             case "mart":
    //                 typeData = houseData.Mart;
    //                 break;
    //             default:
    //                 Debug.LogWarning($"알 수 없는 건물 타입: {buildingType}");
    //                 return null;
    //         }

    //         if (typeData == null || typeData.buildings == null)
    //         {
    //             return null;
    //         }

    //         foreach (var building in typeData.buildings)
    //         {
    //             if (building.buildingIndex == buildingIndex)
    //             {
    //                 return building;
    //             }
    //         }

    //         return null;
    //     }
    // }
}

