using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
public class InventoryTransferButton : MonoBehaviour
{

    public InventoryUI From;
    public InventoryUI To;
    public uint Amount = 1;

    public void DoTransfer()
    {
        if (From.SelectedItem == null || From.SelectedItem.itemID == 0)
        {
            return;
        }
        ItemBase item = ItemSystemUtility.GetItemCopy(From.SelectedItem.itemID, From.SelectedItem.itemType);
        InventoryItemStack selectedStack = From.SelectedStack;


        int amountAdded = To.CurrentStorage.Add(item, (uint)Amount);
        int amountLeft = (int)Amount - amountAdded;
        if (amountLeft > 0)
        {
            // Debug.LogWarning("Could not add " + amountLeft + " " + item.itemName + " to storage: " + To.CurrentStorage.Name);
        }

    }
}
