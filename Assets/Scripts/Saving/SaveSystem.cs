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
            public float dayTime;
            public Vector3 respawnPosition;
            public float playerHealth;

            // public SaveData(int dayCount, float dayTime, Vector3 respawnPosition, float playerHealth){
            //     this.dayCount = dayCount;
            //     this.dayTime = dayTime;
            //     this.respawnPosition = respawnPosition;
            //     this.playerHealth = playerHealth;
            // }
        }

        public string fileName = "saveFile.json";

        private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

        public void Save()
        {
            SaveData data = new SaveData();

            var timeSystem = FindFirstObjectByType<TimeOfDaySystem>();
            if (timeSystem != null)
            {
                data.dayCount = timeSystem.dayCount;
                data.dayTime = timeSystem.currentTimeHours;
            }

            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                data.playerHealth = player.currentHealth;
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
                time.currentTimeHours = Mathf.Repeat(data.dayTime, 1f);
            }

            var player = FindFirstObjectByType<PlayerStats>();
            if (player != null)
            {
                player.transform.position = data.respawnPosition;
                player.currentHealth = Mathf.Clamp(data.playerHealth, 0f, player.maxHealth);
            }

#if UNITY_EDITOR
            Debug.Log($"Loaded from {FilePath}\n{json}");
#endif
            return true;
        }
    }
}



