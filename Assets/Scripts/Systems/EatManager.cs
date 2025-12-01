using UnityEngine;
using ST07.Systems;
public class EatManager : MonoBehaviour
{
    #region 싱글톤
    public static EatManager instance;
    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public int foodPerDay = 4;

    private void Start(){
        TimeOfDaySystem.instance.onNextDay.AddListener(EatFood);
    }

    public void EatFood(){
        if(PlayerPrefs.GetInt("FoodCount") >= foodPerDay){
            PlayerPrefs.SetInt("FoodCount", PlayerPrefs.GetInt("FoodCount") - foodPerDay);
        }else{
            PlayerPrefs.SetInt("FoodCount", 0);
            EndingManager.instance.StarvationEnding();
            return;
        }
    }

    public void NPCRecruited(){
        foodPerDay += 2;
    }


}
