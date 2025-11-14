using UnityEngine;
using ST07.Player;
using ST07.Systems;

/// <summary>
/// 씬 초기화를 담당하는 스크립트
/// 플레이어, 시스템 오브젝트 등을 자동으로 찾아서 설정합니다.
/// </summary>
public class SceneInitializer : MonoBehaviour
{
    [Header("Auto Setup")]
    [Tooltip("씬 시작 시 자동으로 시스템을 초기화할지 여부")]
    public bool autoInitialize = true;
    
    [Header("Player Settings")]
    [Tooltip("플레이어 시작 위치 (비어있으면 현재 위치 사용)")]
    public Transform playerSpawnPoint;
    
    [Tooltip("플레이어를 찾지 못했을 때 경고만 표시할지 여부")]
    public bool warnOnlyIfPlayerNotFound = true;
    
    private void Start()
    {
        if (autoInitialize)
        {
            InitializeScene();
        }
    }
    
    /// <summary>
    /// 씬 초기화 실행
    /// </summary>
    public void InitializeScene()
    {
        SetupPlayer();
        SetupSystems();
    }
    
    /// <summary>
    /// 플레이어 설정
    /// </summary>
    private void SetupPlayer()
    {
        // 플레이어 찾기
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        
        if (player == null)
        {
            if (warnOnlyIfPlayerNotFound)
            {
                Debug.LogWarning("SceneInitializer: 플레이어를 찾을 수 없습니다. 씬에 Player 오브젝트가 있는지 확인하세요.");
            }
            else
            {
                Debug.LogError("SceneInitializer: 플레이어를 찾을 수 없습니다!");
            }
            return;
        }
        
        // 플레이어 시작 위치 설정
        if (playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }
        
        // 카메라 설정
        SetupCamera(player.transform);
    }
    
    /// <summary>
    /// 카메라 설정
    /// </summary>
    private void SetupCamera(Transform playerTransform)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        if (mainCamera != null)
        {
            CamMove camMove = mainCamera.GetComponent<CamMove>();
            if (camMove != null)
            {
                // CamMove가 자동으로 플레이어를 찾도록 설정되어 있을 수 있음
                // 필요시 여기서 target 설정
            }
        }
    }
    
    /// <summary>
    /// 시스템 오브젝트 확인 및 설정
    /// </summary>
    private void SetupSystems()
    {
        // TimeOfDaySystem 확인
        TimeOfDaySystem timeSystem = FindFirstObjectByType<TimeOfDaySystem>();
        if (timeSystem == null)
        {
            Debug.LogWarning("SceneInitializer: TimeOfDaySystem을 찾을 수 없습니다.");
        }
        
        // EndingManager 확인
        EndingManager endingManager = FindFirstObjectByType<EndingManager>();
        if (endingManager == null)
        {
            Debug.LogWarning("SceneInitializer: EndingManager를 찾을 수 없습니다.");
        }
        
        // GameManager 확인
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogWarning("SceneInitializer: GameManager를 찾을 수 없습니다.");
        }
    }
}

