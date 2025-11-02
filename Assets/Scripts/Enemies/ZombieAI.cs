using UnityEngine;
using ST07.Systems;
using Mono.Cecil;

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
        
        [Header("Health")]
        public float maxHealth = 50f;
        public float currentHealth;

        [Header("Detection")]
        public float viewDistance = 8f;
        [Range(0f, 180f)] public float viewAngle = 110f;
        public LayerMask obstacleMask;

        [Header("Refs")]
        public Transform target; // 플레이어
        public Transform headForAlertIcon;
        public GameObject alertIndicatorPrefab;
        public NightBuffs nightBuffs;
        
        private Rigidbody2D body;
        private TimeOfDaySystem timeSystem;
        private State state = State.Idle;
        private float lastAttackTime = -999f;
        private GameObject alertInstance;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // 체력 초기화
            currentHealth = maxHealth;
            
            var player = FindAnyObjectByType<PlayerController_M>();
            if (player != null)
            {
                target = player.transform;
            }

            timeSystem = FindFirstObjectByType<TimeOfDaySystem>();
            if (alertIndicatorPrefab != null && headForAlertIcon != null)
            {
                alertInstance = Instantiate(alertIndicatorPrefab, headForAlertIcon.position, Quaternion.identity, headForAlertIcon);
                alertInstance.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            UpdateState();
            ActByState();
        }

        private void UpdateState()
        {
            bool canSee = CanSeeTarget();
            if (!canSee)
            {
                state = State.Idle;
                SetAlert(false);
                return;
            }

            float dist = Vector2.Distance(transform.position, target.position);
            if (dist <= attackRange)
            {
                state = State.Attack;   
            }
            else
            {
                state = State.Chase;
            }

            SetAlert(true);
        }

        private void ActByState()
        {
            float speed = GetEffectiveSpeed();
            switch (state)
            {
                case State.Idle:
                    body.linearVelocity = Vector2.Lerp(body.linearVelocity, Vector2.zero, 0.1f);
                    break;
                case State.Chase:
                    Vector2 dir = (target.position - transform.position).normalized;
                    body.linearVelocity = dir * speed;
                    break;
                case State.Attack:
                    body.linearVelocity = Vector2.zero;
                    TryAttack();
                    break;
            }
        }

        private bool CanSeeTarget()
        {
            //거리 체크
            if (target == null) return false;
            Vector2 toTarget = (target.position - transform.position);
            if (toTarget.magnitude > viewDistance) return false;
            
            //각도 체크
            float angle = Vector2.Angle(transform.forward, toTarget.normalized);
            if (angle > viewAngle * 0.5f) return false;

            // 시야 차단 검사
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toTarget.normalized, toTarget.magnitude, obstacleMask);
            if (hit.collider != null) return false;

            return true;
        }

        private float GetEffectiveSpeed()
        {
            if (timeSystem != null && timeSystem.IsNight && nightBuffs != null)
            {
                return baseSpeed * nightBuffs.speedMultiplier;
            }
            return baseSpeed;
        }

        private float GetEffectiveDamage()
        {
            if (timeSystem != null && timeSystem.IsNight && nightBuffs != null)
            {
                return attackDamage * nightBuffs.attackMultiplier;
            }
            return attackDamage;
        }

        private void TryAttack()
        {
            if (Time.time - lastAttackTime < attackCooldown) return;
            lastAttackTime = Time.time;

            var stats = target != null ? target.GetComponent<ST07.Player.PlayerStats>() : null;
            if (stats != null)
            {
                stats.ApplyDamage(GetEffectiveDamage());
                OnAttackSuccess();
            }
        }

        // 공격 성공 시 파생에서 행동 커스텀 가능
        protected virtual void OnAttackSuccess() { }

        public void OnDamaged(float damage)
        {
            // 이미 죽었으면 무시
            if (currentHealth <= 0) return;
            
            // 체력 감소
            currentHealth -= damage;
            currentHealth = Mathf.Max(0f, currentHealth);
            
            // 체력이 0 이하면 죽음 처리
            if (currentHealth <= 0)
            {
                Die();
                return;
            }
            
            // 피격 시 어그로 유지/갱신: Chase로 전환
            if (target != null)
            {
                state = State.Chase;
                SetAlert(true);
            }
        }
        
        private void Die()
        {
            // 좀비 제거
            Destroy(gameObject);
        }

        private void SetAlert(bool on)
        {
            if (alertInstance != null)
            {
                alertInstance.SetActive(on);
            }
        }
    }
}



