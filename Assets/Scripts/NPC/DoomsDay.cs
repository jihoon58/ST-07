using UnityEngine;
using ST07.Systems;
using Unity.VisualScripting;

/// <summary>
/// 종말론자
/// </summary>
public class DoomsDay : MonoBehaviour
{
	public bool doomsdayRecruited = false; // 종말론자 영입 여부
	public int doomsdayRecruitDay = 0; // 종말론자 영입하고부터 지난 날짜

    private void Start(){
        TimeOfDaySystem.instance.onNextDay.AddListener(OnNextDay); // 이벤트 등록
    }

    // 영입 이벤트 처리
    // HERE

    // 다음 날 이벤트 처리
    public void OnNextDay(){
        if(doomsdayRecruited){ 
            doomsdayRecruitDay++; // 종말론자 영입하고부터 지난 날짜 증가
            if(doomsdayRecruitDay >= 3){ // 종말론자 영입하고부터 3일이 지나면 엔딩 실행
                EndingManager.instance.DoomsdayEnding(); // 종말론자 엔딩 실행
                Destroy(gameObject); // 오브젝트 삭제
            }
        }
    }
}
