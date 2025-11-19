using UnityEngine;
using UnityEditor;
using System.IO;
using ST07.Data;
using ST07.World;
using ST07.Items;

public class BuildingDataExporter : EditorWindow
{
    private string buildingType = "home";
    private string buildingIndex = "1";
    private Transform itemParent;

    [MenuItem("Tools/Building Data Exporter")]
    public static void ShowWindow()
    {
        GetWindow<BuildingDataExporter>("Building Data Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("건물 데이터 JSON 내보내기", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        buildingType = EditorGUILayout.TextField("Building Type", buildingType);
        buildingIndex = EditorGUILayout.TextField("Building Index", buildingIndex);
        itemParent = (Transform)EditorGUILayout.ObjectField("Item Parent", itemParent, typeof(Transform), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("현재 씬의 아이템들을 JSON으로 저장"))
        {
            ExportToJSON();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("씬에 배치된 Lootable 오브젝트들을 찾아서 JSON 파일로 저장합니다.", MessageType.Info);
    }

    private void ExportToJSON()
    {
        // 기존 JSON 파일 로드 (있으면)
        HouseData houseData = new HouseData();
        string filePath = Path.Combine(Application.persistentDataPath, "BuildingData.json");

        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            houseData = JsonUtility.FromJson<HouseData>(existingJson);
            if (houseData == null)
            {
                houseData = new HouseData();
            }
        }

        // 씬에서 Lootable 오브젝트 찾기
        Lootable[] lootables;
        if (itemParent != null)
        {
            lootables = itemParent.GetComponentsInChildren<Lootable>();
        }
        else
        {
            lootables = FindObjectsOfType<Lootable>();
        }

        // HouseIndexData 생성
        HouseIndexData buildingData = new HouseIndexData();
        buildingData.houseIndex = buildingIndex;

        foreach (var lootable in lootables)
        {
            foreach (var entry in lootable.contents)
            {
                if (entry.item != null)
                {
                    HouseItemData itemData = new HouseItemData
                    {
                        ItemName = entry.item.itemName,
                        ItemCount = entry.quantity,
                        ItemPosition = lootable.transform.position
                    };
                    buildingData.houseItem.Add(itemData);
                }
            }
        }

        // 해당 건물 타입의 데이터에 추가/업데이트
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
        }

        if (typeData != null)
        {
            // 기존 건물 데이터가 있으면 제거
            typeData.buildings.RemoveAll(b => b.houseIndex == buildingIndex);
            // 새 데이터 추가
            typeData.buildings.Add(buildingData);
        }

        // JSON으로 저장
        string json = JsonUtility.ToJson(houseData, true);

        // persistentDataPath 폴더가 없으면 생성
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        File.WriteAllText(filePath, json);

        Debug.Log($"건물 데이터 저장 완료: {filePath}");
        Debug.Log($"저장된 아이템 개수: {buildingData.houseItem.Count}");

        EditorUtility.DisplayDialog("저장 완료", $"JSON 파일이 저장되었습니다.\n경로: {filePath}", "확인");
    }
}