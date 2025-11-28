using UnityEngine;
using ST07.Player;
using ST07.Items;

namespace ST07.Player
{
    public class ItemUseController : MonoBehaviour
    {
        [Header("Refs")]
        public Inventory inventory;      // 플레이어 인벤토리
        public PlayerStats playerStats;  // 플레이어 스탯

        [Header("Keys")]
        public KeyCode useFoodKey = KeyCode.Q;

        private void Update()
        {
            // Q 키를 눌렀을 때 Food 아이템 사용
            if (Input.GetKeyDown(useFoodKey))
            {
                UseFirstFood();
            }
        }

        private void UseFirstFood()
        {
            // 플레이어가 죽었으면 사용 안 함
            if (playerStats == null || playerStats.IsDead) return;
            if (inventory == null) return;

            // 인벤토리 안을 돌면서 Food 타입 찾기
            for (int i = 0; i < inventory.items.Count; i++)
            {
                ItemStack stack = inventory.items[i];

                // ResourceItem인지 확인 + 타입이 Food 인지 확인
                if (stack.item is ResourceItem resource && resource.itemType == ItemType.Food)
                {
                    // 1) 인벤토리에서 1개 제거 시도
                    bool removed = inventory.TryRemove(resource, 1);

                    //if (removed)
                    //{
                    //    // 2) 제거 성공했으면 효과 적용 (체력 회복)      이 부분 지우기 일단
                    //    resource.ApplyEffect(playerStats);
                    //    Debug.Log("Food 사용! 체력 회복됨.");
                    //}
                    //else
                    //{
                    //    Debug.Log("Food 제거 실패…(인벤토리 상태 확인 필요)");
                    //}

                    // 첫 번째 Food만 사용하고 함수 종료
                    return;
                }
            }

            Debug.Log("사용할 Food 아이템이 없습니다.");
        }
    }
}
