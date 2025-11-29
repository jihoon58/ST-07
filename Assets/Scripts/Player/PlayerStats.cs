using ST07.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ST07.Player
{
    public class PlayerStats : MonoBehaviour
    {
        private void Awake()
        {
           DontDestroyOnLoad(gameObject);
        }
        

        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        [Header("Fatigue")]
        public float maxFatigueSeconds = Systems.TimeOfDaySystem.dayLengthSeconds;
        public float currentFatigueSeconds = Systems.TimeOfDaySystem.dayLengthSeconds;

        public bool IsDead => currentHealth <= 0f || currentFatigueSeconds <= 0f;

        [Header("Events")]
        public UnityEvent onDead;

        public Image healthFillImage;

        public void Heal(float amount)
        {
            if (amount <= 0f || IsDead) return;

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            UpdateHealthUI();   // fillAmount 갱신용 함수
        }

        public void TakeDamage(float amount)
        {
            if (amount <= 0f || IsDead) return;

            currentHealth = Mathf.Max(0f, currentHealth - amount);

            // 체력바 갱신 (맞은 데미지 비율만큼 줄어듦)
            UpdateHealthUI();

            if (IsDead)
            {
                onDead?.Invoke();
            }
        }

        public void HealthFull()
        {
            currentHealth = maxHealth;
            UpdateHealthUI();
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
                    TakeDamage(zombie.attackDamage);
                    Debug.Log($"몬스터 타입: {zombie.GetType().Name}, 몬스터 데미지: {zombie.attackDamage}");
                    Debug.Log($"현재 남은 체력: {currentHealth}");
                }
            }
        }

        private void UpdateHealthUI()
        {
            if (healthFillImage == null) return;

            float ratio = (maxHealth > 0f) ? currentHealth / maxHealth : 0f;
            ratio = Mathf.Clamp01(ratio);
            healthFillImage.fillAmount = ratio;
        }
    }
}



