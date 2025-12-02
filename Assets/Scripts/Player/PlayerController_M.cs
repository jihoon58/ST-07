using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using ST07.Player;

public class PlayerController_M : MonoBehaviour
{
    [SerializeField] private Inventory inventory;                 // 플레이어 Inventory 드래그 연결

    [SerializeField] private float speed;

    [Header("Animation")]
    public Animator animator;
    private SpriteRenderer spriteRenderer;


    //private Vector2 lastMoveInput; // 마지막 이동했을 떄의 방향 확인용 백터변수
    private Vector2 input; // 백터변수              

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-find components if not assigned
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate input once per frame
        float vx = Input.GetAxisRaw("Horizontal");
        float vy = Input.GetAxisRaw("Vertical");
        input = new Vector2(vx, vy);

        // Diagonal speed correction
        if (input.sqrMagnitude > 1f)
        {
            input = input.normalized;
        }

        //움직임
        Movement(input);

        // 애니메이션 효과적용
        UpdateAnimation(input);
    }

    public void Movement(Vector2 input)
    {
        // 무게 기반 속도 감소 계산 (1kg = 1% 감소)
        float speedMultiplier = 1f;
        
        if (inventory != null)
        {
            float currentWeight = inventory.CurrentWeight;   //현재의 무게 = 인벤토리의 무게
            
            // 1kg당 1% 속도 감소 (0.01 = 1%)
            // 예: 3kg → 3% 감소 → 0.97 배속
            float penalty = currentWeight * 0.01f;  // 1kg당 이동속도 1% 감소
            speedMultiplier = 1f - penalty;
            
            // 최소 속도 제한 (너무 느려지지 않도록)
            speedMultiplier = Mathf.Max(speedMultiplier, 0.2f);  // 최소 20% 속도
        }

        Vector2 movement = input * speed * speedMultiplier;

        rb.linearVelocity = movement;
    }

    private void UpdateAnimation(Vector2 input)
    {
        if (animator == null)
            return;

        bool isMoving = input.sqrMagnitude > 0.0001f;
        animator.SetBool("isMoving", isMoving);
        
        if (isMoving)
        {
            
            // 대각선 판단 (X와 Y가 둘 다 충분히 큰 경우)
            bool isDiagonal = Mathf.Abs(input.x) > 0.5f && Mathf.Abs(input.y) > 0.5f;

            animator.SetFloat("directionX", Mathf.Abs(input.x));
            animator.SetFloat("directionY", input.y);
            animator.SetBool("isDiagonal", isDiagonal);  // ✅ 대각선 여부 전달

            // X축을 기준으로 방향전환을 하는 건 오로지 SpireRenderer.flipX를 기준으로 구별함
            if (spriteRenderer != null)
            {
                if (input.x < -0.01f)  // 왼쪽 이동을 하면?
                {
                    spriteRenderer.flipX = true; //방향전환 true
                }
                else if (input.x > 0.01f)  // 오른쪽 이동을 하면?
                {
                    spriteRenderer.flipX = false; //방향 전환 false
                }
                
                //대각선 이동도 결국 X축을 기준으로 하기에 (0.7. 0,7) 과 같은 대각선 이동도 방향전환이 제대로 이루어짐
            }
        }
      
    }
}
