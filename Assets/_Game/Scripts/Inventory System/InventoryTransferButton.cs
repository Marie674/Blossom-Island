using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
public class InventoryTransferButton : MonoBehaviour
{

    public InventoryUI From;
    public InventoryUI To;
    public uint Amount = 1;

    public void DoTransfer()
    {
        if (From.SelectedItem == null || From.SelectedItem.ID == 0)
        {
            return;
        }
        ItemBase item = ItemSystem.Instance.GetItemClone(From.SelectedItem.ID);
        InventoryItemStack selectedStack = From.SelectedStack;


        int amountAdded = To.CurrentStorage.Add(item, (uint)Amount);
        int amountLeft = (int)Amount - amountAdded;
        if (amountLeft > 0)
        {
            // Debug.LogWarning("Could not add " + amountLeft + " " + item.itemName + " to storage: " + To.CurrentStorage.Name);
        }

    }
}
