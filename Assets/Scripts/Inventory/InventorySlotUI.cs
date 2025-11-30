using ST07.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public Text quantityText;

    private ItemStack currentStack;
    private Inventory inventory;
    private Transform playerTransform;
    
    // 드래그 관련
    private static InventorySlotUI draggedSlot;  // 현재 드래그 중인 슬롯
    private CanvasGroup canvasGroup;

    public void Initialize(Inventory inv, Transform player)
    {
        inventory = inv;
        playerTransform = player;
        
        // CanvasGroup 추가 (드래그 시 투명도 조절용)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Set(ItemStack stack)
    {
        //현재 슬롯이 들고 있는 스택 저장
        currentStack = stack;

        if (stack == null || stack.item == null)
        {
            Clear();
            return;
        }

        icon.enabled = true;
        icon.sprite = stack.item.icon;

        if (stack.item.maxStack > 1 && stack.quantity > 1)
            quantityText.text = stack.quantity.ToString();
        else
            quantityText.text = "";
    }

    public void Clear()
    {
        //비워질 때도 같이 비움
        currentStack = null;

        if (icon != null)
        {
            icon.enabled = false;
            icon.sprite = null;
        }

        if (quantityText != null)
            quantityText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"Slot clicked: {eventData.button}");

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 우클릭 → 아이템 버리기
            Debug.Log("우클릭 슬롯 감지됨!");
            DropItem();
        }
    }

    private void DropItem()
    {
        if (currentStack == null || currentStack.item == null)
        {
            Debug.Log("DropItem: currentStack이 null");
            return;
        }

        if (inventory == null)
        {
            Debug.Log("DropItem: inventory가 null");
            return;
        }

        if (playerTransform == null)
        {
            Debug.Log("DropItem: playerTransform이 null");
            return;
        }

        GameObject dropPrefab = currentStack.item.worldPrefab;
        if (dropPrefab == null)
        {
            Debug.Log("DropItem: worldPrefab이 비어있음");
            return;
        }

        // 실제로 땅에 생성
        Object.Instantiate(
            dropPrefab,
            playerTransform.position + Vector3.up * 0.2f,
            Quaternion.identity
        );

        // 인벤토리에서 1개 제거
        bool removed = inventory.TryRemove(currentStack.item, 1);
        Debug.Log($"DropItem: TryRemove 결과 = {removed}");
    }

    // ==== 드래그 시작 ====
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentStack == null || currentStack.item == null) return;
        
        draggedSlot = this;
        canvasGroup.alpha = 0.6f;  // 반투명하게
        canvasGroup.blocksRaycasts = false;  // 드래그 중인 슬롯은 레이캠스트 무시
    }

    // ==== 드래그 중 ====
    public void OnDrag(PointerEventData eventData)
    {
        // 비어있지만 필요 (인터페이스 구현용)
    }

    // ==== 드래그 끝 ====
    public void OnEndDrag(PointerEventData eventData)
    {
        draggedSlot = null;
        canvasGroup.alpha = 1f;  // 원래대로
        canvasGroup.blocksRaycasts = true;
    }

    // ==== 드롭 받기 ====
    public void OnDrop(PointerEventData eventData)
    {
        if (draggedSlot == null || draggedSlot == this) return;
        
        // 두 슬롯의 아이템 교체
        SwapItems(draggedSlot, this);
    }

    private void SwapItems(InventorySlotUI slotA, InventorySlotUI slotB)
    {
        if (inventory == null) return;
        
        // 인덱스 찾기
        int indexA = inventory.items.IndexOf(slotA.currentStack);
        int indexB = inventory.items.IndexOf(slotB.currentStack);
        
        // 둘 다 유효한 인덱스라면 교체
        if (indexA >= 0 && indexB >= 0)
        {
            // 리스트에서 교체
            ItemStack temp = inventory.items[indexA];
            inventory.items[indexA] = inventory.items[indexB];
            inventory.items[indexB] = temp;
            
            // UI 갱신
            inventory.InvokeInventoryChanged();
        }
        // 한쪽이 빈 슬롯이면 이동
        else if (indexA >= 0 && indexB < 0)
        {
            // slotA의 아이템을 slotB 위치로 이동
            // 빈 슬롯에 드롭하는 경우는 구현 복잡하니 간단하게 전체 갱신
            inventory.InvokeInventoryChanged();
        }
        else if (indexB >= 0 && indexA < 0)
        {
            inventory.InvokeInventoryChanged();
        }
    }
}