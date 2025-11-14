using UnityEngine;
using UnityEngine.Events;

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

    [Header("Event")]
    public UnityEvent onClick;
    
    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            onClick?.Invoke();
        }
    }
}
