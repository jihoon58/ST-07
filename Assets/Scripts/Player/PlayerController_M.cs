using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using ST07.Player;

public class PlayerController_M : MonoBehaviour
{
    [SerializeField] private Inventory inventory;                 // 플레이어 Inventory 드래그 연결
    [SerializeField, Range(0f, 1f)] private float fullMultiplier; // 꽉 찼을 때 배율

    [SerializeField] private float speed;

    [Header("Animation")]
    public Animator animator;
    private SpriteRenderer spriteRenderer;


    private Vector2 lastMoveInput;
    private Vector2 currentInput; // Current frame's input

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
        currentInput = new Vector2(vx, vy);

        // Diagonal speed correction
        if (currentInput.sqrMagnitude > 1f)
        {
            currentInput = currentInput.normalized;
        }

        // Apply movement
        Movement(currentInput);

        // Update animation based on input
        UpdateAnimation(currentInput);
    }

    public void Movement(Vector2 input)
    {
        // 무게 기반 속도 감소 계산 (1kg = 1% 감소)
        float speedMultiplier = 1f;
        
        if (inventory != null)
        {
            float currentWeight = inventory.CurrentWeight;
            
            // 1kg당 1% 속도 감소 (0.01 = 1%)
            // 예: 3kg → 3% 감소 → 0.97 배속
            float penalty = currentWeight * 0.01f;  // kg당 1% 감소
            speedMultiplier = 1f - penalty;
            
            // 최소 속도 제한 (너무 느려지지 않도록)
            speedMultiplier = Mathf.Max(speedMultiplier, 0.3f);  // 최소 30% 속도
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
            lastMoveInput = input;

            // 입력을 정규화 시켜서 변수에 저장시키고
            Vector2 normalizedInput = input.normalized;
            
            // 대각선 판단 (X와 Y가 둘 다 충분히 큰 경우)
            bool isDiagonal = Mathf.Abs(normalizedInput.x) > 0.4f && Mathf.Abs(normalizedInput.y) > 0.4f;
            
            animator.SetFloat("directionX", Mathf.Abs(normalizedInput.x));
            animator.SetFloat("directionY", normalizedInput.y);
            animator.SetBool("isDiagonal", isDiagonal);  // ✅ 대각선 여부 전달

            // X축을 기준으로 방향전환을 하는 건 오로지 SpireRenderer.flipX를 기준으로 구별함
            if (spriteRenderer != null)
            {
                if (normalizedInput.x < -0.01f)  // 왼쪽 이동을 하면?
                {
                    spriteRenderer.flipX = true; //방향전환 true
                }
                else if (normalizedInput.x > 0.01f)  // 오른쪽 이동을 하면?
                {
                    spriteRenderer.flipX = false; //방향 전환 false
                }
                
                //대각선 이동도 결국 X축을 기준으로 하기에 (0.7. 0,7) 과 같은 대각선 이동도 방향전환이 제대로 이루어짐
            }
        }
        else  // Idle 상태 - 마지막 방향 유지
        {
            if (lastMoveInput.sqrMagnitude > 0.0001f)
            {
                // 마지막 이동 방향 정규화
                Vector2 lastNormalized = lastMoveInput.normalized;
                
                // 대각선 판단 (마지막 방향 기준)
                bool isDiagonal = Mathf.Abs(lastNormalized.x) > 0.4f && Mathf.Abs(lastNormalized.y) > 0.4f;
                
                // Idle 애니메이션도 마지막 방향에 맞게 전환
                animator.SetFloat("directionX", Mathf.Abs(lastNormalized.x));
                animator.SetFloat("directionY", lastNormalized.y);
                animator.SetBool("isDiagonal", isDiagonal);  // ✅ Idle에서도 대각선 상태 유지
                
                // flipX 상태도 유지됨 (이미 설정되어 있음)
            }
        }
      
    }
}
