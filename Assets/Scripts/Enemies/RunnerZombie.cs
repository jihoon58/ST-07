using UnityEngine;

namespace ST07.Enemies
{
    // 러너: 최초 발견 시 1회 돌진, 1회 타격 이후 워커로 변환
    public class RunnerZombie : ZombieAI
    {
        [Header("Runner Dash")]
        public float dashSpeedMultiplier = 3.0f;
        private bool hasAttack = false;

        private void Reset()
        {
            dashSpeedMultiplier = 3.0f;
            baseSpeed = 2.2f * dashSpeedMultiplier;
        }

        // 공격 성공 시 
        protected override void OnAttackSuccess()
        {
            if(hasAttack) return;
            baseSpeed /= dashSpeedMultiplier;
            hasAttack = true;
        }
    }
}


