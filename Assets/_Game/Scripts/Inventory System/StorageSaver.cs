using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using ItemSystem;
public class StorageSaver : PixelCrushers.SpawnedObject
{
    StorageObject TargetStorage;

    private string VariableName = "";
    public override void Start()
    {
        base.Start();
        TargetStorage = GetComponent<StorageObject>();

        if (TargetStorage.GetType() == typeof(PlayerInventory))
        {
            VariableName = "PlayerInventory";
        }
        else
        {
            VariableName = "Chest " + TargetStorage.Level + " " + transform.position.x.ToString() + "," + transform.position.y.ToString();

        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
        // PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }
    public void OnRecordPersistentData()
    {
        TargetStorage = GetComponent<StorageObject>();

        if (TargetStorage.GetType() == typeof(PlayerInventory))
        {
            VariableName = "PlayerInventory";
        }
        else
        {
            VariableName = "Chest " + TargetStorage.Level + " " + transform.position.x.ToString() + "," + transform.position.y.ToString();

        }
        //        print(VariableName);

        DialogueLua.SetVariable(VariableName + "SlotAmount", TargetStorage.ContainedStacks.Count);
        DialogueLua.SetVariable(VariableName + "Init", TargetStorage.Init);

        for (int i = 0; i < TargetStorage.ContainedStacks.Count; i++)
        {
            DialogueLua.SetVariable(VariableName + "StackAmount" + i, TargetStorage.ContainedStacks[i].Amount);
            DialogueLua.SetVariable(VariableName + "StackItem" + i, TargetStorage.ContainedStacks[i].ContainedItem.itemName);
            DialogueLua.SetVariable(VariableName + "StackItemType" + i, TargetStorage.ContainedStacks[i].ContainedItem.itemType.ToString());
        }

    }

    public void Apply()
    {
        TargetStorage = GetComponent<StorageObject>();

        if (TargetStorage.GetType() == typeof(PlayerInventory))
        {
            VariableName = "PlayerInventory";
        }
        else
        {
            VariableName = "Chest " + TargetStorage.Level + " " + transform.position.x.ToString() + "," + transform.position.y.ToString();

        }

        //        print(VariableName);
        int slotAmount = DialogueLua.GetVariable(VariableName + "SlotAmount").asInt;
        TargetStorage.Init = DialogueLua.GetVariable(VariableName + "Init").asBool;


        for (int i = 0; i < slotAmount; i++)
        {
            int itemAmount = DialogueLua.GetVariable(VariableName + "StackAmount" + i).asInt;
            string itemName = DialogueLua.GetVariable(VariableName + "StackItem" + i).asString;
            string itemType = DialogueLua.GetVariable(VariableName + "StackItemType" + i).asString;
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), itemType);

            ItemBase item = ItemSystemUtility.GetItemCopy(itemName, type);
            int amountAdded = TargetStorage.Add(item, (uint)itemAmount);
            int amountLeft = itemAmount - amountAdded;
            if (amountLeft > 0)
            {
                Debug.LogWarning("Saver could not add all items to storage: " + TargetStorage.Name);
            }

        }
    }



}
