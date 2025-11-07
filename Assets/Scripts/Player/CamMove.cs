using UnityEngine;

public class CamMove : MonoBehaviour
{
    private Vector2 originPos; // 카메라 원래 위치 변수
    private Vector2 distance; // 인벤 열었을 때의 카메라 이동거리 변수
    private void Start()
    {
        originPos = new Vector2(0, 0); // 카메라 원래 위치 초기화
        transform.position = originPos; // 카메라 위치를 원래 위치로 설정

        distance = new Vector2(100, 0); // 카메라 이동 거리 초기화
    }

    public void MoveRIghtCamera()
    {
        transform.Translate(distance); // 카메라를 distance만큼 이동
    }

    public void MoveLeftCamera()
    {
        transform.position = originPos; // 카메라를 원래 위치로 이동
    }
}
