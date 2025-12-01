using UnityEngine;
using System.Collections.Generic;
using ST07.Items;

public class FoodStorage : MonoBehaviour
{
    [Header("Storage")]

    private bool isPlayerInRange = false;
    private Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            // 플레이어 인벤토리 찾기
            if (playerInventory == null)
            {
                playerInventory = other.GetComponent<Inventory>();
            }
            
            UIManager.instance.SetHintText("F 키를 눌러 식료품 보관");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.instance.FalseHintText();
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StoreFoodItems();
        }
    }

    /// <summary>
    /// 인벤토리에서 Food 아이템을 모두 찾아서 저장소로 이동
    /// </summary>
    private void StoreFoodItems()
    {
        if (playerInventory == null) {
            Debug.Log("PlayerInventory is null");
            return;
        }

        // 인벤토리에서 Food 타입 아이템 찾기
        foreach (var item in playerInventory.items){
            if(item.item is ResourceItem resourceItem && resourceItem.itemType == ItemType.Food){
                
                PlayerPrefs.SetInt("FoodCount", item.quantity);
                playerInventory.items.Remove(item);
                
            }
        }
    }



}
