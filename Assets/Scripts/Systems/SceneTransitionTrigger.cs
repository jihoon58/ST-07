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
    
    [Header("Trigger Settings")]
    [Tooltip("트리거 반응 태그")]
    private string triggerTag = "Player";
    
    [Tooltip("상호작용 키")]
    private KeyCode interactionKey = KeyCode.F;
    
    [Header("Visual Feedback")]
    [Tooltip("상호작용 가능할 때 표시할 UI 텍스트")]
    private string hintText = "F키를 눌러 이동";
    
    [Header("Building Settings")]
    [Tooltip("건물 타입 (home, CVS, Mart 등) - InBuilding 씬으로 이동할 때만 사용")]
    public string buildingType = "";
    
    [Tooltip("건물 인덱스 - InBuilding 씬으로 이동할 때만 사용")]
    public int buildingIndex = 0;

    private bool isPlayerInRange = false;
    
    
    /// <summary>
    /// 플레이어가 트리거 영역에 들어올 때 호출
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTag))
        {
            UIManager.instance.SetHintText(hintText);
            isPlayerInRange = true;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            TransitionToScene();
        }
    }
    
    /// <summary>
    /// 플레이어가 트리거 영역을 벗어날 때 호출
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTag))
        {
            UIManager.instance.FalseHintText();
            isPlayerInRange = false;
        }
    }
    
    /// <summary>
    /// 씬 전환
    /// </summary>
    private void TransitionToScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("SceneTransitionTrigger: targetSceneName이 설정되지 않았습니다!");
            return;
        }
        
        // InBuilding 씬으로 이동하는 경우 건물 정보 저장
        if (targetSceneName == "InBuilding")
        {
            if (!string.IsNullOrEmpty(buildingType))
            {
                PlayerPrefs.SetString("BuildingType", buildingType);
            }
            if (buildingIndex != 0)
            {
                PlayerPrefs.SetInt("BuildingIndex", buildingIndex);
            }
        }
        
        // Transition Scene을 거쳐서 전환
        PlayerPrefs.SetString("NextScene", targetSceneName);
        SceneManager.LoadScene("Transition Scene");
    }
}

