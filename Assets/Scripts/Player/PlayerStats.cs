using ST07.Enemies;
using UnityEngine;

namespace ST07.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        [Header("Fatigue")]
        public float maxFatigueSeconds = Systems.TimeOfDaySystem.dayLengthSeconds;
        public float currentFatigueSeconds = Systems.TimeOfDaySystem.dayLengthSeconds;

        public bool IsDead => currentHealth <= 0f || currentFatigueSeconds <= 0f;

        public void ApplyDamage(float amount)
        {
            if (amount <= 0f || IsDead) return;
            currentHealth = Mathf.Max(0f, currentHealth - amount);
        }

        public void HealthFull()
        {
            currentHealth = maxHealth;
        }

        public void FatigueFull()
        {
            currentFatigueSeconds = maxFatigueSeconds;
        }

        public void ConsumeFatigueByDay(float dayFraction)
        {
            currentFatigueSeconds -= Mathf.Max(0f, dayFraction);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Zombie"))
            {
                // 충돌한 좀비에서 ZombieAI 컴포넌트 가져오기
                var zombie = collision.gameObject.GetComponent<ST07.Enemies.ZombieAI>();

                if (zombie != null)
                {
                    // 그 좀비의 attackDamage 사용
                    ApplyDamage(zombie.attackDamage);
                    Debug.Log($"좀비 타입: {zombie.GetType().Name}, 데미지: {zombie.attackDamage}");
                    Debug.Log($"플레이어의 남은 체력: {currentHealth}");
                }
            }
        }
    }
}



