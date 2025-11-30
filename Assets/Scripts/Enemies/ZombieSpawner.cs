using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    public GameObject walkerZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject biterZombiePrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // 생성 위치 (없으면 이 오브젝트 위치)
    
    [Header("Auto Spawn Settings")]
    public bool autoSpawn = true; // 자동 생성 활성화
    public float minSpawnInterval = 3f; // 최소 생성 간격 (초)
    public float maxSpawnInterval = 8f; // 최대 생성 간격 (초)

    void Start()
    {
        // 자동 생성 시작
        if (autoSpawn)
        {
            StartCoroutine(AutoSpawnZombies());
        }
    }

    // 수동 생성 (주석 처리)
    /*
    void Update()
    {
        // 1번 키: 워커좀비 생성
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            SpawnZombie(walkerZombiePrefab);
        }
        
        // 2번 키: 러너좀비 생성
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            SpawnZombie(runnerZombiePrefab);
        }
        
        // 3번 키: 바이터좀비 생성
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            SpawnZombie(biterZombiePrefab);
        }
    }
    */

    // 자동 좀비 생성 코루틴
    private IEnumerator AutoSpawnZombies()
    {
        while (autoSpawn)
       {
            // 랜덤 시간 대기
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // 랜덤으로 좀비 종류 선택
            GameObject prefabToSpawn = GetRandomZombiePrefab();
            
            // 좀비 생성
            SpawnZombie(prefabToSpawn);
       }
    }

    // 랜덤 좀비 프리팹 선택
    GameObject GetRandomZombiePrefab()
    {
        int randomIndex = Random.Range(0, 3);
        
        switch (randomIndex)
        {
            case 0:
                return walkerZombiePrefab;
            case 1:
                return runnerZombiePrefab;
            case 2:
                return biterZombiePrefab;
            default:
                return walkerZombiePrefab;
        }
    }

    void SpawnZombie(GameObject prefab)
    {
        if (prefab == null)
        {
            //Debug.LogWarning("프리팹이 할당되지 않았습니다!");
            return;
        }

        // 생성 위치 결정
        Vector3 spawnPos = spawnPoint.position;
        
        // 좀비 생성
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

