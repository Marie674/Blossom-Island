using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class Toolbar : Singleton<Toolbar>
{

    public ToolbarSlotUI[] Slots;
    int SelectedSlotIndex = 0;
    public ToolbarSlotUI SelectedSlot;
    private InventoryUI Inventory;

    PlayerInventory PlayerInventory;
    void Start()
    {
        PlayerInventory = FindObjectOfType<PlayerInventory>();
        Inventory = GameObject.FindGameObjectWithTag("PlayerInventoryUI").GetComponent<InventoryUI>();
        Slots = GetComponentsInChildren<ToolbarSlotUI>();
        foreach (ToolbarSlotUI slot in Slots)
        {
            slot.Init();
        }
        ChangeSelectedSlot();
    }

    public delegate void SelectedSlotChange();
    public event SelectedSlotChange OnSelectedSlotChanged;

    public void ChangeSelectedSlot(ToolbarSlotUI pSlot = null)
    {

        if (GameManager.Instance.Player.IsActing)
        {
            return;
        }
        // If there is a currently selected slot, unsubscribe it from listening to its item change event
        if (SelectedSlot != null)
        {
            SelectedSlot.OnSlotItemChanged -= ItemChanged;
        }

        // If no slot is specified, set the selected slot to the current index
        if (pSlot == null)
        {
            SelectedSlot = Slots[SelectedSlotIndex];
            //			print (SelectedSlot.gameObject.name + " " + SelectedSlot.ReferencedItemStack);
        }

        // If a slot is specified, set the selected slot to that slot and set the slot index to its index
        else
        {
            SelectedSlot = pSlot;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (SelectedSlot == Slots[i])
                {
                    SelectedSlotIndex = i;
                    break;
                }
            }
        }

        // Set the active frame graphic on the selected slot and set it off for the others
        SetActiveFrame();

        // Subscribe the selected slot to its item change event
        SelectedSlot.OnSlotItemChanged += ItemChanged;
        //Call the item changed event
        ItemChanged();
        if (Inventory.IsOpen && Inventory.SelectedItem.Name != "")
        {
            SelectedSlot.ChangeItem(PlayerInventory.FindItemStack(Inventory.SelectedItem.ID));
        }


        //Call the slot change event
        if (OnSelectedSlotChanged != null)
        {
            OnSelectedSlotChanged();
        }
    }

    // Set the active frame graphic on the selected slot and set it off for the others

    void SetActiveFrame()
    {
        foreach (ToolbarSlotUI slot in Slots)
        {
            if (slot != SelectedSlot)
            {
                slot.Activate(false);
            }
            else
            {
                slot.Activate(true);
            }
        }
    }


    // Update is called once per frame
    // Check the inputs for item use and slot cycling
    void Update()
    {
        if (Input.GetButtonDown("Cycle Toolbar Up") || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectedSlotIndex = ((SelectedSlotIndex + 1) % (Slots.Length));
            ChangeSelectedSlot();
        }
        if (Input.GetButtonDown("Cycle Toolbar Down") || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectedSlotIndex = (((SelectedSlotIndex - 1) + (Slots.Length)) % (Slots.Length));
            ChangeSelectedSlot();
        }
        if (Input.GetButtonDown("Interact"))
        {
            //			if (InteractionManager.Instance.Interact() == true) {
            //				return;
            //			}
        }
        if (Input.GetButtonDown("Use Item"))
        {
            if (SelectedSlot.ReferencedItemStack != null)
            {
                ItemUseManager.Instance.UseItem(SelectedSlot.ReferencedItemStack);
            }
        }

    }


    // Broadcast even upon item change in selected slot
    public delegate void SelectedSlotItemChange();
    public event SelectedSlotItemChange OnSelectedSlotItemChanged;

    // Broadcast even upon item change in selected slot
    public delegate void EquippedPlaceableItem();
    public event EquippedPlaceableItem OnEquippedPlaceableItem;

    void ItemChanged()
    {
        if (OnSelectedSlotItemChanged != null)
        {
            OnSelectedSlotItemChanged();
        }
        //		if (SelectedSlot.ReferencedItemStack != null) {
        //			ItemBase item = ItemSystemUtility.GetItemCopy(SelectedSlot.ReferencedItemStack.Item.itemName,ItemType.PlaceableItem);
        //			if (SelectedSlot.ReferencedItemStack.Item is ItemPlaceable) {
        //				if (OnEquippedPlaceableItem != null) {
        //					OnEquippedPlaceableItem ();
        //				}
        //			}
        //		}
    }
}

