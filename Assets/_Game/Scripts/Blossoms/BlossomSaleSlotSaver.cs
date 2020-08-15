using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs.Blossoms
{


    public class BlossomSaleSlotSaver : MonoBehaviour
    {

        BlossomSaleSlot TargetSlot;
        private string VariableName = "";
        void Start()
        {
            TargetSlot = GetComponent<BlossomSaleSlot>();
            VariableName = "BlossomSaleSlot" + gameObject.name;
        }
        void OnEnable()
        {
            PersistentDataManager.RegisterPersistentData(this.gameObject);
        }

        void OnDisable()
        {
            OnRecordPersistentData();

            PersistentDataManager.RegisterPersistentData(this.gameObject);
        }
        public void OnRecordPersistentData()
        {
            TargetSlot = GetComponent<BlossomSaleSlot>();
            VariableName = "BlossomSaleSlot" + gameObject.name;
            DialogueLua.SetVariable(VariableName + "ContainedBlossom", TargetSlot.ContainedBlossom);
        }

        public void OnApplyPersistentData()
        {
            TargetSlot = GetComponent<BlossomSaleSlot>();
            VariableName = "BlossomSaleSlot" + gameObject.name;
            TargetSlot.ContainedBlossom = DialogueLua.GetVariable(VariableName + "ContainedBlossom").asString;
        }
    }
}