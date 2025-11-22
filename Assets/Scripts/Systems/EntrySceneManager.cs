using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Entry Scene (시작 화면) 관리 스크립트
/// </summary>
public class EntrySceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("게임 시작 시 로드할 첫 씬 이름")]
    public string firstSceneName = "Bunker";
    
    [Tooltip("Transition Scene을 거쳐서 전환할지 여부")]
    public bool useTransitionScene = true;
    
    /// <summary>
    /// 게임 시작 버튼 클릭 시 호출
    /// </summary>
    public void StartGame()
    {
        if (string.IsNullOrEmpty(firstSceneName))
        {
            Debug.LogError("EntrySceneManager: firstSceneName이 설정되지 않았습니다!");
            return;
        }
        
        if (useTransitionScene)
        {
            // Transition Scene을 거쳐서 전환
            PlayerPrefs.SetString("NextKey", firstSceneName);
            SceneManager.LoadScene("Transition Scene");
        }
        else
        {
            // 직접 전환
            SceneManager.LoadScene(firstSceneName);
        }
    }
    
    /// <summary>
    /// 설정 버튼 클릭 시 호출 (선택사항)
    /// </summary>
    public void OpenSettings()
    {
        // 설정 UI 표시 (추후 구현)
        Debug.Log("설정 메뉴 열기");
    }
    
    /// <summary>
    /// 종료 버튼 클릭 시 호출
    /// </summary>
    public void QuitGame()
    {
        // 종료 처리 (추후 구현)
        Debug.Log("게임 종료");
    }
}

