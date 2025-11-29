using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Entry Scene (시작 화면) 관리 스크립트
/// </summary>
public class EntrySceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("게임 시작 시 로드할 첫 씬 이름")]
    public string firstSceneName;
    
    [Header("Objects")]
    [Tooltip("플레이어 상태 패널")]
    public GameObject playerStatusPanel;
    public GameObject mainCanvas;
    public GameObject inventoryPanel;
    public GameObject hintText;
    public GameObject player;
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    /// <summary>
    /// 게임에서 필요한 설정 초기화
    /// </summary>
    private void Awake(){
        // 시작화면에서 불필요한 요소 비활성화
        playerStatusPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        hintText.SetActive(false);
        player.SetActive(false);

        // 게임에서 가지고 다닐 요소 DontDestroyOnLoad
        DontDestroyOnLoad(mainCanvas);
        DontDestroyOnLoad(player);

        // 게임 시작 시 로드할 첫 씬 이름 설정
        firstSceneName = "Bunker";

        // 버튼 클릭 시 이벤트 추가
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }
    
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
        // 비활성화 시킨 요소 활성화
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        hintText.SetActive(true);
        player.SetActive(true);

        // 다음 씬 이름 설정
        PlayerPrefs.SetString("NextKey", firstSceneName);
        SceneManager.LoadScene("Transition Scene");
    }
    
    /// <summary>
    /// 설정 버튼 클릭 시 호출
    /// </summary>
    public void OpenSettings()
    {
        // 설정 메뉴 열기
        Debug.Log("설정 메뉴 열기");
    }
    
    /// <summary>
    /// 종료 버튼 클릭 시 호출
    /// </summary>
    public void QuitGame()
    {
        // 게임 종료
        Debug.Log("게임 종료");
    }
}

