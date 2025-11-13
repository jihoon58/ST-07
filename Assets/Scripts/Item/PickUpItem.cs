using ST07.Items;
using ST07.Player;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PickUpItem : MonoBehaviour
{
    [Header("Item")]
    public ResourceItem item;          // 사과 SO 할당 (ItemType = Food)
    [Min(1)] public int quantity = 1;  // 줍는 수량

    [Header("Interaction")]
    public string playerTag = "Player";
    public GameObject hintUI;          // "F: 줍기" 안내(선택)

    [Tooltip("이 스크립트를 '음식' 전용으로 쓰고 싶다면 Food로 고정.")]
    public ItemType requiredType = ItemType.Food;

    private Inventory playerInventory;
    private bool inRange;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true; // 트리거 자동 세팅
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInventory = other.GetComponent<Inventory>() ?? other.GetComponentInParent<Inventory>();
        inRange = playerInventory != null;

        if (inRange && hintUI) hintUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        inRange = false;
        playerInventory = null;
        if (hintUI) hintUI.SetActive(false);
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

        // 이 스크립트를 '음식' 전용으로 쓰고 싶다면 타입 체크
        if (item.itemType != requiredType)
        {
            Debug.LogWarning($"타입 불일치: 필요한 타입 {requiredType}, 실제 {item.itemType}");
            return;
        }

        // 무게/스택 제한은 Inventory.TryAdd 안에서 처리
        if (playerInventory.TryAdd(item, quantity))
        {
            if (hintUI) hintUI.SetActive(false);
            Destroy(gameObject); // 성공 시 씬에서 제거
        }
        else
        {
            Debug.Log("인벤토리가 가득이거나 무게 제한 초과입니다.");
        }
    }
}
