using UnityEngine;
using UnityEngine.Events;

public class EnterBuildingTrigger : MonoBehaviour
{
    [Header("Building Settings")]
    public string buildingType;
    public int buildingIndex;
    
    [Header("Event")]
    public UnityEvent onEnterBuilding;

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            PlayerPrefs.SetString("BuildingType", buildingType);
            PlayerPrefs.SetInt("BuildingIndex", buildingIndex);
            onEnterBuilding?.Invoke();
        }
    }


}
