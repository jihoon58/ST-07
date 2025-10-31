using UnityEngine;
using UnityEngine.Events;

namespace ST07.Systems
{
    public class TimeOfDaySystem : MonoBehaviour
    {
        //0-1단위가 효율적으로 보이지만 직관적으로 보기 위하여 0-24h 단위를 사용
        [Header("Day/Night Length (seconds)")]
        [Tooltip("하루 길이 = 900초 = 15분")]
        public const float dayLengthSeconds = 900f;

        [Header("Night Range (hours)")]
        [Range(0f, 24f)] public float nightStartHours = 18f;
        [Range(0f, 24f)] public float nightEndHours = 7.5f;

        [Header("State")]
        [Range(0f, 24f)] public float currentTimeHours = 0f;
        public int dayCount = 1; //1일차부터 시작

        [Header("Events")]
        public UnityEvent onNextDay; //다음 날
        public UnityEvent onNightEnded; // 낮. 해당 이벤트 쓸모없을거 같기는 하지만 삭제하지는 않을게요. 마지막 점검 때도 사용하지 않으면 삭제하세요.
        public UnityEvent onNightStarted;

        private bool wasNight;

        public bool IsNight
        {
            get
            {
                return !(currentTimeHours < nightStartHours && currentTimeHours >= nightEndHours); //낮 조건
            }
        }

        private void Start()
        {
            //event 내용이라서 잘 모르지만 쓸모없어보이기에 주석 처리함. 아는사람 확인해주세요
            // if (IsNight)
            // {
            //     onNightStarted?.Invoke();
            // }
            // else
            // {
            //     onDayStarted?.Invoke();
            // }
        }

        private void Update()
        {
            //필요성을 느끼지 못해 주석처리 차후에 필요하면 주석 해제하세요
            // if (dayLengthSeconds <= 0.01f)
            // {
            //     return;
            // }
            
            float delta = Time.deltaTime / dayLengthSeconds;
            currentTimeHours += delta * 24;

            if (currentTimeHours >= 24f)
            {
                currentTimeHours -= 24f;
                dayCount++;
                // 새로운 하루 시작
                onNextDay?.Invoke();
            }

            if(IsNight != wasNight){
                //wasNight를 쓴 이유는 IsNight의 계산을 생략하기 위해서이다.
                if(wasNight){
                    onNightEnded?.Invoke();
                }
                else{
                    onNightStarted?.Invoke();
                }
            }
        }

        public void SkipHours(float hours)
        {
            //필요성을 느끼지 못해 주석처리 차후에 필요하면 주석 해제하세요
            // if (dayLengthSeconds <= 0.01f)
            // {
            //     return;
            // }

            //식사 처리해주세요.
            //HERE

            // 1차 수정 코드. 차후를 위해서 2차 수정코드로 개선
            // if(currentTimeHours >= 24f){
            //     currentTimeHours -= 24f;
            //     dayCount++;
            // }

            // 2차 수정 코드
            currentTimeHours += hours;
            if(hours>=24f){
                for(int i = 0; i<hours/24;i++){
                    dayCount++;
                    onNextDay?.Invoke();
                }
                currentTimeHours = Mathf.Repeat(currentTimeHours, 24f);
            }
        }
    }
}



