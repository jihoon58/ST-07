using System;
using System.IO;
using UnityEngine;
using ST07.Player;
using ST07.Systems;

namespace ST07.Saving
{
    public class Data{
        
    }
    public class SaveSystem : MonoBehaviour
    {
        static GameObject container;

        static SaveSystem instance;
        public static SaveSystem Instance
        {
            get
            {
                if(!instance){
                    container = new GameObject("SaveSystem");
                    instance = container.AddComponent(typeof(SaveSystem)) as SaveSystem;
                    DontDestroyOnLoad(container);
                }
                return instance;
            }
        }
        string gameDataFileName = "GameData.json";

        public Data data = new Data();

        public void SaveGameData(){
            string ToJsonData = JsonUtility.ToJson(data, true);
            string filePath = Application.persistentDataPath + "/" + gameDataFileName;

            File.WriteAllText(filePath, ToJsonData );
        }

        public void LoadGameData(){
            string filePath = Application.persistentDataPath + "/" + gameDataFileName;

            if(File.Exists(filePath)){
                string FromJsonData = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<Data>(FromJsonData);
                
            }
        }
    }

}



