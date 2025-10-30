using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ST07.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 4.0f;
        public float acceleration = 20.0f;
        public float deceleration = 30.0f;

        [Header("Aim")]
        public Transform aimTransform; // 캐릭터가 바라볼 기준(스프라이트/무기 루트)

        private Rigidbody2D body;
        private Vector2 currentVelocity;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            HandleAim();
        }

        private void FixedUpdate()
        {
            Vector2 input = ReadMoveInput();
            Vector2 desiredVelocity = input.normalized * moveSpeed;

            Vector2 velocity = body.linearVelocity;
            Vector2 velocityDelta = desiredVelocity - velocity;

            float accel = desiredVelocity.sqrMagnitude > 0.0001f ? acceleration : deceleration;
            Vector2 change = Vector2.ClampMagnitude(velocityDelta, accel * Time.fixedDeltaTime);

            body.linearVelocity = velocity + change;
        }

        private Vector2 ReadMoveInput()
        {
#if ENABLE_INPUT_SYSTEM
            Vector2 input = Vector2.zero;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed) input.y += 1f;
                if (Keyboard.current.sKey.isPressed) input.y -= 1f;
                if (Keyboard.current.aKey.isPressed) input.x -= 1f;
                if (Keyboard.current.dKey.isPressed) input.x += 1f;
            }
            return input;
#else
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
        }

        private void HandleAim()
        {
            if (aimTransform == null)
            {
                return;
            }

            Vector3 mouseWorld = GetMouseWorldPosition();
            Vector2 dir = (mouseWorld - aimTransform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            aimTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private Vector3 GetMouseWorldPosition()
        {
#if ENABLE_INPUT_SYSTEM
            Vector2 mouse = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
            var cam = Camera.main;
            return cam != null ? cam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, -cam.transform.position.z)) : Vector3.zero;
#else
            var cam = Camera.main;
            return cam != null ? cam.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
#endif
        }
    }
}



