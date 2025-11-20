using ST07.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text quantityText;

    private ItemStack currentStack;
    private Inventory inventory;
    private Transform playerTransform;

    public void Initialize(Inventory inv, Transform player)
    {
        inventory = inv;
        playerTransform = player;
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

        // 인벤토리에서 1개 제거 (TryRemove 안에 OnInventoryChanged 호출 이미 있음) :contentReference[oaicite:2]{index=2}
        bool removed = inventory.TryRemove(currentStack.item, 1);
        Debug.Log($"DropItem: TryRemove 결과 = {removed}");
    }
}