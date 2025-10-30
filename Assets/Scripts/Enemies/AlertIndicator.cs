using UnityEngine;

namespace ST07.Enemies
{
    //??? 이해 못함 누군가 해주셈
    public class AlertIndicator : MonoBehaviour
    {
        [Tooltip("경보 아이콘을 위아래로 살짝 띄우는 속도")]
        public float bobSpeed = 2f;
        [Tooltip("경보 아이콘의 상하 진폭")]
        public float bobAmplitude = 0.1f;

        private Vector3 startLocalPos;

        private void Awake()
        {
            startLocalPos = transform.localPosition;
        }

        private void Update()
        {
            float y = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            transform.localPosition = startLocalPos + new Vector3(0f, y, 0f);
        }
    }
}



