using UnityEngine;
using ST07.Player;
using ST07.Items;

namespace ST07.Player
{
    public class ItemUseController : MonoBehaviour
    {
        [Header("Refs")]
        public Inventory inventory;      // �÷��̾� �κ��丮
        public PlayerStats playerStats;  // �÷��̾� ����

        [Header("Keys")]
        public KeyCode useFoodKey = KeyCode.Q;

        private void Update()
        {
            // Q Ű�� ������ �� Food ������ ���
            if (Input.GetKeyDown(useFoodKey))
            {
                UseFirstFood();
            }
        }

        private void UseFirstFood()
        {
            // �÷��̾ �׾����� ��� �� ��
            if (playerStats == null || playerStats.IsDead) return;
            if (inventory == null) return;

            // �κ��丮 ���� ���鼭 Food Ÿ�� ã��
            for (int i = 0; i < inventory.items.Count; i++)
            {
                ItemStack stack = inventory.items[i];

                // ResourceItem���� Ȯ�� + Ÿ���� Food ���� Ȯ��
                if (stack.item is ResourceItem resource && resource.itemType == ItemType.Food)
                {
                    // 1) �κ��丮���� 1�� ���� �õ�
                    bool removed = inventory.TryRemove(resource, 1);

                    if (removed)
                    {
                        // 2) ���� ���������� ȿ�� ���� (ü�� ȸ��)
                        //resource.ApplyEffect(playerStats);
                        Debug.Log("Food ���! ü�� ȸ����.");
                    }
                    else
                    {
                        Debug.Log("Food ���� ���С�(�κ��丮 ���� Ȯ�� �ʿ�)");
                    }

                    // ù ��° Food�� ����ϰ� �Լ� ����
                    return;
                }
            }

            Debug.Log("����� Food �������� �����ϴ�.");
        }
    }
}
