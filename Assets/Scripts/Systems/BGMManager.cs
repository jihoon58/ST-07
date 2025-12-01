using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬별 배경음악을 자동으로 재생하고 관리하는 싱글톤 매니저
/// Unity Inspector에서 오디오 클립을 직접 할당하여 사용합니다.
/// </summary>
public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    
    [Header("배경음악 설정")]
    [Tooltip("Entry Scene에서 재생될 메인 테마")]
    public AudioClip mainThemeClip;
    
    [Tooltip("City 씬에서 재생될 음악")]
    public AudioClip cityClip;
    
    [Tooltip("InBuilding (Mart) 씬에서 재생될 음악")]
    public AudioClip martClip;
    
    [Tooltip("Bunker 씬에서 재생될 음악")]
    public AudioClip bunkerClip;

    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // AudioSource 컴포넌트 설정
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            audioSource.loop = true; // 반복 재생 설정
            audioSource.playOnAwake = false;
            
            // 씬 로드 이벤트 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // 현재 씬의 BGM 재생
        PlayBGMForCurrentScene();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// 씬이 로드될 때 호출되는 이벤트 핸들러
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    /// <summary>
    /// 현재 씬에 맞는 BGM 재생
    /// </summary>
    private void PlayBGMForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayBGMForScene(currentSceneName);
    }

    /// <summary>
    /// 지정된 씬 이름에 맞는 BGM 재생
    /// </summary>
    private void PlayBGMForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        // 씬 이름에 따라 재생할 오디오 클립 선택
        switch (sceneName)
        {
            case "Entry Scene":
                clipToPlay = mainThemeClip;
                break;
            case "City":
                clipToPlay = cityClip;
                break;
            case "InBuilding":
                clipToPlay = martClip;
                break;
            case "Bunker":
                clipToPlay = bunkerClip;
                break;
            default:
                // 매핑되지 않은 씬인 경우 오디오 정지
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                return;
        }

        // 오디오 클립이 할당되어 있는 경우에만 재생
        if (clipToPlay != null)
        {
            // 현재 재생 중인 오디오와 같은 경우 재생 유지
            if (audioSource.clip == clipToPlay && audioSource.isPlaying)
            {
                return;
            }

            audioSource.clip = clipToPlay;
            audioSource.Play();
            Debug.Log($"BGM 재생 시작: {clipToPlay.name} (씬: {sceneName})");
        }
        else
        {
            Debug.LogWarning($"씬 '{sceneName}'에 할당된 오디오 클립이 없습니다.");
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

