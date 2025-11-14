using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ChatSystem : MonoBehaviour
{
    #region 싱글톤
    public static ChatSystem instance;
    private void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}
    }
    #endregion

    [Header("Refs")]
    private ChatWithNPC chatWithNPC;
    private Sprite scientistSprite;
    private Sprite doomsdaySprite;
    private Sprite foodResearchSprite;
    private Sprite civilianSprite;
    public Image dialogueBox;
    private Text dialogue;


    private void Start(){
        // 이벤트 등록
        chatWithNPC = FindFirstObjectByType<ChatWithNPC>();
        chatWithNPC.onChatWithScientist.AddListener(ChatWithScientist);
        chatWithNPC.onChatWithDoomsday.AddListener(ChatWithDoomsday);
        chatWithNPC.onChatWithFoodResearch.AddListener(ChatWithFoodResearch);
        chatWithNPC.onChatWithCivilian.AddListener(ChatWithCivilian);

        // 스프라이트 로드
        scientistSprite = Resources.Load<Sprite>("Sprites/NPC/Scientist/Large");
        doomsdaySprite = Resources.Load<Sprite>("Sprites/NPC/Doomsday/Large");
        foodResearchSprite = Resources.Load<Sprite>("Sprites/NPC/FoodResearch/Large");
        civilianSprite = Resources.Load<Sprite>("Sprites/NPC/Civilian/Large");

        // 텍스트 참조
        dialogue = dialogueBox.GetComponentInChildren<Text>();
    }
    
    /// <summary>
    /// 대화 로직: Sprite를 오른쪽으로 띄우고, 대화 내용을 출력한다. onClick 이벤트로 다음 대화로 넘어간다.
    /// </summary>
    private void ChatWithScientist(){
        Debug.Log("Chat with Scientist");
    }
    private void ChatWithDoomsday(){
        Debug.Log("Chat with Doomsday");
    }
    private void ChatWithFoodResearch(){
        Debug.Log("Chat with Food Research");
    }
    private void ChatWithCivilian(){
        Debug.Log("Chat with Civilian");
    }
}
