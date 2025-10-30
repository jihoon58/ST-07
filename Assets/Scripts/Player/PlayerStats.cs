using UnityEngine;

namespace ST07.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        [Header("Fatigue (Days remaining without sleep)")]
        [Tooltip("수면 없이 버틸 수 있는 남은 일수. 0 이하 → 사망.")]
        public float maxFatigueDays = 2f;
        public float currentFatigueDays = 2f;

        public bool IsDead => currentHealth <= 0f || currentFatigueDays <= 0f;

        public void ApplyDamage(float amount)
        {
            if (amount <= 0f || IsDead) return;
            currentHealth = Mathf.Max(0f, currentHealth - amount);
        }

        public void HealFull()
        {
            currentHealth = maxHealth;
        }

        public void RestoreFatigueFull()
        {
            currentFatigueDays = maxFatigueDays;
        }

        public void ConsumeFatigueByDay(float dayFraction)
        {
            currentFatigueDays -= Mathf.Max(0f, dayFraction);
        }
    }
}



