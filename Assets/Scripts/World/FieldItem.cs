using UnityEngine;
using ST07.Items;

namespace ST07.World{
    public class FieldItem : MonoBehaviour
    {
        public string itemName;
        public int quantity = 0;
        public ItemDefinition itemDefinition;

        public Inventory inventory;
        [Header("Size Settings")]
        [Tooltip("모든 아이템이 이 크기(픽셀)로 표시됩니다")]
        public float targetSize = 64f;

        private bool isPlayerInRange = false;
        
        // [Header("Events")]
        // public UnityEvent onItemEnter;
        // public UnityEvent onItemExit;
        // public UnityEvent onItemPickup;

        public void set(string itemName, int quantity){
            this.itemName = itemName;
            this.quantity = quantity;
            setItemDefinition();
            setSprite();
        }
        private void setItemDefinition(){
            itemDefinition = Resources.Load<ItemDefinition>("Items/" + itemName);
        }
        private void Update(){
            if(isPlayerInRange && Input.GetKeyDown(KeyCode.F)){
                    if(inventory.Add(itemDefinition, quantity)){
                        Destroy(gameObject);
                        Debug.Log("Item picked up");
                    }else{
                        Debug.Log("Item not picked up");
                    }
                }
        }
        private void OnTriggerEnter2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                isPlayerInRange = true;
                UIManager.instance.SetHintText("F 키를 눌러 아이템 습득");
            }
        }
        private void OnTriggerExit2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                isPlayerInRange = false;
                UIManager.instance.FalseHintText();
            }
        }
    
        private void setSprite(){
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemDefinition.icon;
            
            // 아이콘 크기를 기준으로 스케일 계산
            if(itemDefinition.icon != null){
                Vector2 iconSize = itemDefinition.icon.rect.size;
                if(iconSize.x > 0 && iconSize.y > 0){
                    float scaleX = targetSize / iconSize.x;
                    float scaleY = targetSize / iconSize.y;
                    // 비율을 유지하면서 더 큰 축을 기준으로 스케일 조정
                    float uniformScale = Mathf.Min(scaleX, scaleY);
                    transform.localScale = Vector3.one * uniformScale;
                }
            }
        }
    }
}