using UnityEngine;
using ST07.Systems;
using ST07.Items;
using System;
using UnityEngine.Events;

namespace ST07.World{
    public class FieldItem : MonoBehaviour
    {
        public string itemName;
        public int quantity = 0;
        public int weight = 0;
        public ItemDefinition itemDefinition;

        public Inventory inventory;
        [Header("Events")]
        public UnityEvent onItemEnter;
        public UnityEvent onItemExit;
        public UnityEvent onItemPickup;
        public void setItemDefinition(){
            // try{
            //     resourceItem = Resources.Load<ResourceItem>("Items/" + itemName);
            // }catch(Exception){
            //     weaponItem = Resources.Load<WeaponItem>("Items/" + itemName);
            // }
            itemDefinition = Resources.Load<ItemDefinition>("Items/" + itemName);
            weight = (int)(itemDefinition.weight * quantity);
            // try{
            //     weight = (int)(resourceItem.weight * quantity);
            // }catch(Exception){
            //     weight = (int)(weaponItem.weight * quantity);
            // }
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.F)){
                if(inventory.CanAdd(itemDefinition, quantity)) {
                    inventory.TryAdd(itemDefinition, quantity);
                    onItemPickup?.Invoke();
                    Destroy(gameObject);
                }else {
                    Debug.Log("Inventory is full");
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                onItemEnter?.Invoke();
            }
        }
        private void OnTriggerExit2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                onItemExit?.Invoke();
            }
        }
    }
}