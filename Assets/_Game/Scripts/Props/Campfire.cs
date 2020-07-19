using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class Campfire : MonoBehaviour
{

    public bool IsOn = false;
    public float MinutesLeft = 0;
    public bool DestroyAfter = false;

    public GameObject WoodSprite;

    public ParticleSystem FireVXF;


    public CraftingStation CraftingStation;

    void OnEnable()
    {
        TimeManager.OnMinuteChanged += MinuteChange;

    }
    void OnDisable()
    {
        TimeManager.OnMinuteChanged -= MinuteChange;
    }

    void Start()
    {
        if (CraftingStation != null)
        {

            CraftingStation.CanProgress = false;
        }

    }

    public void MinuteChange()
    {
        if (IsOn)
        {
            MinutesLeft = Mathf.Clamp(MinutesLeft -= 1, 0, 10000f);

            if (MinutesLeft <= 0)
            {
                TurnOn(false);
                if (WoodSprite != null)
                {
                    WoodSprite.SetActive(false);
                }
                if (DestroyAfter)
                {
                    if (CraftingStation != null)
                    {
                        CraftingStation.Reset();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }


    public void Interact()
    {

        ToolbarSlotUI currentSlot = Toolbar.Instance.SelectedSlot;

        if (currentSlot.ReferencedItemStack == null && CraftingStation != null)
        {
            CraftingStation.Interact();
            return;
        }
        ItemBase heldItem = currentSlot.ReferencedItemStack.ContainedItem;


        if (IsOn == false && heldItem.ItemTags.Contains(CraftingManager.ItemTags.FireStarter))
        {
            UseFireStarter(heldItem);
            return;
        }
        else if (heldItem.itemType == ItemType.Material)
        {
            if ((heldItem as ItemMaterial).burnTime > 0)
            {
                AddFuel(currentSlot.ReferencedItemStack);
                return;
            }
        }
        else
        {
            if (CraftingStation != null)
            {
                CraftingStation.Interact();

            }
        }
    }



    public void AddFuel(InventoryItemStack pItemStack)
    {
        ItemMaterial item = pItemStack.ContainedItem as ItemMaterial;
        MinutesLeft = Mathf.Clamp(MinutesLeft += item.burnTime, 0, 10000f);
        FindObjectOfType<PlayerInventory>().RemoveFromStack(pItemStack, 1);

        if (WoodSprite != null)
        {
            WoodSprite.SetActive(true);
        }
    }

    public void UseFireStarter(ItemBase pStarter)
    {
        if (IsOn == false)
        {
            TurnOn();
        }
    }

    public delegate void LitCampfire();
    public static event LitCampfire OnLitCampfire;


    private void TurnOn(bool pToggle = true)
    {
        if (pToggle == true && MinutesLeft <= 0)
        {
            return;
        }
        IsOn = pToggle;
        FireVXF.gameObject.SetActive(pToggle);
        if (OnLitCampfire != null)
        {
            OnLitCampfire();
        }

        if (CraftingStation != null)
        {
            if (IsOn == true)
            {
                CraftingStation.CanProgress = true;
            }
            else
            {
                CraftingStation.CanProgress = false;
            }
        }

    }

}
