using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 트리거. 플레이어가 이 오브젝트에 접촉하면 지정된 씬으로 전환합니다.
/// </summary>
public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("전환할 씬 이름")]
    public string targetSceneName;
    
    [Tooltip("Transition Scene을 거쳐서 전환할지 여부")]
    public bool useTransitionScene = true;
    
    [Header("Trigger Settings")]
    [Tooltip("트리거 반응 태그")]
    public string triggerTag = "Player";
    
    [Tooltip("상호작용 키 입력이 필요한지 여부")]
    public bool requiresInteraction = false;
    
    [Tooltip("상호작용 키 (기본: E)")]
    public KeyCode interactionKey = KeyCode.E;
    
    [Header("Visual Feedback")]
    [Tooltip("상호작용 가능할 때 표시할 UI 텍스트")]
    public string interactionPrompt = "E키를 눌러 이동";
    
    private bool isPlayerInRange = false;
    private GameObject playerObject;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTag))
        {
            isPlayerInRange = true;
            playerObject = collision.gameObject;
            
            if (!requiresInteraction)
            {
                // 즉시 전환
                TransitionToScene();
            }
            else
            {
                // 상호작용 UI 표시 (추후 구현 가능)
                Debug.Log(interactionPrompt);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTag))
        {
            isPlayerInRange = false;
            playerObject = null;
        }
    }
    
    private void Update()
    {
        if (requiresInteraction && isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            TransitionToScene();
        }
    }
    
    private void TransitionToScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("SceneTransitionTrigger: targetSceneName이 설정되지 않았습니다!");
            return;
        }
        
        if (useTransitionScene)
        {
            // Transition Scene을 거쳐서 전환
            PlayerPrefs.SetString("NextKey", targetSceneName);
            SceneManager.LoadScene("Transition Scene");
        }
        else
        {
            // 직접 전환
            SceneManager.LoadScene(targetSceneName);
        }
    }
    
    private void OnDrawGizmos()
    {
        // 에디터에서 트리거 영역 시각화
        Gizmos.color = Color.yellow;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            if (col is BoxCollider2D boxCol)
            {
                Gizmos.DrawWireCube(transform.position + (Vector3)boxCol.offset, boxCol.size);
            }
            else if (col is CircleCollider2D circleCol)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)circleCol.offset, circleCol.radius);
            }
        }
    }
}

