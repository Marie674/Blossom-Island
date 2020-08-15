using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs.Blossoms
{
    public class HutSaver : MonoBehaviour
    {

        Hut TargetHut;
        private string VariableName = "";
        void Start()
        {
            TargetHut = GetComponent<Hut>();
            VariableName = TargetHut.Name;
        }
        void OnEnable()
        {
            PersistentDataManager.RegisterPersistentData(this.gameObject);
        }

        void OnDisable()
        {
            PersistentDataManager.RegisterPersistentData(this.gameObject);
        }

        public void OnRecordPersistentData()
        {
            TargetHut = GetComponent<Hut>();
            VariableName = TargetHut.Name;

            DialogueLua.SetVariable(VariableName + "Blossom", TargetHut.ContainedBlossom);
            DialogueLua.SetVariable(VariableName + "X", transform.position.x);
            DialogueLua.SetVariable(VariableName + "Y", transform.position.y);
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutX", transform.position.x);
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutY", transform.position.y);
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutName", TargetHut.Name);
        }

        public void OnApplyPersistentData()
        {
            TargetHut = GetComponent<Hut>();
            VariableName = TargetHut.Name;
            //  print(TargetHut.Name);
            TargetHut.ContainedBlossom = DialogueLua.GetVariable(VariableName + "Blossom").AsString;
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutX", transform.position.x);
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutY", transform.position.y);
            DialogueLua.SetVariable(TargetHut.ContainedBlossom + "HutName", TargetHut.Name);
        }

    }
}

