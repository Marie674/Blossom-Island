using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class StorageTransferUI : MonoBehaviour
{
    public InventoryUI StorageUI;
    public InventoryUI PlayerInventoryUI;

    public bool IsOpen = false;
    public void Open(StorageObject pCurrentStorage)
    {
        StorageUI.CurrentStorage = pCurrentStorage;
        StorageUI.Title.text = pCurrentStorage.Name;
        PlayerInventoryUI.CurrentStorage = GameManager.Instance.Player.GetComponent<PlayerInventory>();
        PlayerInventoryUI.Title.text = PlayerInventoryUI.CurrentStorage.Name;
        IsOpen = true;
        GetComponent<WindowToggle>().Open();
    }
    public void Close()
    {
        IsOpen = false;
    }

    public void TransferItem(InventoryItemStack pStack, InventoryUI pFrom)
    {
        InventoryUI from = pFrom;
        InventoryUI to;

        if (StorageUI == from)
        {
            to = PlayerInventoryUI;
        }
        else
        {
            to = StorageUI;
        }
        int amount = 1;

        if (Input.GetButton("Toggle 1"))
        {
            amount = pStack.Amount;
        }

        if (from.SelectedItem == null || from.SelectedItem.ID == 0)
        {
            return;
        }
        ItemBase item = ItemSystem.Instance.GetItemClone(pStack.ContainedItem.ID);
        InventoryItemStack selectedStack = pStack;


        int amountAdded = to.CurrentStorage.Add(item, (uint)amount);
        int amountLeft = amount - amountAdded;

        if (amountLeft > 0)
        {
            //inventory full
        }

        from.CurrentStorage.RemoveFromStack(selectedStack, (uint)amountAdded);
    }
}

