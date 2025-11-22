using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using ST07.Player; // ← 맨 위 using에 추가


public class PlayerController_M : MonoBehaviour
{

    [SerializeField] private Inventory inventory;                 // 플레이어 Inventory 드래그 연결
    [SerializeField, Range(0f, 1f)] private float fullMultiplier; // 꽉 찼을 때 배율

    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        
    }

    public void Movement()
    {
        float vx = Input.GetAxisRaw("Horizontal");
        float vy = Input.GetAxisRaw("Vertical");

        //입력백터
        Vector2 input = new Vector2(vx, vy);

        //대각선 속도 보정
        if (input.sqrMagnitude > 1f)
        {
            input = input.normalized;
        }

        // 무게 비율 계산
        float mult = 1f;
        if (inventory != null && inventory.weightLimitKg > 0f)
        {
            float ratio = inventory.CurrentWeight / inventory.weightLimitKg; // 0~1+
            if (ratio >= 1f) mult = fullMultiplier;
        }

        Vector2 movement = input * speed * mult;
       


        rb.linearVelocity = movement;
    }
}
