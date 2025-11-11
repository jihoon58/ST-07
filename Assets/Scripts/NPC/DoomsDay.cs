using UnityEngine;
using ST07.Systems;
using Unity.VisualScripting;

public class DoomsDay : MonoBehaviour
{
	[Header("DoomsDay State")] // 종말론자 관련 필드. Dommsday는 마지막 날, 종말론 등의 뜻을 가지고 있다.
	public bool doomsdayRecruited = false; // 종말론자 영입 여부
	public int doomsdayRecruitDay = 0; // 종말론자 영입하고부터 지난 날짜

	[Header("Refs")]
	public TimeOfDaySystem timeSystem; // 시간 시스템
	public EndingManager endingManager; // 엔딩 매니저

    private void Start(){
        timeSystem = FindFirstObjectByType<TimeOfDaySystem>();
        endingManager = FindFirstObjectByType<EndingManager>();
        timeSystem.onNextDay.AddListener(OnNextDay); // 이벤트 등록
    }

    // 영입 이벤트 처리
    // HERE

    // 다음 날 이벤트 처리
    public void OnNextDay(){
        if(doomsdayRecruited){ 
            doomsdayRecruitDay++; // 종말론자 영입하고부터 지난 날짜 증가
            if(doomsdayRecruitDay >= 3){ // 종말론자 영입하고부터 3일이 지나면 엔딩 실행
                endingManager.DoomsdayEnding(); // 종말론자 엔딩 실행
                Destroy(gameObject); // 오브젝트 삭제
            }
        }
    }
}
