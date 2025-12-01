using UnityEngine;
using ST07.Player;

/// <summary>
/// 시작 위치
/// </summary>
public class EntryPos : MonoBehaviour
{
    [Header("Ref")]
    public Transform entryPos;
    public GameObject player;

    /// <summary>
    /// 씬 전환 시 플레이어 위치 설정
    /// </summary>
    private void Start(){
        // PlayerStats 인스턴스가 없으면 FindFirstObjectByType으로 찾기
        if (PlayerStats.instance == null)
        {
            var playerStats = FindFirstObjectByType<PlayerStats>();
            if (playerStats != null)
            {
                player = playerStats.gameObject;
                Debug.Log("컴포넌트로 할당");
            }
            else
            {
                // Player 태그로 찾기
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj;
                    Debug.Log("태그로 할당");
                }
                else
                {
                    Debug.LogError("EntryPos: 플레이어를 찾을 수 없습니다.");
                    return;
                }
            }
        }
        else
        {
            player = PlayerStats.instance.gameObject;
            Debug.Log("스탯의 인스턴스로 할당");
        }
        
        SetPlayerPosition();
    }

    private void SetPlayerPosition(){
        player.transform.position = entryPos.position;
    }
}
