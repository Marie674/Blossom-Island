using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;
namespace Game.Blossoms
{

    public class BlossomStoreUI : MonoBehaviour
    {
        public Button AcceptButton;
        public TextMeshProUGUI AgilityText;
        public TextMeshProUGUI StrengthText;
        public TextMeshProUGUI IntellectText;
        public TextMeshProUGUI CharmText;

        public TextMeshProUGUI FeeText;

        private WindowToggle Window;


        void Start()
        {
            Window = GetComponent<WindowToggle>();
        }
        public void Close()
        {
            AcceptButton.onClick.RemoveAllListeners();
            Window.Close();
        }

        public void Open(string pBlossom, int pFee)
        {

            BlossomController blossom = BlossomManager.Instance.GetSpawnedBlossom(pBlossom).GetComponent<BlossomController>();

            float AgilityPotential = blossom.GetTruePotential(Stat.StatName.Agility);// DialogueLua.GetVariable(pBlossom + "AgilityPotential").asFloat;
            float AgilityLearningSpeed = blossom.GetTrueLearningSpeed(Stat.StatName.Agility);//DialogueLua.GetVariable(pBlossom + "AgilityLearningSpeed").asFloat;

            float StrengthPotential = blossom.GetTruePotential(Stat.StatName.Strength); //DialogueLua.GetVariable(pBlossom + "StrengthPotential").asFloat;
            float StrengthLearningSpeed = blossom.GetTrueLearningSpeed(Stat.StatName.Strength); //DialogueLua.GetVariable(pBlossom + "StrengthLearningSpeed").asFloat;

            float IntellectPotential = blossom.GetTruePotential(Stat.StatName.Intellect); //DialogueLua.GetVariable(pBlossom + "IntellectPotential").asFloat;
            float IntellectLearningSpeed = blossom.GetTrueLearningSpeed(Stat.StatName.Intellect); // DialogueLua.GetVariable(pBlossom + "IntellectLearningSpeed").asFloat;

            float CharmPotential = blossom.GetTruePotential(Stat.StatName.Charm); //DialogueLua.GetVariable(pBlossom + "CharmPotential").asFloat;
            float CharmLearningSpeed = blossom.GetTrueLearningSpeed(Stat.StatName.Charm); //DialogueLua.GetVariable(pBlossom + "CharmLearningSpeed").asFloat;

            AgilityText.text = "Agility: Potential: " + AgilityPotential.ToString("F2") + " Learning Speed: " + AgilityLearningSpeed.ToString("F2");
            StrengthText.text = "Strength: Potential: " + StrengthPotential.ToString("F2") + " Learning Speed: " + StrengthLearningSpeed.ToString("F2");
            IntellectText.text = "Intellect: Potential: " + IntellectPotential.ToString("F2") + " Learning Speed: " + IntellectLearningSpeed.ToString("F2");
            CharmText.text = "Charm: Potential: " + CharmPotential.ToString("F2") + " Learning Speed: " + CharmLearningSpeed.ToString("F2");

            FeeText.text = "Adoption Fee: " + pFee.ToString("F2");

            FeeText.color = new Color(0, 0.5f, 0);
            AcceptButton.interactable = true;

            if (GameManager.Instance.Player.GetComponent<PlayerInventory>().Gold < pFee)
            {
                FeeText.color = Color.red;
                AcceptButton.interactable = false;
            }
            if (BlossomManager.Instance.GetEmptyHut() == null)
            {
                DialogueManager.ShowAlert("No empty hut");
                AcceptButton.interactable = false;
            }



            Window.Open();
        }

        public void Toggle()
        {
            Window.Toggle();
        }
    }
}

