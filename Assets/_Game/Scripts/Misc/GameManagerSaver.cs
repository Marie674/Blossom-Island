using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Game.Items;
public class GameManagerSaver : MonoBehaviour
{
    GameManager TargetManager;
    string VariableName;

    public void OnEnable()
    {

        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        OnRecordPersistentData();
        PersistentDataManager.UnregisterPersistentData(this.gameObject);
    }


    void OnRecordPersistentData()
    {
        VariableName = GameManager.Instance.name;
        TargetManager = GetComponent<GameManager>();
        DialogueLua.SetVariable(VariableName + "GameStarted", TargetManager.GameStarted);
        if (TargetManager.GameStarted == true)
        {
            DialogueLua.SetVariable(VariableName + "NativeTreeID", TargetManager.NativeTreeID);
        }

        DialogueLua.SetVariable(VariableName + "ShippedItemsAmount", TargetManager.ShippedItems.Count);
        int i = 0;
        foreach (ShippedItem item in TargetManager.ShippedItems)
        {
            DialogueLua.SetVariable(VariableName + "ShippedItemName" + i, item.ContainedItem.Name);
            DialogueLua.SetVariable(VariableName + "ShippedItemType" + i, item.ContainedItem.Type);
            DialogueLua.SetVariable(VariableName + "ShippedItemAmount" + i, item.Amount);

            i++;
        }
    }
    void OnApplyPersistentData()
    {
        VariableName = GameManager.Instance.name;
        TargetManager = GetComponent<GameManager>();
        TargetManager.GameStarted = DialogueLua.GetVariable(VariableName + "GameStarted").asBool;

        if (DialogueLua.DoesVariableExist(VariableName + "NativeTreeID"))
        {
            TargetManager.NativeTree = TargetManager.PossibleNativeTrees[DialogueLua.GetVariable(VariableName + "NativeTreeID").asInt];
            TargetManager.NativeFruit = TargetManager.NativeTree.ProduceOutputs.Items[0].Item;
        }
        int shippedItemsAmt = DialogueLua.GetVariable(VariableName + "ShippedItemsAmount").AsInt;
        TargetManager.ShippedItems.Clear();
        for (int i = 0; i < shippedItemsAmt; i++)
        {
            string itemID = DialogueLua.GetVariable(VariableName + "ShippedItemName" + i).AsString;
            string itemType = DialogueLua.GetVariable(VariableName + "ShippedItemType" + i).AsString;
            ItemSystem.ItemTypes type = (ItemSystem.ItemTypes)System.Enum.Parse(typeof(ItemSystem.ItemTypes), itemType);
            ItemBase item = ItemSystem.Instance.GetItemClone(itemID);
            print("got item: " + item.Name);
            ShippedItem newShippedItem = new ShippedItem();
            newShippedItem.ContainedItem = item;
            int amount = DialogueLua.GetVariable(VariableName + "ShippedItemAmount" + i).AsInt;
            TargetManager.AddShippedItem(item, amount);
        }

    }

}
