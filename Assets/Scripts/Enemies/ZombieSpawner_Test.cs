using UnityEngine;

public class ZombieSpawner_Test : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    public GameObject walkerZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject biterZombiePrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // 생성 위치 (없으면 이 오브젝트 위치)

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

    void SpawnZombie(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("프리팹이 할당되지 않았습니다!");
            return;
        }

        // 생성 위치 결정
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        
        // 좀비 생성
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

