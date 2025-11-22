using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionSceneManager : MonoBehaviour
{
   public Image progressImage;
   public Text progressText;
   private float delay = 0.5f;
   
   // Start is called the first frame update
   void Start()
   {
      StartCoroutine(LoadScene());
   }
   
   IEnumerator LoadScene()
   {  
      string nextSceneName = PlayerPrefs.GetString("NextKey");
      AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
      asyncOperation.allowSceneActivation = false;

      float progress = 0f;

      while (!asyncOperation.isDone)
      {
         progress = Mathf.Lerp(progress, asyncOperation.progress, 0.95f);
         progressImage.fillAmount = progress;
         progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
         if(asyncOperation.progress >= 0.9f){
            yield return new WaitForSeconds(delay);
            asyncOperation.allowSceneActivation = true;
         }
         yield return null;
      }   
   }  
}
