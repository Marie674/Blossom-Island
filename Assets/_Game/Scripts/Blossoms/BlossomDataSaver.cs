using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


namespace Game.Blossoms
{
    public class BlossomDataSaver : MonoBehaviour
    {

        BlossomData TargetData;
        private string VariableName = "";
        void Start()
        {
            TargetData = GetComponent<BlossomData>();
            VariableName = TargetData.ID;
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
            TargetData = GetComponent<BlossomData>();
            VariableName = TargetData.ID;

            DialogueLua.SetVariable(VariableName + "ID", TargetData.ID);
            //STATS
            DialogueLua.SetVariable(VariableName + "AgilityValue", TargetData.Agility.Value);
            DialogueLua.SetVariable(VariableName + "AgilityPotential", TargetData.Agility.Potential);
            DialogueLua.SetVariable(VariableName + "AgilityLearningSpeed", TargetData.Agility.LearningSpeed);

            DialogueLua.SetVariable(VariableName + "StrengthValue", TargetData.Strength.Value);
            DialogueLua.SetVariable(VariableName + "StrengthPotential", TargetData.Strength.Potential);
            DialogueLua.SetVariable(VariableName + "StrengthLearningSpeed", TargetData.Strength.LearningSpeed);

            DialogueLua.SetVariable(VariableName + "IntellectValue", TargetData.Intellect.Value);
            DialogueLua.SetVariable(VariableName + "IntellectPotential", TargetData.Intellect.Potential);
            DialogueLua.SetVariable(VariableName + "IntellectLearningSpeed", TargetData.Intellect.LearningSpeed);

            DialogueLua.SetVariable(VariableName + "CharmValue", TargetData.Charm.Value);
            DialogueLua.SetVariable(VariableName + "CharmPotential", TargetData.Charm.Potential);
            DialogueLua.SetVariable(VariableName + "CharmLearningSpeed", TargetData.Charm.LearningSpeed);


            //FAMILY
            if (TargetData.Parent1 != string.Empty)
            {
                DialogueLua.SetVariable(VariableName + "Parent1", TargetData.Parent1);
            }
            if (TargetData.Parent2 != string.Empty)
            {
                DialogueLua.SetVariable(VariableName + "Parent2", TargetData.Parent2);
            }

            DialogueLua.SetVariable(VariableName + "ChildAmount", TargetData.Children.Count);
            int i = 0;
            foreach (string child in TargetData.Children)
            {
                DialogueLua.SetVariable(VariableName + "Child" + i, child);
                i++;
            }

            DialogueLua.SetVariable(VariableName + "Name", TargetData.Name);
            DialogueLua.SetVariable(VariableName + "Age", TargetData.Age);

            DialogueLua.SetVariable(VariableName + "Growth", TargetData.Growth.ToString());
            DialogueLua.SetVariable(VariableName + "Affection", TargetData.Affection);
            DialogueLua.SetVariable(VariableName + "Energy", TargetData.Energy);
            DialogueLua.SetVariable(VariableName + "Pregnant", TargetData.Pregnant);
            DialogueLua.SetVariable(VariableName + "DaysPregnant", TargetData.DaysPregnant);
            DialogueLua.SetVariable(VariableName + "BabyID", TargetData.BabyID);
            DialogueLua.SetVariable(VariableName + "PetToday", TargetData.PetToday);
            DialogueLua.SetVariable(VariableName + "TalkedToday", TargetData.TalkedToday);
            DialogueLua.SetVariable(VariableName + "FedToday", TargetData.FedToday);
            DialogueLua.SetVariable(VariableName + "Hungry", TargetData.Hungry);
            DialogueLua.SetVariable(VariableName + "Happiness", TargetData.Happiness);

            DialogueLua.SetVariable(VariableName + "TraitAmount", TargetData.Traits.Count);
            i = 0;
            foreach (Trait trait in TargetData.Traits)
            {
                DialogueLua.SetVariable(VariableName + "Trait" + i, trait.Name);
                i++;
            }

            DialogueLua.SetVariable(VariableName + "CurrentLevel", TargetData.CurrentLevel);
            DialogueLua.SetVariable(VariableName + "CurrentX", TargetData.transform.position.x);
            DialogueLua.SetVariable(VariableName + "CurrentY", TargetData.transform.position.y);

            DialogueLua.SetVariable(VariableName + "Color", TargetData.Color);

            DialogueLua.SetVariable(VariableName + "ForSale", TargetData.ForSale);
        }
        public void ApplyPersistentData(string pID = null)
        {
            if (pID == null)
            {
                return;
            }
            TargetData = GetComponent<BlossomData>();
            VariableName = pID;
            TargetData.ID = pID;
            //STATS
            TargetData.Agility.Value = DialogueLua.GetVariable(VariableName + "AgilityValue").asFloat;
            TargetData.Agility.Potential = DialogueLua.GetVariable(VariableName + "AgilityPotential").asFloat;
            TargetData.Agility.LearningSpeed = DialogueLua.GetVariable(VariableName + "AgilityLearningSpeed").asFloat;

            TargetData.Strength.Value = DialogueLua.GetVariable(VariableName + "StrengthValue").asFloat;
            TargetData.Strength.Potential = DialogueLua.GetVariable(VariableName + "StrengthPotential").asFloat;
            TargetData.Strength.LearningSpeed = DialogueLua.GetVariable(VariableName + "StrengthLearningSpeed").asFloat;

            TargetData.Intellect.Value = DialogueLua.GetVariable(VariableName + "IntellectValue").asFloat;
            TargetData.Intellect.Potential = DialogueLua.GetVariable(VariableName + "IntellectPotential").asFloat;
            TargetData.Intellect.LearningSpeed = DialogueLua.GetVariable(VariableName + "IntellectLearningSpeed").asFloat;

            TargetData.Charm.Value = DialogueLua.GetVariable(VariableName + "CharmValue").asFloat;
            TargetData.Charm.Potential = DialogueLua.GetVariable(VariableName + "CharmPotential").asFloat;
            TargetData.Charm.LearningSpeed = DialogueLua.GetVariable(VariableName + "CharmLearningSpeed").asFloat;


            //FAMILY
            if (DialogueLua.DoesVariableExist(VariableName + "Parent1"))
            {
                TargetData.Parent1 = DialogueLua.GetVariable(VariableName + "Parent1").asString;
            }
            if (DialogueLua.DoesVariableExist(VariableName + "Parent2"))
            {
                TargetData.Parent2 = DialogueLua.GetVariable(VariableName + "Parent2").asString;
            }

            TargetData.ChildAmount = DialogueLua.GetVariable(VariableName + "ChildAmount").asInt;
            TargetData.Children.Clear();
            for (int i = 0; i < TargetData.ChildAmount; i++)
            {
                TargetData.Children.Add(DialogueLua.GetVariable(VariableName + "Child" + i).asString);
            }

            TargetData.Name = DialogueLua.GetVariable(VariableName + "Name").asString;
            TargetData.Age = DialogueLua.GetVariable(VariableName + "Age").asInt;

            BlossomData.BlossomGrowth growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(VariableName + "Growth").asString);
            TargetData.Growth = growth;

            TargetData.Affection = DialogueLua.GetVariable(VariableName + "Affection").asInt;
            TargetData.Energy = DialogueLua.GetVariable(VariableName + "Energy").asInt;
            TargetData.Pregnant = DialogueLua.GetVariable(VariableName + "Pregnant").asBool;
            TargetData.DaysPregnant = DialogueLua.GetVariable(VariableName + "DaysPregnant").asInt;
            TargetData.BabyID = DialogueLua.GetVariable(VariableName + "BabyID").asString;
            TargetData.PetToday = DialogueLua.GetVariable(VariableName + "PetToday").asBool;
            TargetData.TalkedToday = DialogueLua.GetVariable(VariableName + "TalkedToday").asBool;
            TargetData.FedToday = DialogueLua.GetVariable(VariableName + "FedToday").asBool;
            TargetData.Hungry = DialogueLua.GetVariable(VariableName + "Hungry").asBool;
            TargetData.Happiness = DialogueLua.GetVariable(VariableName + "Happiness").asInt;



            TargetData.TraitAmount = DialogueLua.GetVariable(VariableName + "TraitAmount").asInt;
            TargetData.Traits.Clear();


            for (int i = 0; i < TargetData.TraitAmount; i++)
            {
                string traitName = DialogueLua.GetVariable(VariableName + "Trait" + i).asString;
                TargetData.Traits.Add(Resources.Load<Trait>("BlossomTraits/" + traitName));
            }

            TargetData.CurrentLevel = DialogueLua.GetVariable(VariableName + "CurrentLevel").asString;
            Vector2 newPos = new Vector2();
            newPos.x = DialogueLua.GetVariable(VariableName + "CurrentX").asFloat;
            newPos.y = DialogueLua.GetVariable(VariableName + "CurrentY").asFloat;
            TargetData.transform.position = newPos;

            if (GetComponent<Usable>() != null)
            {
                GetComponent<Usable>().overrideName = TargetData.Name;

            }
            gameObject.name = "Blossom " + TargetData.name;

            TargetData.Color = DialogueLua.GetVariable(VariableName + "Color").asString;

            TargetData.ForSale = DialogueLua.GetVariable(VariableName + "ForSale").asBool;


            GetComponent<BlossomAppearance>().SetAppearance(TargetData.Growth, TargetData.Color);

            if (DialogueLua.DoesVariableExist(VariableName + "HutName"))
            {
                TargetData.HutName = DialogueLua.GetVariable(VariableName + "HutName").AsString;
                TargetData.HutPosition = new Vector2(DialogueLua.GetVariable(VariableName + "HutX").asFloat, DialogueLua.GetVariable(VariableName + "HutY").asFloat);
                TargetData.Hut = BlossomManager.Instance.GetHutObject(TargetData.HutName);
            }

        }


    }

}
