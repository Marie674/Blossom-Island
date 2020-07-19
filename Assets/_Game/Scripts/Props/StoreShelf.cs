// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using ItemSystem;

// public class StoreShelf : MonoBehaviour
// {

//     [SerializeField]
//     public List<StoreItem> ItemPool = new List<StoreItem>();

//     public List<ShelfSlot> Slots = new List<ShelfSlot>();

//     public void Populate()
//     {

//         var weights = new Dictionary<StoreItem, int>();

//         int slotAmount = Slots.Count;

//         foreach (StoreItem poolItem in ItemPool)
//         {
//             if (!weights.ContainsKey(poolItem))
//                 weights.Add(poolItem, poolItem.Probability);
//         }
//         if (weights.Count > 0)
//         {

//             for (int i = 0; i < slotAmount; i++)
//             {
//                 ShelfSlot slot = Slots[i];

//                 StoreItem storeItem = WeightedRandomizer.From(weights).TakeOne();
//                 slot.ItemSlot.item = ItemSystemUtility.GetItemCopy(storeItem.ItemContainer.item.itemID, storeItem.ItemContainer.item.itemType);
//                 slot.MaxAmount = storeItem.MaxAmount;
//                 slot.CurrentAmount = storeItem.MaxAmount;
//                 slot.CanInteract = true;
//                 slot.UpdateVisuals();
//                 weights.Remove(storeItem);

//                 if (weights.Count < 1)
//                 {
//                     foreach (StoreItem poolItem in ItemPool)
//                     {
//                         if (!weights.ContainsKey(poolItem))
//                             weights.Add(poolItem, poolItem.Probability);
//                     }
//                 }
//             }
//         }

//     }


// }
