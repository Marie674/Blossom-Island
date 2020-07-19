using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class AllInventoryUI : InventoryTabUI
// {
//     public override void Draw(InventoryUI pCurrentUI, Transform pContainer)
//     {
//         GetItems();
//         int stackAmt = pCurrentUI.CurrentStorage.MaxStacks;
//         StorageTransferUI transferUI = GameObject.FindObjectOfType<StorageTransferUI>();

//         int i = 0;
//         foreach (InventoryItemStack itemStack in ItemStacks)
//         {

//             ItemStackUI itemUI = Instantiate(pCurrentUI.ItemUIPrefab, pContainer);
//             CraftingInputUI craftingInputUI = GameObject.FindObjectOfType<CraftingInputUI>();

//             itemUI.CurrentUI = pCurrentUI;
//             itemUI.ItemStack = itemStack;
//             itemUI.Draw();
//             DrawnItems.Add(itemUI);
//             i++;
//             if (transferUI != null && transferUI.IsOpen)
//             {
//                 itemUI.OnStackSelected += transferUI.TransferItem;

//             }
//             else if (craftingInputUI != null && craftingInputUI.IsOpen)
//             {
//                 itemUI.OnStackSelected += craftingInputUI.AddItem;

//             }

//             else if (pCurrentUI != null)
//             {
//                 itemUI.OnStackSelected += pCurrentUI.SetSelectedItem;

//             }
//         }
//         stackAmt -= i;
//         for (int j = 0; j < stackAmt; j++)
//         {
//             CraftingInputUI craftingInputUI = GameObject.FindObjectOfType<CraftingInputUI>();

//             ItemStackUI itemUI = Instantiate(pCurrentUI.ItemUIPrefab, pContainer);
//             itemUI.CurrentUI = pCurrentUI;
//             itemUI.ItemStack = null;
//             itemUI.Draw();
//             DrawnItems.Add(itemUI);
//             if (transferUI != null && transferUI.IsOpen)
//             {
//                 itemUI.OnStackSelected += transferUI.TransferItem;

//             }
//             else if (craftingInputUI != null && craftingInputUI.IsOpen)
//             {
//                 itemUI.OnStackSelected += craftingInputUI.AddItem;

//             }

//             else if (pCurrentUI != null)
//             {
//                 itemUI.OnStackSelected += pCurrentUI.SetSelectedItem;

//             }
//         }

//     }
//}
