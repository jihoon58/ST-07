using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 씬 전환 매니저
/// </summary>
public class TransitionSceneManager : MonoBehaviour
{
   [Header("Ref")]
   public Image progressImage;
   public Text progressText;
   private float delay = 0.5f;

   /// <summary>
   /// 로드 시작
   /// </summary>
   void Start()
   {
      // 로드 시작 시 UI 비활성화
      UIManager.instance.OnLoadStart();
      // 로드 시작
      StartCoroutine(LoadScene());
   }
   
   /// <summary>
   /// 씬 로드
   /// </summary>
   IEnumerator LoadScene()
   {  
      // 다음 씬 이름 가져오기
      string nextSceneName = PlayerPrefs.GetString("NextScene");
      // 다음 씬 로드
      AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
      // 즉시 로드 비활성화
      asyncOperation.allowSceneActivation = false;

      // 로드 진행률 변수
      float progress = 1f;

      // 로드 진행률 표시
      while (!asyncOperation.isDone)
      {
         progress  = Mathf.Lerp(progress, 1 - asyncOperation.progress/0.9f, 0.9f);
         progressImage.fillAmount = progress;
         progressText.text = $"{Mathf.RoundToInt((1 - progress) * 100f)}%";
         yield return null;
         
         if(asyncOperation.progress >= 0.9f){
            // 로드 완료 시 진행률 100%로 설정
            progressImage.fillAmount = 0f;
            progressText.text = "100%";

            break;
         }
      }   
      // 로드 완료 후 지연 후 UI 활성화
      yield return new WaitForSeconds(delay);
      UIManager.instance.OnLoadEnd();
      asyncOperation.allowSceneActivation = true;
   }  
}
