using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform target; // 플레이어 Transform
    private Vector3 offset; // 카메라와 플레이어 사이의 초기 거리

    void Start()
    {
        // 카메라와 플레이어의 초기 Z축 거리 저장
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // 플레이어 위치를 따라가되, Z축은 유지
        Vector3 targetPos = target.position + offset;
        transform.position = targetPos;
    }
}
