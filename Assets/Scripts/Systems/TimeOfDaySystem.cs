using UnityEngine;
using UnityEngine.Events;

namespace ST07.Systems
{
    public class TimeOfDaySystem : MonoBehaviour
    {
        [Header("Day/Night Length (seconds)")]
        [Tooltip("하루 길이(초). 12~15분 권장: 720~900")]
        public float dayLengthSeconds = 900f;

        [Header("Night Range (normalized time)")]
        [Range(0f, 1f)] public float nightStartNormalized = 0.7f;
        [Range(0f, 1f)] public float nightEndNormalized = 1.0f;

        [Header("State (read-only)")]
        [Range(0f, 1f)] public float dayTime01 = 0f; // 0..1 진행도
        public int dayCount = 0;

        [Header("Events")]
        public UnityEvent onDayStarted;
        public UnityEvent onNightStarted;
        public UnityEvent onNightEnded;

        private bool wasNight;

        public bool IsNight
        {
            get
            {
                if (nightStartNormalized <= nightEndNormalized)
                {
                    return dayTime01 >= nightStartNormalized && dayTime01 < nightEndNormalized;
                }
                // 래핑 케이스 (예: 0.8~0.2)
                return dayTime01 >= nightStartNormalized || dayTime01 < nightEndNormalized;
            }
        }

        public float SecondsPerGameHour
        {
            get { return dayLengthSeconds / 24f; }
        }

        private void Start()
        {
            wasNight = IsNight;
            if (wasNight)
            {
                onNightStarted?.Invoke();
            }
            else
            {
                onDayStarted?.Invoke();
            }
        }

        private void Update()
        {
            if (dayLengthSeconds <= 0.01f)
            {
                return;
            }

            float delta = Time.deltaTime / dayLengthSeconds;
            dayTime01 += delta;

            if (dayTime01 >= 1f)
            {
                dayTime01 -= 1f;
                dayCount++;
                // 새로운 하루 시작
                onDayStarted?.Invoke();
            }

            bool isNightNow = IsNight;
            if (isNightNow != wasNight)
            {
                if (isNightNow)
                {
                    onNightStarted?.Invoke();
                }
                else
                {
                    onNightEnded?.Invoke();
                }
                wasNight = isNightNow;
            }
        }

        public void SkipHours(float hours)
        {
            if (dayLengthSeconds <= 0.01f)
            {
                return;
            }
            float normalized = Mathf.Repeat(hours / 24f, 1f);
            float before = dayTime01;
            dayTime01 = Mathf.Repeat(dayTime01 + normalized, 1f);

            if (dayTime01 < before)
            {
                dayCount++;
                onDayStarted?.Invoke();
            }

            bool isNightNow = IsNight;
            if (isNightNow != wasNight)
            {
                if (isNightNow) onNightStarted?.Invoke();
                else onNightEnded?.Invoke();
                wasNight = isNightNow;
            }
        }
    }
}



