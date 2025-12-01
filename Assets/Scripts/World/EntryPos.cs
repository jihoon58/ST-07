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
        player = PlayerStats.instance.gameObject;
        SetPlayerPosition();
    }

    private void SetPlayerPosition(){
        player.transform.position = entryPos.position;
    }
}
