using UnityEngine;
using UnityEngine.Events;

public class ChatWithNPC : MonoBehaviour
{
    [Header("State")]
    public bool isChatNow = false;

    [Header("Event")]
    public UnityEvent onChatWithScientist;
    public UnityEvent onChatWithDoomsday;
    public UnityEvent onChatWithFoodResearch;
    public UnityEvent onChatWithCivilian;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Scientist")){
            onChatWithScientist?.Invoke();
        }
        else if(collision.gameObject.CompareTag("Doomsday")){
            onChatWithDoomsday?.Invoke();
        }
        else if(collision.gameObject.CompareTag("FoodResearch")){
            onChatWithFoodResearch?.Invoke();
        }
        else if(collision.gameObject.CompareTag("Civilian")){
            onChatWithCivilian?.Invoke();
        }
    }
}
