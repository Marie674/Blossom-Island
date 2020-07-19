using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class ItemUseManager : Singleton<ItemUseManager>
{

    private PlayerCharacter Player;
    PlayerInventory PlayerInventory;

    public void Start()
    {
        if (Player == null)
        {
            Player = GameManager.Instance.Player;
        }
        PlayerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void UseItem(InventoryItemStack pStack)
    {
        if (pStack.ContainedItem.UsableFromToolbar == false)
        {
            return;
        }
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "UseItem", pStack.ContainedItem.itemName);
        if (pStack.ContainedItem is ItemBottle)
        {
            UseBottle(pStack.ContainedItem as ItemBottle, pStack);
        }
        if (pStack.ContainedItem is ItemFood)
        {
            UseFood(pStack.ContainedItem as ItemFood, pStack);
        }
        if (pStack.ContainedItem is ItemRecipe)
        {
            UseRecipe(pStack.ContainedItem as ItemRecipe, pStack);
        }
    }

    private void UseBottle(ItemBottle pItem, InventoryItemStack pStack)
    {
        if (pItem.HeldLiquid == null)
        {
            return;
        }
        //remove liquid
        pItem.currentCharge--;
        //use liquid
        UseFood(pItem.HeldLiquid, pStack);

        if (pItem.currentCharge == 0)
        {
            PlayerInventory.RemoveFromStack(Toolbar.Instance.SelectedSlot.ReferencedItemStack, 1);
            ItemContainer container = new ItemContainer();
            container.item = ItemSystemUtility.GetItemCopy("Empty Bottle", ItemType.Bottle);
            ItemSpawner.Instance.SpawnItems(container.item, Player.transform.position, 1);
        }
    }

    private void UseFood(ItemFood pItem, InventoryItemStack pStack)
    {
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "EatItem", pStack.ContainedItem.itemName);

        PlayerNeedManager.Instance.GetNeed("Energy").Change(pItem.energyRegen);

        if (pItem.consumable == true)
        {
            PlayerInventory.RemoveFromStack(pStack, 1);
        }
    }

    private void UseRecipe(ItemRecipe pItem, InventoryItemStack pStack)
    {
        if (CraftingManager.Instance.TeachRecipe(pItem.Recipe.UniqueName))
        {
            if (pItem.consumable == true)
            {
                PlayerInventory.RemoveFromStack(pStack, 1);
            }
        }

    }

}
