using UnityEngine;

/// <summary>
/// 카메라가 플레이어를 부드럽게 따라다니는 스크립트
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("따라다닐 타겟 (비어있으면 자동으로 Player 찾기)")]
    public Transform target;
    
    [Header("Follow Settings")]
    [Tooltip("카메라 이동 속도")]
    public float followSpeed = 2f;
    
    [Tooltip("카메라 오프셋 (플레이어로부터의 거리)")]
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Tooltip("X축 이동 제한 (0이면 제한 없음)")]
    public float xLimit = 0f;
    
    [Tooltip("Y축 이동 제한 (0이면 제한 없음)")]
    public float yLimit = 0f;
    
    [Header("Smoothing")]
    [Tooltip("부드러운 이동 사용 여부")]
    public bool useSmoothing = true;
    
    private void Start()
    {
        // 타겟이 없으면 자동으로 찾기
        if (target == null)
        {
            var playerStats = FindFirstObjectByType<ST07.Player.PlayerStats>();
            if (playerStats != null)
            {
                target = playerStats.transform;
            }
            else
            {
                var playerController = FindFirstObjectByType<PlayerController_M>();
                if (playerController != null)
                {
                    target = playerController.transform;
                }
            }
        }
        
        // 초기 위치 설정
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;
            transform.position = new Vector3(targetPos.x, targetPos.y, offset.z);
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        
        // 제한 적용
        if (xLimit > 0f)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, -xLimit, xLimit);
        }
        if (yLimit > 0f)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, -yLimit, yLimit);
        }
        
        // Z축은 항상 offset.z 유지
        targetPosition.z = offset.z;
        
        if (useSmoothing)
        {
            // 부드러운 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            // 즉시 이동
            transform.position = targetPosition;
        }
    }
}

