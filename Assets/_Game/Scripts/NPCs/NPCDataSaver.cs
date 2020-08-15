using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs
{


    public class NPCDataSaver : MonoBehaviour
    {
        NPCData TargetData;
        private string VariableName = "";

        public void OnEnable()
        {

            PersistentDataManager.RegisterPersistentData(this.gameObject);
        }

        public void OnDisable()
        {
            OnRecordPersistentData();
            PersistentDataManager.UnregisterPersistentData(this.gameObject);
        }

        public void OnRecordPersistentData()
        {
            //  print("RECORD");
            TargetData = GetComponent<NPCData>();

            VariableName = "NPC" + TargetData.NPCID;
            DialogueLua.SetVariable(VariableName + "CurrentLevel", TargetData.CurrentLevel);
            DialogueLua.SetVariable(VariableName + "CurrentX", TargetData.transform.position.x);
            DialogueLua.SetVariable(VariableName + "CurrentY", TargetData.transform.position.y);
            DialogueLua.SetVariable(VariableName + "Met", TargetData.Met);
            DialogueLua.SetVariable(VariableName + "GreetedToday", TargetData.GreetedToday);
            DialogueLua.SetVariable(VariableName + "CurrentAffection", TargetData.CurrentAffection);
            DialogueLua.SetVariable(VariableName + "CurrentAcquaintance", TargetData.CurrentAcquaintance);
        }
        public void OnApplyPersistentData()
        {
            TargetData = GetComponent<NPCData>();

            VariableName = "NPC" + TargetData.NPCID;
            TargetData.Met = DialogueLua.GetVariable(VariableName + "Met").asBool;
            TargetData.GreetedToday = DialogueLua.GetVariable(VariableName + "GreetedToday").asBool;
            TargetData.CurrentAffection = DialogueLua.GetVariable(VariableName + "CurrentAffection").asFloat;
            TargetData.CurrentAcquaintance = DialogueLua.GetVariable(VariableName + "CurrentAcquaintance").asFloat;
            if (DialogueLua.DoesVariableExist(VariableName + "CurrentLevel"))
            {
                TargetData.CurrentLevel = DialogueLua.GetVariable(VariableName + "CurrentLevel").asString;

            }
            if (DialogueLua.DoesVariableExist(VariableName + "CurrentX"))
            {
                TargetData.transform.position = new Vector2(DialogueLua.GetVariable(VariableName + "CurrentX").asFloat, DialogueLua.GetVariable(VariableName + "CurrentY").asFloat);

            }
        }
    }
}