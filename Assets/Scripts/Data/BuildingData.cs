using System;
using System.Collections.Generic;
using UnityEngine;

namespace ST07.Data
{
    /// <summary>
    /// 건물 내부 아이템 위치 정보
    /// </summary>
    [Serializable]
    public class HouseItemData
    {
        [Tooltip("아이템 이름")]
        public string ItemName;
        
        [Tooltip("아이템 개수")]
        public int ItemCount;
        
        [Tooltip("아이템 위치 (x, y)")]
        public Vector2 ItemPosition;
    }

    /// <summary>
    /// 건물 인덱스와 해당 건물의 아이템들
    /// </summary>
    [Serializable]
    public class HouseIndexData
    {
        [Tooltip("건물 인덱스")]
        public string houseIndex;
        
        [Tooltip("건물 내부 아이템 리스트")]
        public List<HouseItemData> houseItem = new List<HouseItemData>();
    }

    /// <summary>
    /// 건물 타입별 데이터 (home, CVS, Mart 등)
    /// </summary>
    [Serializable]
    public class BuildingTypeData
    {
        [Tooltip("건물 인덱스별 데이터 리스트")]
        public List<HouseIndexData> buildings = new List<HouseIndexData>();
    }

    /// <summary>
    /// 전체 건물 데이터 (JSON 파일 구조)
    /// </summary>
    [Serializable]
    public class HouseData
    {
        [Tooltip("집 데이터")]
        public BuildingTypeData home = new BuildingTypeData();
        
        [Tooltip("편의점 데이터")]
        public BuildingTypeData CVS = new BuildingTypeData();
        
        [Tooltip("마트 데이터")]
        public BuildingTypeData Mart = new BuildingTypeData();
    }

    /// <summary>
    /// 특정 건물의 데이터를 찾기 위한 헬퍼 클래스
    /// </summary>
    public static class BuildingDataHelper
    {
        /// <summary>
        /// 건물 타입과 인덱스로 해당 건물 데이터 찾기
        /// </summary>
        public static HouseIndexData FindBuildingData(HouseData houseData, string buildingType, string buildingIndex)
        {
            BuildingTypeData typeData = null;
            
            switch (buildingType.ToLower())
            {
                case "home":
                    typeData = houseData.home;
                    break;
                case "cvs":
                    typeData = houseData.CVS;
                    break;
                case "mart":
                    typeData = houseData.Mart;
                    break;
                default:
                    Debug.LogWarning($"알 수 없는 건물 타입: {buildingType}");
                    return null;
            }

            if (typeData == null || typeData.buildings == null)
            {
                return null;
            }

            foreach (var building in typeData.buildings)
            {
                if (building.houseIndex == buildingIndex)
                {
                    return building;
                }
            }

            return null;
        }
    }
}

