using UnityEngine;
using System.Collections.Generic;
using ST07.Data;
using System.IO;
using ST07.World;

namespace ST07.Systems{
    /// <summary>
    /// InBuilding 씬에서 건물 데이터를 JSON 파일에서 로드하고 아이템을 배치하는 시스템
    /// </summary>
    public class BuildingLoad : MonoBehaviour
    {
        [Header("JSON File Settings")]
        [Tooltip("JSON 파일 이름")]
        public string jsonFileName = "BuildingData.json";

        [Tooltip("아이템 프리팹")]
        public GameObject itemPrefab;

        private Building.BuildingData data;
        private List<Building.Item> itemList;
        private List<FieldItem> fieldItemList;

        private void Start()
        {
            fieldItemList = new List<FieldItem>();
            LoadJsonFile();
            SpawnItems();
        }

        private void LoadJsonFile(){
            string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
            if (!File.Exists(filePath)) {
                Debug.LogError($"JSON 파일을 찾을 수 없습니다: {filePath}");
                return;
            }
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(json)) {
                Debug.LogError("JSON 파일이 비어있습니다.");
                return;
            }
            data = JsonUtility.FromJson<Building.BuildingData>(json);
            if (data == null) {
                Debug.LogError("JSON 파일을 파싱할 수 없습니다.");
                return;
            }
        }

        private void SpawnItems(){
            string type = PlayerPrefs.GetString("BuildingType", "");
            int index = PlayerPrefs.GetInt("BuildingIndex", 0);
            if (string.IsNullOrEmpty(type) || index == 0) {
                Debug.LogError($"건물 정보가 없습니다. type: {type}, index: {index}");
                return;
            }
            itemList = data.getItemList(type, index);
            foreach (var item in itemList) {
                fieldItemList.Add(Instantiate(itemPrefab, new Vector3(item.itemPos.x, item.itemPos.y, 0), Quaternion.identity).GetComponent<FieldItem>());
                fieldItemList[fieldItemList.Count - 1].set(item.itemName, item.itemCount);
            }
        }
    }
}