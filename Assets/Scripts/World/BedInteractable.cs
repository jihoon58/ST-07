using UnityEngine;
using ST07.World;
using ST07.Player;
using ST07.Systems;
using ST07.Saving;

namespace ST07.World
{
    public class BedInteractable : MonoBehaviour, IInteractable
    {
        [Tooltip("취침 시 스킵되는 시간(시간 단위)")]
        public float sleepHours = 8f;

        public bool CanInteract(GameObject actor)
        {
            return actor != null && actor.GetComponent<PlayerStats>() != null;
        }

        public void Interact(GameObject actor)
        {
            if (!CanInteract(actor)) return;

            var stats = actor.GetComponent<PlayerStats>();
            var timeSystem = FindFirstObjectByType<TimeOfDaySystem>();
            if (timeSystem != null)
            {
                timeSystem.SkipHours(Mathf.Max(0f, sleepHours));
            }

            if (stats != null)
            {
                stats.RestoreFatigueFull();
            }

            // 세이브 지점
            var save = FindFirstObjectByType<SaveSystem>();
            if (save != null)
            {
                save.Save();
            }
        }
    }
}



