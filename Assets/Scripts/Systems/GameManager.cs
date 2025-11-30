using UnityEngine;

/// <summary>
/// 게임 매니저
/// </summary>

// HERE
public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager instance;
    private void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}
    }
    #endregion

    
}
