using System.IO;
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
    [Tooltip("JSON 파일 이름 (Resources 폴더 또는 persistentDataPath에 있어야 함)")]
    public string jsonFileName = "BuildingData.json";
    
    [Tooltip("JSON 파일이 Resources 폴더에 있는지 여부 (false면 persistentDataPath에서 로드)")]
    public bool loadFromResources = false;
    
    [Header("Item Spawn Settings")]
    [Tooltip("아이템을 배치할 부모 오브젝트 (비어있으면 씬 루트에 배치)")]
    public Transform itemParent;
    
    [Tooltip("아이템 프리팹 (Lootable 컴포넌트가 있는 GameObject)")]
    public GameObject itemPrefab;
    
    [Header("Debug")]
    [Tooltip("로드된 데이터를 콘솔에 출력할지 여부")]
    public bool debugLog = true;
    
    private HouseData loadedHouseData;
    private HouseIndexData currentBuildingData;
    
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
        string buildingIndex = PlayerPrefs.GetString("BuildingIndex", "");
        
        if (string.IsNullOrEmpty(buildingType) || string.IsNullOrEmpty(buildingIndex))
        {
            Debug.LogWarning("BuildingDataLoader: 건물 타입 또는 인덱스가 설정되지 않았습니다. PlayerPrefs를 확인하세요.");
            return;
        }
        
        if (debugLog)
        {
            Debug.Log($"BuildingDataLoader: 건물 타입={buildingType}, 인덱스={buildingIndex} 로드 시도");
        }
        
        // JSON 파일 로드
        if (!LoadJsonFile())
        {
            Debug.LogError("BuildingDataLoader: JSON 파일 로드 실패");
            return;
        }
        
        // 해당 건물 데이터 찾기
        currentBuildingData = BuildingDataHelper.FindBuildingData(loadedHouseData, buildingType, buildingIndex);
        
        if (currentBuildingData == null)
        {
            Debug.LogWarning($"BuildingDataLoader: 건물 데이터를 찾을 수 없습니다. 타입={buildingType}, 인덱스={buildingIndex}");
            return;
        }
        
        if (debugLog)
        {
            Debug.Log($"BuildingDataLoader: 건물 데이터 찾음. 아이템 개수={currentBuildingData.houseItem.Count}");
        }
        
        // 아이템 배치
        SpawnItems();
    }
    
    /// <summary>
    /// JSON 파일 로드
    /// </summary>
    private bool LoadJsonFile()
    {
        string jsonContent = "";
        
        if (loadFromResources)
        {
            // Resources 폴더에서 로드
            TextAsset jsonFile = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(jsonFileName));
            if (jsonFile == null)
            {
                Debug.LogError($"BuildingDataLoader: Resources 폴더에서 {jsonFileName} 파일을 찾을 수 없습니다.");
                return false;
            }
            jsonContent = jsonFile.text;
        }
        else
        {
            // persistentDataPath에서 로드
            string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
            if (!File.Exists(filePath))
            {
                Debug.LogError($"BuildingDataLoader: {filePath} 파일이 존재하지 않습니다.");
                return false;
            }
            jsonContent = File.ReadAllText(filePath);
        }
        
        if (string.IsNullOrEmpty(jsonContent))
        {
            Debug.LogError("BuildingDataLoader: JSON 파일 내용이 비어있습니다.");
            return false;
        }
        
        try
        {
            // JSON 파싱
            loadedHouseData = JsonUtility.FromJson<HouseData>(jsonContent);
            if (loadedHouseData == null)
            {
                Debug.LogError("BuildingDataLoader: JSON 파싱 실패");
                return false;
            }
            
            if (debugLog)
            {
                Debug.Log("BuildingDataLoader: JSON 파일 로드 성공");
            }
            
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"BuildingDataLoader: JSON 파싱 중 오류 발생: {e.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// 아이템들을 씬에 배치
    /// </summary>
    private void SpawnItems()
    {
        if (currentBuildingData == null || currentBuildingData.houseItem == null)
        {
            return;
        }
        
        if (itemPrefab == null)
        {
            Debug.LogError("BuildingDataLoader: itemPrefab이 설정되지 않았습니다!");
            return;
        }
        
        foreach (var itemData in currentBuildingData.houseItem)
        {
            if (string.IsNullOrEmpty(itemData.ItemName) || itemData.ItemCount <= 0)
            {
                continue;
            }
            
            // 아이템 정의 찾기 (Resources 폴더에서)
            ItemDefinition itemDef = FindItemDefinition(itemData.ItemName);
            if (itemDef == null)
            {
                Debug.LogWarning($"BuildingDataLoader: 아이템 '{itemData.ItemName}'을 찾을 수 없습니다.");
                continue;
            }
            
            // 아이템 오브젝트 생성
            GameObject itemObj = Instantiate(itemPrefab, itemParent);
            itemObj.transform.position = itemData.ItemPosition;
            itemObj.name = $"Lootable_{itemData.ItemName}_{itemData.ItemCount}";
            
            // Lootable 컴포넌트 설정
            Lootable lootable = itemObj.GetComponent<Lootable>();
            if (lootable == null)
            {
                lootable = itemObj.AddComponent<Lootable>();
            }
            
            // 아이템 내용 설정
            lootable.contents.Clear();
            var lootEntry = new Lootable.LootEntry
            {
                item = itemDef,
                quantity = itemData.ItemCount
            };
            lootable.contents.Add(lootEntry);
            
            if (debugLog)
            {
                Debug.Log($"BuildingDataLoader: 아이템 배치 - {itemData.ItemName} x{itemData.ItemCount} at {itemData.ItemPosition}");
            }
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
    
    /// <summary>
    /// 수동으로 건물 데이터 다시 로드 (에디터에서 테스트용)
    /// </summary>
    [ContextMenu("Reload Building Data")]
    public void ReloadBuildingData()
    {
        // 기존 아이템 제거
        if (itemParent != null)
        {
            for (int i = itemParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(itemParent.GetChild(i).gameObject);
            }
        }
        
        LoadBuildingData();
    }
}

