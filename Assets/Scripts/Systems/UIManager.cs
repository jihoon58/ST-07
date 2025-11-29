using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Ref")]
    public GameObject mainCanvas;   
    public GameObject player;
    
    #region 싱글톤
    public static UIManager instance;
    private void Awake(){
        // 싱글톤
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    /// <summary>
    /// 시작 시 UI 비활성화
    /// </summary>
    private void Start(){
        // 시작화면에서 불필요한 요소 비활성화
        mainCanvas.SetActive(false);
        player.SetActive(false);

        // 게임에서 가지고 다닐 요소 DontDestroyOnLoad
        DontDestroyOnLoad(mainCanvas);
        DontDestroyOnLoad(player);
    }

    /// <summary>
    /// 씬 로드 시작 시 UI 비활성화
    /// </summary>
    public void OnLoadStart(){
        mainCanvas.SetActive(false);
        player.SetActive(false);
    }

    /// <summary>
    /// 씬 로드 완료 시 UI 활성화
    /// </summary>
    public void OnLoadEnd(){
        mainCanvas.SetActive(true);
        player.SetActive(true);
    }



}
