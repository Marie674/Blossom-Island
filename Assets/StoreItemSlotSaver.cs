// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Game.Items;
// using PixelCrushers.DialogueSystem;
// using UnityEngine.SceneManagement;
// public class StoreItemSlotSaver : MonoBehaviour
// {
//     StoreItemSlot TargetSlot;
//     string VariableName;

//     public void OnEnable()
//     {

//         PersistentDataManager.RegisterPersistentData(this.gameObject);
//     }

//     public void OnDisable()
//     {
//         OnRecordPersistentData();
//         PersistentDataManager.UnregisterPersistentData(this.gameObject);
//     }


//     public void OnRecordPersistentData()
//     {
//         TargetSlot = GetComponent<StoreItemSlot>();
//         VariableName = SceneManager.GetActiveScene().name + "StoreSlot" + transform.position.x + transform.position.y;

//         DialogueLua.SetVariable(VariableName + "CurrentItemName", TargetSlot.CurrentItem.Name);
//         DialogueLua.SetVariable(VariableName + "CurrentItemType", TargetSlot.CurrentItem.Type);
//         DialogueLua.SetVariable(VariableName + "CurrentItemAmount", TargetSlot.CurrentAmount);
//         DialogueLua.SetVariable(VariableName + "MaxItemAmount", TargetSlot.MaxAmount);

//     }

//     void OnApplyPersistentData()
//     {
//         TargetSlot = GetComponent<StoreItemSlot>();
//         VariableName = SceneManager.GetActiveScene().name + "StoreSlot" + transform.position.x + transform.position.y;
//         if (DialogueLua.DoesVariableExist(VariableName + "CurrentItemName"))
//         {
//             string itemID = DialogueLua.GetVariable(VariableName + "CurrentItemName").AsString;
//             string itemType = DialogueLua.GetVariable(VariableName + "CurrentItemType").AsString;
//             ItemSystem.ItemTypes type = (ItemSystem.ItemTypes)System.Enum.Parse(typeof(ItemSystem.ItemTypes), itemType);
//             ItemBase item = ItemSystem.Instance.GetItemClone(itemID);
//             print("got item: " + item.Name);
//             TargetSlot.CurrentItem = item;
//             TargetSlot.CurrentAmount = DialogueLua.GetVariable(VariableName + "CurrentItemAmount").AsInt;
//             TargetSlot.MaxAmount = DialogueLua.GetVariable(VariableName + "MaxItemAmount").AsInt;
//             TargetSlot.UpdateVisuals();
//             if (TargetSlot.CurrentAmount > 0)
//             {
//                 TargetSlot.CanInteract = true;
//             }
//         }
//     }

// }