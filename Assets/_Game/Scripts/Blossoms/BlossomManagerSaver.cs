using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace Game.NPCs.Blossoms
{
    public class BlossomManagerSaver : MonoBehaviour
    {

        BlossomManager TargetManager;
        private string VariableName = "";


        void Start()
        {
            TargetManager = GetComponent<BlossomManager>();
            VariableName = "BlossomManager";
        }
        void OnEnable()
        {
            PersistentDataManager.RegisterPersistentData(this.gameObject);
            GameManager.OnSceneChanged += SceneChanged;
            GameManager.OnSceneUnloaded += BeforeSceneChange;


        }

        void OnDisable()
        {
            PersistentDataManager.RegisterPersistentData(this.gameObject);
            GameManager.OnSceneChanged -= SceneChanged;
            GameManager.OnSceneUnloaded -= BeforeSceneChange;

        }

        public void OnRecordPersistentData()
        {
            TargetManager = GetComponent<BlossomManager>();
            VariableName = "BlossomManager";

            DialogueLua.SetVariable(VariableName + "CurrentBlossomID", TargetManager.CurrentBlossomID);
            DialogueLua.SetVariable(VariableName + "OwnedBlossomAmount", TargetManager.OwnedBlossoms.Count);
            int i = 0;
            foreach (string blossom in TargetManager.OwnedBlossoms)
            {
                DialogueLua.SetVariable(VariableName + "OwnedBlossom" + i, blossom);
                i++;
            }

            DialogueLua.SetVariable(VariableName + "ExistingBlossomAmount", TargetManager.ExistingBlossoms.Count);
            i = 0;
            foreach (string blossom in TargetManager.ExistingBlossoms)
            {
                DialogueLua.SetVariable(VariableName + "ExistingBlossom" + i, blossom);
                i++;
            }

            // DialogueLua.SetVariable(VariableName + "HutAmount", TargetManager.HutAmount);
            // i = 0;
            // foreach (string hut in TargetManager.BlossomHuts)
            // {
            //     DialogueLua.SetVariable(VariableName + "Hut" + i, hut);
            //     i++;
            // }

            // DialogueLua.SetVariable(VariableName + "CompetitionDone", GetComponent<BlossomCompetitionManager>().CompetitionDone);
        }

        public void OnApplyPersistentData()
        {
            TargetManager = GetComponent<BlossomManager>();
            VariableName = "BlossomManager";

            if (DialogueLua.DoesVariableExist(VariableName + "OwnedBlossomAmount") == false)
            {
                return;
            }
            TargetManager.CurrentBlossomID = DialogueLua.GetVariable(VariableName + "CurrentBlossomID").asInt;
            TargetManager.OwnedBlossomAmount = DialogueLua.GetVariable(VariableName + "OwnedBlossomAmount").asInt;
            TargetManager.ExistingBlossomAmount = DialogueLua.GetVariable(VariableName + "ExistingBlossomAmount").asInt;

            TargetManager.OwnedBlossoms.Clear();
            for (int i = 0; i < TargetManager.OwnedBlossomAmount; i++)
            {
                TargetManager.OwnedBlossoms.Add(DialogueLua.GetVariable(VariableName + "OwnedBlossom" + i).asString);
            }
            TargetManager.ExistingBlossoms.Clear();
            for (int i = 0; i < TargetManager.ExistingBlossomAmount; i++)
            {
                TargetManager.ExistingBlossoms.Add(DialogueLua.GetVariable(VariableName + "ExistingBlossom" + i).asString);
            }

            // TargetManager.BlossomHuts.Clear();
            // for (int i = 0; i < TargetManager.HutAmount; i++)
            // {
            //     TargetManager.BlossomHuts.Add(DialogueLua.GetVariable(VariableName + "Hut" + i).asString);
            // }

            GetComponent<BlossomCompetitionManager>().CompetitionDone = DialogueLua.GetVariable(VariableName + "CompetitionDone").asBool;


        }
        void BeforeSceneChange()
        {
            DialogueLua.SetVariable(VariableName + "CompetitionDone", GetComponent<BlossomCompetitionManager>().CompetitionDone);

        }
        void SceneChanged()
        {
            GetComponent<BlossomCompetitionManager>().CompetitionDone = DialogueLua.GetVariable(VariableName + "CompetitionDone").asBool;

        }
    }
}
