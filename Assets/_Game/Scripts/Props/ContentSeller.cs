using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class ContentSeller : MonoBehaviour
{
    private StorageObject Storage;
    // Use this for initialization
    void Start()
    {
        Storage = GetComponent<StorageObject>();
    }

    void OnEnable()
    {
        TimeManager.OnDayChanged += SellContents;
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= SellContents;
    }

    void SellContents(int pDayIndex)
    {
        float coins = 0;
        ItemBase coinItem = ItemSystem.Instance.GetItemClone("Coin");
        print(coinItem);
        ItemBase halfCoinItem = ItemSystem.Instance.GetItemClone("Halfcoin");

        foreach (InventoryItemStack stack in Storage.ContainedStacks)
        {
            if (stack.ContainedItem.ID != coinItem.ID && stack.ContainedItem.ID != halfCoinItem.ID)
            {
                coins += stack.ContainedItem.Value * stack.Amount;
                GameManager.Instance.AddShippedItem(stack.ContainedItem, stack.Amount);
                PixelCrushers.MessageSystem.SendMessage(this, "SellItem", stack.ContainedItem.Name, stack.Amount);
            }

        }

        //        print(coins);

        int coinItems = Mathf.FloorToInt(coins);
        //        print(coinItems);
        int halfCoinItems = (int)((coins % 1) / 0.5);
        //       print(halfCoinItems);
        Storage.ClearStorage();


        int amountAdded = Storage.Add(coinItem, (uint)coinItems);
        int amountLeft = coinItems - amountAdded;
        if (amountLeft > 0)
        {
            Debug.LogWarning("Could not add " + amountLeft + " " + coinItem.Name + " to storage: " + Storage.Name);
        }

        amountAdded = Storage.Add(halfCoinItem, (uint)halfCoinItems);
        amountLeft = halfCoinItems - amountAdded;
        if (amountLeft > 0)
        {
            Debug.LogWarning("Could not add " + amountLeft + " " + coinItem.Name + " to storage: " + Storage.Name);
        }

    }
}
