using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ST07.Systems;
public class UIManager : MonoBehaviour
{
    [Header("Ref")]
    public GameObject mainCanvas;   
    public GameObject player;
    public Text hintText;
    public Text timeText;
    public Image researchBar;
    public Text researchText;
    public GameObject blackScreen;
    public GameObject ResearchEndingScreen;

    
    #region 싱글톤
    public static UIManager instance;
    private void Awake(){
        // 싱글톤
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    /// <summary>
    /// 시작 시 UI 비활성화
    /// </summary>
    private void Start(){
        // 시작화면에서 불필요한 요소 비활성화
        mainCanvas.SetActive(false);
        player.SetActive(false);

        // 게임에서 가지고 다닐 요소 DontDestroyOnLoad
        DontDestroyOnLoad(mainCanvas);
        DontDestroyOnLoad(player);

        // 연구 진행도 텍스트 설정
        researchText.text = "0%";
        researchBar.fillAmount = 0f;

        StartCoroutine(SetTimeTextCoroutine());
        StartCoroutine(SetResearchTextCoroutine());
    }

    /// <summary>
    /// 씬 로드 시작 시 UI 비활성화
    /// </summary>
    public void OnLoadStart(){
        mainCanvas.SetActive(false);
        player.SetActive(false);
    }

    /// <summary>
    /// 씬 로드 완료 시 UI 활성화
    /// </summary>
    public void OnLoadEnd(){
        mainCanvas.SetActive(true);
        player.SetActive(true);
    }

    /// <summary>
    /// 검은 화면으로 페이드 인 (투명 → 불투명)
    /// </summary>
    /// <param name="duration">페이드 시간 (초, 기본값: 1초)</param>
    public void FadeInBlackScreen(float duration)
    {
        StartCoroutine(FadeInCoroutine(duration));
    }

    /// <summary>
    /// 검은 화면에서 페이드 아웃 (불투명 → 투명)
    /// </summary>
    /// <param name="duration">페이드 시간 (초, 기본값: 1초)</param>
    public void FadeOutBlackScreen(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    /// <summary>
    /// 페이드 인 코루틴
    /// </summary>
    private IEnumerator FadeInCoroutine(float duration)
    {
        Image image = blackScreen.GetComponent<Image>();

        // blackScreen 활성화
        blackScreen.SetActive(true);

        Color color = image.color;
        float startAlpha = color.a;
        float targetAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            //??
            image.color = color;
            yield return null;
        }

        // 최종 알파 값 보장
        color.a = targetAlpha;
        //??
        image.color = color;
    }

    /// <summary>
    /// 페이드 아웃 코루틴
    /// </summary>
    private IEnumerator FadeOutCoroutine(float duration)
    {
        Image image = blackScreen.GetComponent<Image>();

        Color color = image.color;
        float startAlpha = color.a;
        float targetAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            image.color = color;
            yield return null;
        }

        // 최종 알파 값 보장
        color.a = targetAlpha;
        image.color = color;
        blackScreen.SetActive(false);
    }
    public void SetHintText(string text){
        hintText.text = text;
    }

    public void FalseHintText(){
        hintText.text = "";
    }

    private IEnumerator SetResearchTextCoroutine(){
        while(true){
            researchText.text = EndingManager.instance.FoodResearchPercent.ToString() + "%";
            researchBar.fillAmount = EndingManager.instance.FoodResearchPercent / 100f;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SetTimeTextCoroutine(){
        while(true){
            timeText.text = TimeOfDaySystem.instance.CurrentTimeHours;
            yield return new WaitForSeconds(1f);
        }
    }   
}
