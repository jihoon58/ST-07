using ST07.Systems;
using ST07.Player;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;

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
    public bool isWeaponEnd = false; // 무기연구 엔딩 완료 여부 (현재 미구현)
    public bool isDeadEnd = false; // 플레이어 사망 엔딩 완료 여부
    public bool doomsdayEnding = false; // 종말론자 엔딩 완료 여부

    [Header("Refs")]
    public PlayerStats player; // 플레이어 상태. 이벤트 추가를 위해서 필요

    private void Start(){
        player = FindFirstObjectByType<PlayerStats>();
        player.onDead.AddListener(OnPlayerDead);
    }

    // 사망 이벤트 처리
    public void OnPlayerDead(){
        if(isDeadEnd) return; // 사망 엔딩 완료하면 반환
        DeadEnding(); // 사망 엔딩 실행
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
