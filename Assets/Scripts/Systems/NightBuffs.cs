using UnityEngine;

namespace ST07.Systems
{
    [CreateAssetMenu(fileName = "NightBuffs", menuName = "ST07/Balance/NightBuffs")]
    public class NightBuffs : ScriptableObject
    {
        [Header("Multipliers applied during night")]
        [Min(0f)] public float healthMultiplier = 1.2f;
        [Min(0f)] public float attackMultiplier = 1.25f;
        [Min(0f)] public float speedMultiplier = 1.15f;
    }
}



