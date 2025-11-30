using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using ST07.Player;
using ST07.Systems;
public class Bed : MonoBehaviour
{
    public UnityEvent onBedEnter;
    public UnityEvent onBedExit;

    private bool isPlayerInRange = false;
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            onBedEnter?.Invoke();
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            onBedExit?.Invoke();
            isPlayerInRange = false;
        }
    }

    private void Update(){
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
            sleep();
        }
    }

    private void sleep(){
        // 검은 화면으로 전환

        TimeOfDaySystem.instance.SkipHours(8);
        PlayerStats.instance.FatigueFull();
        
        // 원래 화면으로 전환
        
    }
}
