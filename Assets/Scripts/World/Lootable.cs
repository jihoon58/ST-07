using System.Collections.Generic;
using UnityEngine;
using ST07.Items;
using ST07.Player;

namespace ST07.World
{
    public class Lootable : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        public class LootEntry
        {
            public ItemDefinition item;
            public int quantity = 1;
        }

        public List<LootEntry> contents = new List<LootEntry>();
        public bool destroyAfterLoot = true;

        public bool CanInteract(GameObject actor)
        {
            return actor != null && actor.GetComponent<Inventory>() != null;
        }

        public void Interact(GameObject actor)
        {
            if (!CanInteract(actor)) return;
            var inv = actor.GetComponent<Inventory>();
            bool allAdded = true;

            foreach (var entry in contents)
            {
                if (entry.item == null || entry.quantity <= 0) continue;
                bool ok = inv.TryAdd(entry.item, entry.quantity);
                if (!ok)
                {
                    allAdded = false;
                }
            }

            if (allAdded && destroyAfterLoot)
            {
                Destroy(gameObject);
            }
        }
    }
}



