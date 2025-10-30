using System;
using System.IO;
using UnityEngine;
using ST07.Player;
using ST07.Systems;

namespace ST07.Saving
{
    public class SaveSystem : MonoBehaviour
    {
        [Serializable]
        public class SaveData
        {
            public int dayCount;
            public float dayTime01;
            public Vector3 playerPosition;
            public float playerHealth;
            public float playerFatigueDays;
        }

        public string fileName = "save_slot_01.json";

        private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

        public void Save()
        {
            var data = new SaveData();

            var time = FindFirstObjectByType<TimeOfDaySystem>();
            if (time != null)
            {
                data.dayCount = time.dayCount;
                data.dayTime01 = time.currentTimeHours;
            }

            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                data.playerPosition = player.transform.position;
                data.playerHealth = player.currentHealth;
                data.playerFatigueDays = player.currentFatigueSeconds;
            }

            string json = JsonUtility.ToJson(data, true);
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllText(FilePath, json);
#if UNITY_EDITOR
            Debug.Log($"Saved to {FilePath}\n{json}");
#endif
        }

        public bool Load()
        {
            if (!File.Exists(FilePath))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No save found at {FilePath}");
#endif
                return false;
            }

            string json = File.ReadAllText(FilePath);
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null) return false;

            var time = FindFirstObjectByType<TimeOfDaySystem>();
            if (time != null)
            {
                time.dayCount = data.dayCount;
                time.currentTimeHours = Mathf.Repeat(data.dayTime01, 1f);
            }

            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                player.transform.position = data.playerPosition;
                player.currentHealth = Mathf.Clamp(data.playerHealth, 0f, player.maxHealth);
                player.currentFatigueSeconds = Mathf.Clamp(data.playerFatigueDays, 0f, player.maxFatigueSeconds);
            }

#if UNITY_EDITOR
            Debug.Log($"Loaded from {FilePath}\n{json}");
#endif
            return true;
        }
    }
}



