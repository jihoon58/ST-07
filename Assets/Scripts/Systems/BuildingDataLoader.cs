using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ST07.Data;
using ST07.Items;
using ST07.World;

/// <summary>
/// InBuilding 씬에서 건물 데이터를 JSON 파일에서 로드하고 아이템을 배치하는 시스템
/// </summary>
public class BuildingDataLoader : MonoBehaviour
{
    [Header("JSON File Settings")]
    [Tooltip("JSON 파일 이름")]
    public string jsonFileName = "GameData.json";
    
    // [Header("Item Spawn Settings")]
    // [Tooltip("아이템을 배치할 부모 오브젝트 (비어있으면 씬 루트에 배치)")]
    // public Transform itemParent;
    
    [Tooltip("아이템 프리팹 (Lootable 컴포넌트가 있는 GameObject)")]
    public GameObject itemPrefab;
    
    private GameData.BuildingData loadedBuildingData;
    private List<GameData.BuildingItemData> currentItemList;
    
    private void Start()
    {
        LoadBuildingData();
    }
    
    /// <summary>
    /// 건물 데이터 로드 및 아이템 배치
    /// </summary>
    public void LoadBuildingData()
    {
        // PlayerPrefs에서 건물 정보 가져오기
        string buildingType = PlayerPrefs.GetString("BuildingType", "");
        int buildingIndex = PlayerPrefs.GetInt("BuildingIndex", 0);
        
        if (string.IsNullOrEmpty(buildingType) || buildingIndex == 0) return;  
        
        // JSON 파일 로드
        if (!LoadJsonFile()) return;
        
        // 해당 건물 데이터 찾기 (GameData.BuildingData의 getBuildingItemData 메서드 사용)
        currentItemList = loadedBuildingData.getBuildingItemData(buildingType, buildingIndex);
        
        if (currentItemList == null) return;
        
        // 아이템 배치
        SpawnItems();
    }
    
    private bool LoadJsonFile()
    {
        // persistentDataPath에서 JSON 파일 로드
        string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        
        if (!File.Exists(filePath)) return false;
        
        string jsonContent = File.ReadAllText(filePath);
        
        if (string.IsNullOrEmpty(jsonContent)) return false;
        
        loadedBuildingData = JsonUtility.FromJson<GameData.BuildingData>(jsonContent);
        
        if (loadedBuildingData == null) return false;
        
        return true;
    }
    
    /// <summary>
    /// 아이템들을 씬에 배치
    /// Lootable 프리팹을 Instantiate하고 Sprite와 ItemDefinition만 설정
    /// </summary>
    private void SpawnItems()
    {
        if (currentItemList == null || currentItemList.Count == 0) return;
        
        if (itemPrefab == null) return;
        
        foreach (var itemData in currentItemList)
        {
            // 유효성 검사
            if (string.IsNullOrEmpty(itemData.itemName) || itemData.itemCount <= 0) continue;
            
            // 아이템 정의 찾기 (Resources 폴더에서)
            ItemDefinition itemDef = FindItemDefinition(itemData.itemName);
            if (itemDef == null)
            {
                continue;
            }
            
            // 프리팹 Instantiate
            GameObject itemObj = Instantiate(itemPrefab, itemData.itemPosition, Quaternion.identity);
            // itemObj.name = $"Lootable_{itemData.itemName}_{itemData.itemCount}";
            
            // Sprite 설정: ItemDefinition의 icon을 SpriteRenderer에 설정
            SpriteRenderer spriteRenderer = itemObj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && itemDef.icon != null)
            {
                spriteRenderer.sprite = itemDef.icon;
            }
            
            // Lootable 컴포넌트 가져오기 또는 추가
            Lootable lootable = itemObj.GetComponent<Lootable>();
            if (lootable == null)
            {
                lootable = itemObj.AddComponent<Lootable>();
            }
            

            //lootable 보고 난 다음 읽기
            //HERE
            // ItemDefinition 설정: Lootable의 contents에 아이템 정보 설정
            lootable.contents.Clear();
            var lootEntry = new Lootable.LootEntry
            {
                item = itemDef,
                quantity = itemData.itemCount
            };
            lootable.contents.Add(lootEntry);
        }
    }
    
    /// <summary>
    /// 아이템 이름으로 ItemDefinition 찾기
    /// </summary>
    private ItemDefinition FindItemDefinition(string itemName)
    {
        // Resources/Items 폴더에서 모든 ItemDefinition 로드
        ItemDefinition[] allItems = Resources.LoadAll<ItemDefinition>("Items");
        
        foreach (var item in allItems)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        
        return null;
    }
}

