using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;
    public Camera mainCamera;
    public Transform firePos;

    public GameObject bulletPrefab;

    private void Awake()
    {
        firePos = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ShootBullet();
    }

    public void ShootBullet()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            // 발사 위치에서 마우스로의 방향 계산
            Vector2 direction = (mouseWorldPos - firePos.position).normalized;
            
            // 방향에 맞게 회전 각도 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            // 총알 생성 (방향으로 회전)
            Instantiate(bulletPrefab, firePos.position, rotation);
        }
    }
}