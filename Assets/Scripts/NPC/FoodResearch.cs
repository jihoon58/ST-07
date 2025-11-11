using UnityEngine;
using ST07.Systems;

public class FoodResearch : MonoBehaviour
{
    [Header("Research State")]
    [Range(0, 100)]public float FoodResearchPercent; // 식품연구 진행도(%)
    public float FoodResearchPerDay = 8; // 하루당 식품연구진행도 상승률(%)

    [Header("Refs")]
    public TimeOfDaySystem timeSystem; // 시간 시스템

    public EndingManager endingManager; // 엔딩 매니저

    //연구 진행도 증가
    private void Update(){
        if(FoodResearchPercent >= 100 && !endingManager.isResearchEnd){
            FoodResearchPercent = 100; // clamp
            endingManager.ResearchEnding(); // 연구 엔딩 실행
            return; // 연구 100% 달성하면 반환  
        }
		FoodResearchPercent += FoodResearchPerDay / TimeOfDaySystem.dayLengthSeconds * Time.deltaTime; // 진행도 증가
    }
}
