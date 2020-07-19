using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
namespace Game.Blossoms
{


    public class TrainingBlossomPickerUI : MonoBehaviour
    {

        public TMPro.TextMeshProUGUI TitleText;
        public BlossomSelectionUI BlossomUIPrefab;
        public Transform BlossomContainer;
        public Transform ConfirmationWindow;

        string ChosenBlossom = string.Empty;
        TrainingProp ChosenProp;
        public void Close()
        {
            ConfirmationWindow.gameObject.SetActive(false);
            ChosenProp = null;
            ChosenBlossom = string.Empty;
            GetComponent<WindowToggle>().Close();
        }

        public void Open(Stat.StatName pStat, TrainingProp pProp)
        {

            TitleText.text = pStat.ToString() + " Training: Select a Blossom";
            BlossomDataSaver[] spawnedBlossoms = FindObjectsOfType<BlossomDataSaver>();
            foreach (BlossomDataSaver spawnedBlossom in spawnedBlossoms)
            {
                spawnedBlossom.OnRecordPersistentData();
            }

            BlossomSelectionUI[] children = BlossomContainer.GetComponentsInChildren<BlossomSelectionUI>();
            foreach (BlossomSelectionUI child in children)
            {
                Destroy(child.gameObject);
            }

            ChosenProp = pProp;


            foreach (string blossom in BlossomManager.Instance.OwnedBlossoms)
            {
                BlossomController selectedBlossom = BlossomManager.Instance.GetSpawnedBlossom(blossom).GetComponent<BlossomController>();

                float agility = DialogueLua.GetVariable(blossom + "AgilityValue").asFloat;
                float agilityPotential = selectedBlossom.GetTruePotential(Stat.StatName.Agility);


                float strength = DialogueLua.GetVariable(blossom + "StrengthValue").asFloat;
                float strengthPotential = selectedBlossom.GetTruePotential(Stat.StatName.Strength);

                float intellect = DialogueLua.GetVariable(blossom + "IntellectValue").asFloat;
                float intellectPotential = selectedBlossom.GetTruePotential(Stat.StatName.Intellect);

                float charm = DialogueLua.GetVariable(blossom + "CharmValue").asFloat;
                float charmPotential = selectedBlossom.GetTruePotential(Stat.StatName.Charm);


                bool pregnant = DialogueLua.GetVariable(blossom + "Pregnant").asBool;
                bool hungry = DialogueLua.GetVariable(blossom + "Hungry").asBool;

                float energy = DialogueLua.GetVariable(blossom + "Energy").asFloat;
                bool fatigued = energy <= 0 ? true : false;

                string name = DialogueLua.GetVariable(blossom + "Name").asString;

                BlossomData.BlossomGrowth growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(blossom + "Growth").asString);
                string color = DialogueLua.GetVariable(blossom + "Color").asString;
                Sprite portrait;
                if (growth == BlossomData.BlossomGrowth.Adult)
                {
                    portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).AdultPortrait;
                    print("BlossomColors/" + color);
                }
                else
                {
                    portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).BabyPortrait;
                }

                BlossomSelectionUI ui = Instantiate(BlossomUIPrefab, BlossomContainer);

                ui.SelectBtn.enabled = true;

                ui.BlossomNameText.text = name;
                ui.BlossomPortrait.sprite = portrait;


                switch (pStat)
                {
                    case Stat.StatName.Null:
                        Debug.LogError("No Stat Specified for this prop");
                        return;
                    case Stat.StatName.Agility:
                        if (agility >= agilityPotential)
                        {
                            ui.SelectBtn.enabled = false;
                            ui.AgilityAmtText.color = Color.red;
                        }
                        else
                        {
                            ui.AgilityAmtText.color = Color.blue;
                        }
                        break;
                    case Stat.StatName.Strength:
                        if (strength >= strengthPotential)
                        {
                            ui.SelectBtn.enabled = false;
                            ui.StrengthAmtText.color = Color.red;
                        }
                        else
                        {
                            ui.StrengthAmtText.color = Color.blue;
                        }
                        break;
                    case Stat.StatName.Charm:
                        if (charm >= charmPotential)
                        {
                            ui.SelectBtn.enabled = false;
                            ui.CharmAmtText.color = Color.red;
                        }
                        else
                        {
                            ui.CharmAmtText.color = Color.blue;

                        }
                        break;
                    case Stat.StatName.Intellect:
                        if (intellect >= intellectPotential)
                        {
                            ui.SelectBtn.enabled = false;
                            ui.CharmAmtText.color = Color.red;
                        }
                        else
                        {
                            ui.IntellectAmtText.color = Color.blue;
                        }
                        break;
                }

                ui.AgilityAmtText.text = agility.ToString("F2") + " / " + agilityPotential.ToString("F2");
                ui.StrengthAmtText.text = strength.ToString("F2") + " / " + strengthPotential.ToString("F2");
                ui.IntellectAmtText.text = intellect.ToString("F2") + " / " + intellectPotential.ToString("F2");
                ui.CharmAmtText.text = charm.ToString("F2") + " / " + charmPotential.ToString("F2");

                if (pregnant)
                {
                    ui.SelectBtn.enabled = false;

                    ui.PregnantIcon.enabled = true;
                }
                else
                {
                    ui.PregnantIcon.enabled = false;
                }

                if (hungry)
                {
                    ui.SelectBtn.enabled = false;
                    ui.HungryIcon.enabled = true;
                }
                else
                {
                    ui.HungryIcon.enabled = false;
                }

                if (fatigued)
                {
                    ui.SelectBtn.enabled = false;
                    ui.FatiguedIcon.enabled = true;
                }
                else
                {
                    ui.FatiguedIcon.enabled = false;
                }

                ui.SelectBtn.onClick.AddListener(
                    delegate ()
                    {
                        ChosenBlossom = blossom;
                        ShowConfirmationWindow();
                    }
                    );

            }

            GetComponent<WindowToggle>().Open();
        }

        void ShowConfirmationWindow()
        {
            ConfirmationWindow.gameObject.SetActive(true);
        }

        public void SelectBlossom()
        {
            if (ChosenProp != null && ChosenBlossom != string.Empty)
            {
                ChosenProp.Train(ChosenBlossom);
                Close();
            }
        }
    }


}