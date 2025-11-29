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
    public SpriteRenderer spriteRenderer;

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

    public void Movement(Vector2 input)     //?? 움직이는 메서드 코드에 왜 무게 인벤토리 관련이 있지....? 고민 ㄱ
    {
        // Weight ratio calculation
        float mult = 1f;
        if (inventory != null && inventory.weightLimitKg > 0f)
        {
            float ratio = inventory.CurrentWeight / inventory.weightLimitKg; // 0~1+
            if (ratio >= 1f) mult = fullMultiplier;
        }

        Vector2 movement = input * speed * mult;

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
            animator.SetFloat("directionX", Mathf.Abs(normalizedInput.x)); // Blend Tree에 X 값을 절댓값으로 받아와서 엉뚱한 애니메이션이 실행 되지 않도록 방지함
            animator.SetFloat("directionY", normalizedInput.y);

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
    }
}
