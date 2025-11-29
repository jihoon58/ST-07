using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bed : MonoBehaviour
{
    public UnityEvent onBedEnter;
    public UnityEvent onBedExit;

    private ST07.Systems.TimeOfDaySystem timeOfDaySystem;
    private bool isPlayerInRange = false;
    private ST07.Player.PlayerStats playerStats;

    private void Start(){
        timeOfDaySystem = FindFirstObjectByType<ST07.Systems.TimeOfDaySystem>();
        playerStats = FindFirstObjectByType<ST07.Player.PlayerStats>();
    }
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
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E)){
            sleep();
        }
    }

    private void sleep(){
        // 검은 화면으로 전환

        timeOfDaySystem.SkipHours(8);
        playerStats.FatigueFull();
        
        // 원래 화면으로 전환
        
    }
}
