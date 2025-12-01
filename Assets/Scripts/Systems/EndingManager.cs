using ST07.Player;
using ST07.Systems;
using UnityEngine;
using System.Threading;

/// <summary>
/// 엔딩 매니저
/// </summary>
public class EndingManager : MonoBehaviour
{
    #region 싱글톤
    public static EndingManager instance;
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

    [Header("Ending State")]
    public bool isResearchEnd = false; // 식품연구 엔딩 완료 여부
    public bool isWeaponEnd = false; // 무기연구 엔딩 완료 여부 (현재 미기획)
    public bool isDeadEnd = false; // 플레이어 사망 엔딩 완료 여부
    public bool doomsdayEnding = false; // 종말론자 엔딩 완료 여부
    public bool isStarvationEnd = false; // 식량부족 엔딩 완료 여부

    [Header("Research State")]
    [Range(0, 100)] public float FoodResearchPercent = 0; // 식품연구 진행도(%)
    public float FoodResearchPerDay = 8; // 하루당 식품연구진행도 상승률(%)

    private void Start(){
        PlayerStats.instance.onDead.AddListener(DeadEnding);
    }

    // 연구 진행도 증가
    private void Update(){
        // 엔딩 조건 검사
        if(FoodResearchPercent >= 100 && !isResearchEnd){
            ResearchEnding(); // 연구 엔딩 실행
            return;
        }
        FoodResearchPercent += FoodResearchPerDay / TimeOfDaySystem.dayLengthSeconds * Time.deltaTime; // 진행도 증가
    }

    // 식량부족 엔딩 실행
    public void StarvationEnding(){
        if(isStarvationEnd) return; // 식량부족 엔딩 완료하면 반환
        isStarvationEnd = true; // 식량부족 엔딩 완료

        // 애니메이션 실행 (식량부족 엔딩)
        // HERE
    }
    // 연구 엔딩 실행
    public void ResearchEnding(){ 
		if(isResearchEnd) return; // 연구 엔딩 완료하면 반환
		isResearchEnd = true; // 연구 엔딩 완료

		// 애니메이션 실행 (연구 100% 엔딩)
        // HERE
    }

    // 플레이어 사망 엔딩 실행
    public void DeadEnding(){ 
        if(isDeadEnd) return; // 사망 엔딩 완료하면 반환		
		isDeadEnd = true; // 플레이어 사망 엔딩 완료

        Thread.Sleep(3000); // 3초 후 엔딩 실행

		// 애니메이션 실행 (플레이어 사망 엔딩)
        // HERE
    }

    // 종말론자 엔딩 실행
    public void DoomsdayEnding(){
        if(doomsdayEnding) return; // 종말론자 엔딩 완료하면 반환
        doomsdayEnding = true; // 종말론자 엔딩 완료

        // 애니메이션 실행 (종말론자 엔딩)
        // HERE
    }
}
