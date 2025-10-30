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
    }
}



