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

        [Header("Prefab")]

        [Tooltip("아이템 프리팹")]
        public GameObject itemPrefab;
        public GameObject homeBgPrefab;
        public GameObject CVSBgPrefab;
        public GameObject martBgPrefab;

        private Building.BuildingData data;
        private List<Building.Item> itemList;
        private List<FieldItem> fieldItemList;
        private void Start()
        {
            fieldItemList = new List<FieldItem>();
            LoadJsonFile();
            LoadBG();
            SpawnItems();
        }

        private void LoadJsonFile(){
            // json 파일 경로 설정
            string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
            if (!File.Exists(filePath)) {
                Debug.LogError($"JSON 파일을 찾을 수 없습니다: {filePath}");
                return;
            }
            // json 파일 읽기
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(json)) {
                Debug.LogError("JSON 파일이 비어있습니다.");
                return;
            }
            // json 파일 파싱
            data = JsonUtility.FromJson<Building.BuildingData>(json);
            if (data == null) {
                Debug.LogError("JSON 파일을 파싱할 수 없습니다.");
                return;
            }
        }

        private void SpawnItems(){
            // 건물 정보 가져오기
            string type = PlayerPrefs.GetString("BuildingType", "");
            int index = PlayerPrefs.GetInt("BuildingIndex", 0);
            if (string.IsNullOrEmpty(type) || index == 0) {
                Debug.LogError($"건물 정보가 없습니다. type: {type}, index: {index}");
                return;
            }
            // 건물 아이템 가져오기
            itemList = data.getItemList(type, index);
            // 건물 아이템 생성
            foreach (var item in itemList) {
                fieldItemList.Add(Instantiate(itemPrefab, new Vector3(item.itemPos.x, item.itemPos.y, 0), Quaternion.identity).GetComponent<FieldItem>());
                fieldItemList[fieldItemList.Count - 1].set(item.itemName, item.itemCount);
            }
        }
    
        private void LoadBG(){
            string type = PlayerPrefs.GetString("BuildingType", "");
            switch (type) {
                case "home":
                    Instantiate(homeBgPrefab, Vector3.zero, Quaternion.identity);
                    break;
                case "CVS":
                    Instantiate(CVSBgPrefab, Vector3.zero, Quaternion.identity);
                    break;
                case "mart":
                    Instantiate(martBgPrefab, Vector3.zero, Quaternion.identity);
                    break;
            }
        }
    
    }
}