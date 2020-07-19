using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
namespace Game.Blossoms
{


    public class CompetitionBlossomPickerUI : MonoBehaviour
    {

        public Button BackBtn;
        public BlossomSelectionUI BlossomUIPrefab;
        public Transform BlossomContainer;
        public Transform ConfirmationWindow;

        public void Close()
        {
            ConfirmationWindow.gameObject.SetActive(false);

            GetComponent<WindowToggle>().Close();
        }

        public void StartCompetition()
        {
            Close();
            BlossomCompetitionManager.Instance.StartCompetition();
        }

        public void Open(float pMinAgility, float pMaxAgility, float pMinStrength, float pMaxStrength, float pMinIntellect, float pMaxIntellect, float pMinCharm, float pMaxCharm, CompetitionTierSelectUI prevWindow, List<CompetitioniTier> pTiers, BlossomCompetition pCompetition)
        {
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



            foreach (string blossom in BlossomManager.Instance.OwnedBlossoms)
            {
                float agility = DialogueLua.GetVariable(blossom + "AgilityValue").asFloat;
                float strength = DialogueLua.GetVariable(blossom + "StrengthValue").asFloat;
                float intellect = DialogueLua.GetVariable(blossom + "IntellectValue").asFloat;
                float charm = DialogueLua.GetVariable(blossom + "CharmValue").asFloat;

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


                if (pMinAgility == -1 || pMaxAgility == -1)
                {
                    ui.AgilityAmtText.text = agility.ToString("F2") + " / N/A";
                }
                else
                {
                    ui.AgilityAmtText.text = agility.ToString("F2") + " / " + pMinAgility + "-" + pMaxAgility;
                    if (agility < pMinAgility || agility > pMaxAgility)
                    {
                        ui.SelectBtn.enabled = false;
                        ui.AgilityAmtText.color = Color.red;
                    }
                    else
                    {
                        ui.AgilityAmtText.color = new Color(0, 0.5f, 0);

                    }
                }


                if (pMinStrength == -1 || pMaxStrength == -1)
                {
                    ui.StrengthAmtText.text = strength.ToString("F2") + " / N/A";
                }
                else
                {
                    ui.StrengthAmtText.text = strength.ToString("F2") + " / " + pMinStrength + "-" + pMaxStrength;

                    if (strength < pMinStrength || strength > pMaxStrength)
                    {
                        ui.SelectBtn.enabled = false;
                        ui.StrengthAmtText.color = Color.red;
                    }
                    else
                    {
                        ui.StrengthAmtText.color = new Color(0, 0.5f, 0);

                    }

                }

                if (pMinIntellect == -1 || pMaxIntellect == -1)
                {
                    ui.IntellectAmtText.text = intellect.ToString("F2") + " / N/A";
                }
                else
                {
                    ui.IntellectAmtText.text = intellect.ToString("F2") + " / " + pMinIntellect + "-" + pMaxIntellect;

                    if (intellect < pMinIntellect || intellect > pMaxIntellect)
                    {
                        ui.SelectBtn.enabled = false;
                        ui.IntellectAmtText.color = Color.red;
                    }
                    else
                    {
                        ui.IntellectAmtText.color = new Color(0, 0.5f, 0);

                    }
                }


                if (pMinCharm == -1 || pMaxCharm == -1)
                {
                    ui.CharmAmtText.text = charm.ToString("F2") + " / N/A";
                }
                else
                {

                    ui.CharmAmtText.text = charm.ToString("F2") + " / " + pMinCharm + "-" + pMaxCharm;
                    if (charm < pMinCharm || charm > pMaxCharm)
                    {
                        ui.SelectBtn.enabled = false;
                        ui.CharmAmtText.color = Color.red;
                    }
                    else
                    {
                        ui.CharmAmtText.color = new Color(0, 0.5f, 0);

                    }
                }

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
                    ui.HungryIcon.enabled = true;
                }
                else
                {
                    ui.HungryIcon.enabled = false;
                }

                if (fatigued)
                {
                    ui.FatiguedIcon.enabled = true;
                }
                else
                {
                    ui.FatiguedIcon.enabled = false;
                }

                ui.SelectBtn.onClick.AddListener(
                    delegate ()
                    {
                        BlossomCompetitionManager.Instance.SetBlossom(blossom);
                        ShowConfirmationWindow();
                    }
                    );

            }

            BackBtn.onClick.AddListener(
                delegate () {
                    Close();
                    prevWindow.Open(pTiers, pCompetition);
                }
            );

            GetComponent<WindowToggle>().Open();
        }

        void ShowConfirmationWindow()
        {
            ConfirmationWindow.gameObject.SetActive(true);
        }
    }


}