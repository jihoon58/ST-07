using ST07.Items;
using ST07.World;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemStack
{
    public ItemDefinition item;
    public int quantity;

    public float TotalWeight => item != null ? item.weight * quantity : 0f;
}

public class Inventory : MonoBehaviour
{
    [Header("Capacity")]
    [Tooltip("총 무게 제한 (kg)")]
    public float weightLimitKg = 50f;

    [Header("Items")]
    public List<ItemStack> items = new List<ItemStack>();

    public event System.Action OnInventoryChanged;

    public float CurrentWeight
    {
        get
        {
            float sum = 0f;
            for (int i = 0; i < items.Count; i++)
            {
                sum += items[i].TotalWeight;
            }
            return sum;
        }
    }

    /// <summary>
    /// 아이템을 추가할 수 있는지 조건만 검사 (실제로 추가하지 않음)
    /// </summary>
    public bool CanAdd(ItemDefinition item, int quantity)
    {
        if (item == null || quantity <= 0) return false;
        float newWeight = CurrentWeight + item.weight * quantity;
        return newWeight <= weightLimitKg + 1e-4f;
    }

    /// <summary>
    /// 아이템을 인벤토리에 추가 (조건 검사 포함)
    /// </summary>
    /// <returns>성공 여부</returns>
    public bool Add(ItemDefinition item, int quantity)
    {
        // 조건 검사 - CanAdd() 호출
        if (!CanAdd(item, quantity)) return false;

        // 실제 추가 로직
        // 1. 스택 가능한 아이템이면 기존 스택에 합치기
        if (item.maxStack > 1)
        {
            for (int i = 0; i < items.Count && quantity > 0; i++)
            {
                if (items[i].item == item && items[i].quantity < item.maxStack)
                {
                    int canFill = item.maxStack - items[i].quantity;
                    int toMove = Mathf.Min(canFill, quantity);
                    items[i].quantity += toMove;
                    quantity -= toMove;
                }
            }
        }

        // 2. 남은 수량은 새 스택으로 추가
        while (quantity > 0)
        {
            int toStack = Mathf.Min(item.maxStack, quantity);
            items.Add(new ItemStack { item = item, quantity = toStack });
            quantity -= toStack;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>
/*    / TryAdd는 Add()의 별칭 (하위 호환성 유지)
    / </summary>*/
    //public bool TryAdd(ItemDefinition item, int quantity)
    //{
    //    return Add(item, quantity);
    //}

    public bool TryRemove(ItemDefinition item, int quantity)
    {
        if (item == null || quantity <= 0) return false;
        int remaining = quantity;
        for (int i = items.Count - 1; i >= 0 && remaining > 0; i--)
        {
            if (items[i].item == item)
            {
                int take = Mathf.Min(items[i].quantity, remaining);
                items[i].quantity -= take;
                remaining -= take;
                if (items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }
            }
        }

        if (remaining == 0)
        {
            OnInventoryChanged?.Invoke();  // ✅ 제거 성공하면 알림
            return true;
        }

        return false;
    }

    public void RemoveItem(ItemDefinition item, int amount = 1)
    {
        foreach (var stack in items)
        {
            if (stack.item == item)
            {
                stack.quantity -= amount;
                if (stack.quantity <= 0)
                {
                    items.Remove(stack);
                }
                break;
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public void InvokeInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

}




