using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
[System.Serializable]
public class ContainedItem
{
    public ItemBase Item;
    public int Amount;
}
public class StorageObject : MonoBehaviour
{

    public string UniqueID;
    public string Name;
    public float CurrentWeight;
    public float MaxWeight;

    public int MaxStackAmount = 1000;
    public int MaxStacks;
    public bool IsSellChest = false;

    public ItemBase CoinItem;
    public ItemBase HalfCoinItem;

    public List<InventoryItemStack> ContainedStacks = new List<InventoryItemStack>();

    [SerializeField]
    public List<ContainedItem> StartingItems = new List<ContainedItem>();

    public delegate void ItemChange();
    public event ItemChange OnItemChanged;

    private StorageTransferUI StorageUI;

    public string Level;

    public bool Init = false;
    void Start()
    {
        Level = SceneManager.GetActiveScene().name;
        GetComponent<StorageSaver>().Apply();
        if (Init == false)
        {
            InitChest();
        }

    }

    void InitChest()
    {
        foreach (ContainedItem item in StartingItems)
        {
            int amountAdded = Add(item.Item, (uint)item.Amount);
            int amountLeft = item.Amount - amountAdded;
            if (amountLeft > 0)
            {
                Debug.LogWarning("Could not add " + amountLeft + " " + item.Item.name + " to storage: " + Name);
            }

        }

        Init = true;
    }

    public void Interact()
    {
        ShowUI(this);
    }

    private void ShowUI(StorageObject pObj)
    {
        StorageUI = GameObject.FindObjectOfType<StorageTransferUI>();
        StorageUI.Open(pObj);

        PixelCrushers.MessageSystem.SendMessage(gameObject, "OpenStorage", UniqueID);
    }

    public delegate void AddItem(StorageObject pObj);
    public static event AddItem OnAddItem;

    public virtual int Add(ItemBase pItem, uint pAmount, bool pDropLeftOvers = false)
    {
        if (IsSellChest == false && pItem.Name == "Coin")
        {
            return 0;
        }
        int amountToAdd = (int)pAmount;

        //don't add more than weight allows

        if (pItem.Weight > 0)
        {
            amountToAdd = Mathf.FloorToInt((MaxWeight - CurrentWeight) / pItem.Weight);
        }

        //don't add more than requested
        amountToAdd = (int)Mathf.Clamp(amountToAdd, 0f, pAmount);

        //        print("Can add amount if stacks allow: " + amountToAdd);
        // If weight cannot be added, return false
        if (amountToAdd < 1)
        {
            DialogueManager.ShowAlert("Inventory can't contain 1.");
            return 0;
        }
        int amountLeft = amountToAdd;
        int amountAdded = 0;

        //Check for an existing stack
        InventoryItemStack existingStack = FindItemStack(pItem.ID);


        //if there exists a stack, add as much to it as possible
        if (existingStack != null)
        {
            //            print("existing stack");
            int maxAmt = MaxStackAmount - existingStack.Amount;
            amountAdded = Mathf.Clamp(amountToAdd, 0, maxAmt);
            existingStack.Add(amountAdded);
            amountLeft -= amountAdded;
        }

        //if there are still items to add, determine how many stacks are needed, then create as many as possible, while adding as much as possible to them until there are no more items.
        //If the stack space runs out, return the amount added.

        if (amountLeft > 0)
        {
            //            print("amount left: " + amountToAdd);
            int stacksLeft = MaxStacks - ContainedStacks.Count;
            int requiredStacks = Mathf.CeilToInt((float)amountLeft / (float)MaxStackAmount);
            //   print("required stacks: " + requiredStacks);
            //
            //            print("free stacks: " + stacksLeft);

            if (requiredStacks > stacksLeft)
            {
                requiredStacks = stacksLeft;
            }
            for (int i = 0; i < requiredStacks; i++)
            {
                InventoryItemStack newItemStack = new InventoryItemStack();
                newItemStack.ContainedItem = pItem.Clone(pItem);
                int newAmt = Mathf.Clamp(amountLeft, 0, MaxStackAmount);
                newItemStack.Amount = newAmt;
                amountAdded += newAmt;
                ContainedStacks.Add(newItemStack);
                amountLeft -= newAmt;
            }
        }
        if (OnItemChanged != null)
        {
            OnItemChanged();
        }

        if (OnAddItem != null)
        {
            OnAddItem(this);
        }
        CurrentWeight += pItem.Weight * amountAdded;

        if (pDropLeftOvers)
        {
            DropLeftOvers(pItem, amountLeft);
        }
        return amountAdded;
    }

    void DropLeftOvers(ItemBase pItem, int pAmount)
    {

        ItemSpawner.Instance.SpawnItems(pItem, transform.position, (uint)pAmount);
    }

    public void DropContents()
    {
        foreach (InventoryItemStack item in ContainedStacks)
        {
            Vector3 newPos = transform.position;
            newPos.y -= 0.16f;
            ItemSpawner.Instance.SpawnItems(item.ContainedItem, newPos, (uint)item.Amount);
        }
        if (OnItemChanged != null)
        {
            OnItemChanged();
        }
        ContainedStacks.Clear();
    }
    public bool CheckAdd(ItemBase pItem, int pAmount)
    {
        float WeightToAdd = pItem.Weight * pAmount;

        if (CurrentWeight + WeightToAdd > MaxWeight)
        {
            return false;
        }
        if (FindItemStack(pItem) == null)
        {
            if (ContainedStacks.Count + 1 > MaxStacks)
            {
                return false;
            }
        }

        return true;
    }

    public int GetItemAmount(ItemBase pItem)
    {
        List<InventoryItemStack> stacks = new List<InventoryItemStack>();
        foreach (InventoryItemStack stack in ContainedStacks)
        {
            if (pItem.ID == stack.ContainedItem.ID)
            {
                stacks.Add(stack);
            }
        }
        int amt = 0;
        foreach (InventoryItemStack stack in stacks)
        {
            amt += stack.Amount;
        }
        return amt;
    }

    public uint RemoveFromStack(InventoryItemStack itemStack, uint amount = 1)
    {
        uint i = 0;
        for (i = 0; i < amount; i++)
        {
            itemStack.Remove(1);
            CurrentWeight -= itemStack.ContainedItem.Weight;
            if (itemStack.Amount <= 0)
            {

                itemStack.Delete();
                ContainedStacks.Remove(itemStack);
                i++;
                break;
            }
        }
        if (OnItemChanged != null)
        {
            OnItemChanged();
        }
        for (i = 0; i < amount; i++)
        {
            PixelCrushers.MessageSystem.SendMessage(gameObject, "LoseItem", itemStack.ContainedItem.Name);

        }
        return i;
    }

    public void RemoveItem(ItemBase pItem, uint pAmount = 1)
    {
        RemoveFromStack(FindItemStack(pItem.ID), pAmount);
    }


    public void DropItem(InventoryItemStack itemStack, uint amount = 1)
    {
        uint DropAmount = RemoveFromStack(itemStack, amount);
        Vector3 newPos = transform.position;
        newPos.y -= 0.16f;
        ItemSpawner.Instance.SpawnItems(itemStack.ContainedItem, newPos, DropAmount);
    }

    public InventoryItemStack FindItemStack(ItemBase pItem)
    {
        foreach (InventoryItemStack itemStack in ContainedStacks)
        {
            if (pItem.ID == itemStack.ContainedItem.ID)
            {
                return itemStack;
            }
        }
        return null;
    }
    public InventoryItemStack FindItemStack(int pItemID)
    {
        foreach (InventoryItemStack itemStack in ContainedStacks)
        {
            if (pItemID == itemStack.ContainedItem.ID)
            {
                return itemStack;
            }
        }
        return null;
    }

    public void ClearStorage()
    {
        List<InventoryItemStack> coinStacks = new List<InventoryItemStack>();
        if (CoinItem != null && HalfCoinItem != null)
        {
            foreach (InventoryItemStack item in ContainedStacks)
            {
                if (item.ContainedItem.ID == CoinItem.ID || item.ContainedItem.ID == HalfCoinItem.ID)
                {
                    coinStacks.Add(item);
                }
            }
        }

        ContainedStacks.Clear();
        ContainedStacks = coinStacks;
        CurrentWeight = 0;
    }

}
