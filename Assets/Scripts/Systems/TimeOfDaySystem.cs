using UnityEngine;
using UnityEngine.Events;

namespace ST07.Systems
{
    public class TimeOfDaySystem : MonoBehaviour
    {
        #region 싱글톤
        public static TimeOfDaySystem instance;
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

        // 0-1단위가 효율적으로 보이지만 직관적으로 보기 위하여 0-24h 단위를 사용
        [Header("One Day Length (seconds)")]
        [Tooltip("하루 길이 = 900초 = 15분")]
        public static readonly float dayLengthSeconds = 900f; // 하루 길이(초)

        [Header("Night Range (hours)")]
        [Range(0f, 24f)] public float nightStartHours = 18f; // 밤 시작 시간
        [Range(0f, 24f)] public float nightEndHours = 7.5f; // 밤 종료 시간

        [Header("Current State")]
        [Range(0f, 24f)] private float currentTimeHours = 9f; // 현재 시간 변수. AM 9시부터 시작
        public string CurrentTimeHours{
            get{
                return currentTimeHours.ToString("00") + ":" + (currentTimeHours%1f*3f/5f).ToString("00");
            }
        }
        public int dayCount = 1; // 1일차부터 시작

        [Header("Events")]
        public UnityEvent onNextDay; // 다음 날 이벤트
        public UnityEvent onNightEnded; // 낮 시작 이벤트
        public UnityEvent onNightStarted; // 밤 시작 이벤트

        // 밤 상태 관리를 위한 변수
        private bool wasNight; // 이전 낮/밤 상태

        public bool IsNight
        {
            get
            {
                return !(currentTimeHours < nightStartHours && currentTimeHours >= nightEndHours); // 낮 조건
            }
        }

        private void Update()
        {
            // 시간 증가
            currentTimeHours += Time.deltaTime / dayLengthSeconds * 24f;

            // 하루 종료 처리
            if (currentTimeHours >= 24f)
            {
                currentTimeHours -= 24f;
                dayCount++; // 날짜 증가
                onNextDay?.Invoke(); // 다음 날 이벤트 실행
            }

            // 밤 상태 처리
            if(IsNight != wasNight){
                // wasNight를 쓴 이유는 IsNight의 계산을 생략하기 위해서이다.
                if(wasNight){
                    onNightEnded?.Invoke(); // 낮 시작 이벤트 실행
                }
                else{
                    onNightStarted?.Invoke(); // 밤 시작 이벤트 실행
                }
            }
        }

        public void SkipHours(float hours)
        {
            //식사 처리해주세요.
            // HERE

            // 2차 수정 코드
            currentTimeHours += hours; // 시간 증가
            for(int i = 0; i<hours/24;i++){ // 24시간 초과 시 날짜 증가 처리
                dayCount++;
                onNextDay?.Invoke();
            }
            currentTimeHours = Mathf.Repeat(currentTimeHours, 24f); // 시간 정규화
            
        }
    }
}



