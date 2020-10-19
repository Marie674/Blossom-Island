using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

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
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "UseItem", pStack.ContainedItem.Name);
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
        pItem.CurrentCharge--;
        //use liquid
        UseFood(pItem.HeldLiquid, pStack);

        if (pItem.CurrentCharge == 0)
        {
            PlayerInventory.RemoveFromStack(Toolbar.Instance.SelectedSlot.ReferencedItemStack, 1);
            ItemBottle newBottle = ItemSystem.Instance.GetItemClone("Empty Bottle") as ItemBottle;
            ItemSpawner.Instance.SpawnItems(newBottle, Player.transform.position, 1);
        }
    }

    private void UseFood(ItemFood pItem, InventoryItemStack pStack)
    {
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "EatItem", pStack.ContainedItem.Name);

        PlayerNeedManager.Instance.GetNeed("Energy").Change(pItem.EnergyRegen);

        if (pItem.Consumable == true)
        {
            PlayerInventory.RemoveFromStack(pStack, 1);
        }
    }

    private void UseRecipe(ItemRecipe pItem, InventoryItemStack pStack)
    {
        if (CraftingManager.Instance.TeachRecipe(pItem.Recipe.UniqueName))
        {
            if (pItem.Consumable == true)
            {
                PlayerInventory.RemoveFromStack(pStack, 1);
            }
        }

    }

}
