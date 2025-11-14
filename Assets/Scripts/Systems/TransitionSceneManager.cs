using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionSceneManager : MonoBehaviour
{
   const string nextSceneNameKey = "NextKey";
   public Image progressImage;
   
   // Start is called the first frame update
   void Start()
   {
      LoadNextScene();
   }
   
   void LoadNextScene()
   {
      StartCoroutine(LoadScene());      
   }
   
   IEnumerator LoadScene()
   {
      yield return new WaitForSeconds(1.0f);
      
      if(PlayerPrefs.HasKey(nextSceneNameKey) == false)
      {
         Debug.LogError("NextSceneNameKey does not exist.");
      }
      
      string nextSceneName = PlayerPrefs.GetString(nextSceneNameKey);
      AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
      
      while (!asyncOperation.isDone)
      {
         progressImage.fillAmount = asyncOperation.progress / 0.9f;
         yield return null;
      }   
   }  
}
