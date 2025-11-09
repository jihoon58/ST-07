// ZombieAI.cs (drop-in replacement)
using UnityEngine;
using ST07.Systems;

namespace ST07.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ZombieAI : MonoBehaviour
    {
        public enum State { Idle, Chase, Attack }

        [Header("Stats (Base)")]
        public float baseSpeed = 2.2f;
        public float attackRange = 0.8f;
        public float attackDamage = 10f;
        public float attackCooldown = 1.2f;

        [Header("Night Buffs")]
        public float healthMultiplier = 1.5f;
        public float attackMultiplier = 2;
        public float speedMultiplier = 1.2f;

        [Header("Health")]
        public float maxHealth = 50f;
        public float currentHealth;

        [Header("Detection")]
        public float viewDistance = 8f;
        [Range(0f, 180f)] public float viewAngle = 110f;
        public LayerMask obstacleMask;

        [Tooltip("FOV 체크를 끌 수 있습니다(회전 안 하는 스프라이트라면 권장).")]
        public bool useFOV = false;
        [Tooltip("이 거리 안에서는 FOV/시야차폐 무시하고 무조건 발견.")]
        public float closeProximityDistance = 1.6f;

        [Header("Refs")]
        public Transform target;
        public Transform headForAlertIcon;
        public GameObject alertIndicatorPrefab;
        //public NightBuffs nightBuffs;

        [Header("Idle Sway (생동감 옵션)")]
        public float idleRadius = 0.6f;
        public float idleMoveSpeed = 1.2f;
        public float idleSwaySpeed = 0.6f;

        private Vector2 _homePos;
        private float _idleSeed;
        private float _idleSpeedJitter;
        private Vector2 _idlePhase;

        private Rigidbody2D body;
        private TimeOfDaySystem timeSystem;
        private State state = State.Idle;
        private float lastAttackTime = -999f;
        private GameObject alertInstance;

        protected virtual void Awake()  // private → protected virtual
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            currentHealth = maxHealth;

            var player = FindAnyObjectByType<PlayerController_M>();
            if (player != null) target = player.transform;

            timeSystem = FindFirstObjectByType<TimeOfDaySystem>();

            if (alertIndicatorPrefab != null && headForAlertIcon != null)
            {
                alertInstance = Instantiate(alertIndicatorPrefab, headForAlertIcon.position, Quaternion.identity, headForAlertIcon);
                alertInstance.SetActive(false);
            }

            _homePos = transform.position;

            // 각 개체마다 “다른” 위상/속도 부여
            _idleSeed = Random.Range(0f, 10000f) + GetInstanceID() * 0.123f;
            _idleSpeedJitter = Random.Range(0.75f, 1.35f);
            _idlePhase = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
        }

        void FixedUpdate()
        {
            UpdateState();
            ActByState();
        }

        void UpdateState()
        {
            bool canSee = CanSeeTarget();
            if (!canSee)
            {
                state = State.Idle;
                SetAlert(false);
                return;
            }

            float dist = Vector2.Distance(transform.position, target.position);
            state = (dist <= attackRange) ? State.Attack : State.Chase;
            SetAlert(true);
        }

        void ActByState()
        {
            float speed = GetEffectiveSpeed();

            switch (state)
            {
                case State.Idle:
                    body.linearVelocity = Vector2.Lerp(body.linearVelocity, GetIdleVelocity(), 0.15f);
                    break;

                case State.Chase:
                    Vector2 toTarget = (target.position - transform.position);
                    Vector2 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.zero;
                    body.linearVelocity = dir * speed;

                    // (선택) FOV를 사용할 때만, 바라보는 방향을 이동방향과 맞춥니다.
                    if (useFOV && dir != Vector2.zero)
                        transform.right = dir;
                    break;

                case State.Attack:
                    body.linearVelocity = Vector2.zero;
                    TryAttack();
                    break;
            }
        }

        Vector2 GetIdleVelocity()
        {
            float t = (Time.time + _idleSeed * 0.37f) * idleSwaySpeed * _idleSpeedJitter;

            float nx = Mathf.PerlinNoise(_idlePhase.x, t);
            float ny = Mathf.PerlinNoise(_idlePhase.y, t * 0.87f);

            Vector2 offset = (new Vector2(nx, ny) - new Vector2(0.5f, 0.5f)) * 2f * idleRadius;
            Vector2 targetPos = _homePos + offset;
            Vector2 toTarget = targetPos - (Vector2)transform.position;

            if (toTarget.sqrMagnitude < 0.01f) return Vector2.zero;
            return toTarget.normalized * idleMoveSpeed;
        }

        bool CanSeeTarget()
        {
            if (!target) return false;

            Vector2 toTarget = (target.position - transform.position);
            float dist = toTarget.magnitude;

            if (dist > viewDistance) return false;

            // 아주 가까우면 각도/장애물 무시하고 발견
            if (dist <= closeProximityDistance) return true;

            if (useFOV)
            {
                float angle = Vector2.Angle(transform.right, toTarget.normalized);
                if (angle > viewAngle * 0.5f) return false;
            }

            // 장애물 레이어가 설정된 경우에만 LOS 체크
            if (obstacleMask.value != 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toTarget.normalized, dist, obstacleMask);
                if (hit.collider != null) return false;
            }

            return true;
        }

        float GetEffectiveSpeed()
        {
            if (timeSystem != null && timeSystem.IsNight)
                return baseSpeed * speedMultiplier;
            return baseSpeed;
        }

        float GetEffectiveDamage()
        {
            if (timeSystem != null && timeSystem.IsNight)
                return attackDamage * attackMultiplier;
            return attackDamage;
        }

        void TryAttack()
        {
            if (Time.time - lastAttackTime < attackCooldown) return;
            lastAttackTime = Time.time;

            var stats = target ? target.GetComponent<ST07.Player.PlayerStats>() : null;
            if (stats != null)
            {
                stats.ApplyDamage(GetEffectiveDamage());
                OnAttackSuccess();
            }
        }

        protected virtual void OnAttackSuccess() { }

        public void OnDamaged(float damage)
        {
            if (currentHealth <= 0) return;

            currentHealth -= damage;
            currentHealth = Mathf.Max(0f, currentHealth);

            if (currentHealth <= 0) { Die(); return; }

            if (target != null)
            {
                state = State.Chase;
                SetAlert(true);
            }
        }

        void Die() => Destroy(gameObject);

        void SetAlert(bool on)
        {
            if (alertInstance != null) alertInstance.SetActive(on);
        }
    }
}
