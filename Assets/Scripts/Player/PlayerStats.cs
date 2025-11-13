using ST07.Enemies;
using UnityEngine;
using UnityEngine.Events;

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

        [Header("Events")]
        public UnityEvent onDead;

        public void ApplyDamage(float amount)
        {
            if (amount <= 0f || IsDead) return;
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            if(IsDead){
                onDead?.Invoke();
            }
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
                // �浹�� ���񿡼� ZombieAI ������Ʈ ��������
                var zombie = collision.gameObject.GetComponent<ST07.Enemies.ZombieAI>();

                if (zombie != null)
                {
                    // �� ������ attackDamage ���
                    ApplyDamage(zombie.attackDamage);
                    Debug.Log($"몬스터 타입: {zombie.GetType().Name}, 몬스터 데미지: {zombie.attackDamage}");
                    Debug.Log($"현재 남은 체력: {currentHealth}");
                }
            }
        }
    }
}



