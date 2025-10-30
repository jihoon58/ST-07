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
            if (target == null) return false;
            Vector2 toTarget = (target.position - transform.position);
            if (toTarget.magnitude > viewDistance) return false;

            float angle = Vector2.Angle(transform.right, toTarget.normalized);
            if (angle > viewAngle * 0.5f) return false;

            // 시야 차단 검사
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toTarget.normalized, toTarget.magnitude, obstacleMask);
            if (hit.collider != null) return false;

            return true;
        }

        private float GetEffectiveSpeed()
        {
            float mul = 1f;
            if (timeSystem != null && timeSystem.IsNight && nightBuffs != null)
            {
                mul *= nightBuffs.speedMultiplier;
            }
            return baseSpeed * mul;
        }

        private float GetEffectiveDamage()
        {
            float mul = 1f;
            if (timeSystem != null && timeSystem.IsNight && nightBuffs != null)
            {
                mul *= nightBuffs.attackMultiplier;
            }
            return attackDamage * mul;
        }

        private void TryAttack()
        {
            if (Time.time - lastAttackTime < attackCooldown) return;
            lastAttackTime = Time.time;

            var stats = target != null ? target.GetComponent<ST07.Player.PlayerStats>() : null;
            if (stats != null)
            {
                stats.ApplyDamage(GetEffectiveDamage());
            }
        }

        public void OnDamaged(float damage)
        {
            // 피격 시 어그로 유지/갱신: Chase로 전환
            if (target != null)
            {
                state = State.Chase;
                SetAlert(true);
            }
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



