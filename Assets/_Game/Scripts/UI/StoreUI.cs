using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;


public class StoreUI : InventoryUI
{

    public void Buy()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        ItemBase storageSelection = SelectedItem;

        if (SelectedStack == null || SelectedStack.Amount < 1)
        {
            return;
        }

        if (inventory.Gold >= storageSelection.Value)
        {
            if (inventory.Add(storageSelection, 1) > 0)
            {
                CurrentStorage.RemoveFromStack(SelectedStack, 1);
                if (SelectedStack.Amount < 1)
                {
                    SetSelectedItem(null);
                }
            }

        }
    }

}
