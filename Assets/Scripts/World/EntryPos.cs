using UnityEngine;

public class EntryPos : MonoBehaviour
{
    [Header("Ref")]
    public Transform entryPos;
    public GameObject player;

    /// <summary>
    /// 씬 전환 시 플레이어 위치 설정
    /// </summary>
    
    private void Start(){
        player = FindFirstObjectByType<PlayerController_M>().gameObject;
        SetPlayerPosition();
    }

    public void SetPlayerPosition(){
        player.transform.position = entryPos.position;
    }
}
