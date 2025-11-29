using ST07.Items;
using ST07.Player;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class PickUpItem : MonoBehaviour
{
    [Header("Item")]
    public ResourceItem item;          // 사과 SO 할당 (ItemType = Food)
    [Min(1)] public int quantity = 1;  // 줍는 수량

    [Header("Interaction")]
    public string playerTag = "Player";
    //public GameObject hintUI;          // "F: 줍기" 안내(선택)

    private static Text sharedHintText; // 프리팹에 자동 할당 시켜줄 변수

    // 안내 텍스트 ex) F키를 눌러 습득하세요
    public Text hintText;

    private Inventory playerInventory;
    private bool inRange;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true; // 트리거 자동 세팅
    }

    private void Awake()
    {
        // 스프라이트 자동 지정 (기존 코드 유지)
        var sr = GetComponent<SpriteRenderer>();
        if (sr && item && item.icon) sr.sprite = item.icon;
    }

    private void Start()
    {
        Debug.Log($"[{gameObject.name}] Start 시작 - hintText: {hintText != null}, sharedHintText: {sharedHintText != null}");
        
        // 1) sharedHintText가 아직 없으면 씬에서 찾기
        if (sharedHintText == null)
        {
            Debug.Log($"[{gameObject.name}] sharedHintText가 null, 찾는 중...");
            
            // 인스펙터에서 넣어줬으면 그걸 사용
            if (hintText != null)
            {
                sharedHintText = hintText;
                Debug.Log($"[{gameObject.name}] Inspector에서 할당된 hintText 사용!");
            }

        }
        else
        {
            Debug.Log($"[{gameObject.name}] sharedHintText 이미 있음 (재사용)");
        }

        hintText = sharedHintText;
        Debug.Log($"[{gameObject.name}] hintText 최종 할당: {hintText != null}");

        // 3) 기본 상태는 꺼두기 + 기본 문구 설정
        if (hintText)
        {
            hintText.gameObject.SetActive(false);
            if (item)
                hintText.text = $"F 키로 {item.itemName} 습득";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[{gameObject.name}] OnTriggerEnter2D - Tag: {other.tag}");
        
        if (!other.CompareTag(playerTag)) return;

        // ✅ hintText가 null이면 다시 찾기 (Start에서 실패한 경우 대비)
        if (hintText == null)
        {
            Debug.LogWarning($"[{gameObject.name}] hintText가 null! 다시 찾는 중...");
            
            if (sharedHintText == null)
            {
                GameObject hintObj = GameObject.Find("HintText");
                if (hintObj != null)
                {
                    sharedHintText = hintObj.GetComponent<Text>();
                    Debug.Log($"[{gameObject.name}] OnTrigger에서 HintText 찾음!");
                }
            }
            hintText = sharedHintText;
        }

        playerInventory = other.GetComponent<Inventory>() ?? other.GetComponentInParent<Inventory>();
        inRange = playerInventory != null;

        Debug.Log($"[{gameObject.name}] 플레이어 감지! inRange: {inRange}, hintText: {hintText != null}");

        //if (inRange && hintUI) hintUI.SetActive(true);

        // 힌트 문구 세팅
        if (inRange)
        {
            if (hintText) 
            { 
                hintText.text = $"F 키를 눌러 {item.itemName} 습득하기"; 
                hintText.gameObject.SetActive(true); 
                Debug.Log($"[{gameObject.name}] 힌트 텍스트 활성화!");
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] hintText가 null이어서 힌트를 표시할 수 없음!");
            }
            //if (hintUI) { hintUI.SetActive(true); }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        inRange = false;
        playerInventory = null;
        if (hintText) hintText.gameObject.SetActive(false);
        //if (hintUI) hintUI.SetActive(false);
    }

    private void Update()
    {
        if (!inRange || playerInventory == null) return;

        if (Input.GetKeyDown(KeyCode.F))
            TryPickup();
    }

    private void TryPickup()
    {
        if (item == null)
        {
            Debug.LogWarning("PickupResourceItem2D: item이 비어 있습니다.");
            return;
        }

        // 무게/스택 제한은 Inventory.TryAdd 안에서 처리
        if (playerInventory.TryAdd(item, quantity))
        {
            if (hintText) hintText.gameObject.SetActive(false);
            //if (hintUI) hintUI.SetActive(false);
            Destroy(gameObject); // 성공 시 씬에서 제거
        }
        else
        {
            Debug.Log("인벤토리가 가득이거나 무게 제한 초과입니다.");
        }
    }
}
