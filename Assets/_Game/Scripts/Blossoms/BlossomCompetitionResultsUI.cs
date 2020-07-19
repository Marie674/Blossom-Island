using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;

namespace Game.Blossoms
{

    public class BlossomCompetitionResultsUI : UIWindowBase
    {
        public GameObject ResultsPrefab;

        public TextMeshProUGUI WinnerText;
        public Image WinnerImage;
        public TextMeshProUGUI SecondText;
        public Image SecondImage;
        public TextMeshProUGUI ThirdText;
        public Image ThirdImage;

        public void Open(List<string> pResults,List<string> pBlossoms, int pRank, string pTitle = "", string pPrompt = "")
        {

            BlossomCompetition currentCompetition = BlossomCompetitionManager.Instance.CurrentCompetition;
            CompetitioniTier currentTier = BlossomCompetitionManager.Instance.CurrentTier;

           // BlossomColor[] allColors = Resources.LoadAll<BlossomColor>("BlossomColors");


            WinnerText.text = pResults[0];

            BlossomData.BlossomGrowth growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(pBlossoms[0] + "Growth").asString);
            string color = DialogueLua.GetVariable(pBlossoms[0] + "Color").asString;
            Sprite portrait;
            if (growth == BlossomData.BlossomGrowth.Adult)
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).AdultPortrait;
            }
            else
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).BabyPortrait;
            }

            WinnerImage.sprite = portrait;



            SecondText.text = pResults[1];

            growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(pBlossoms[1] + "Growth").asString);
            color = DialogueLua.GetVariable(pBlossoms[2] + "Color").asString;

            if (growth == BlossomData.BlossomGrowth.Adult)
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).AdultPortrait;
            }
            else
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).BabyPortrait;
            }


            ThirdText.text = pResults[2];

            growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(pBlossoms[2] + "Growth").asString);
            color = DialogueLua.GetVariable(pBlossoms[2] + "Color").asString;

            if (growth == BlossomData.BlossomGrowth.Adult)
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).AdultPortrait;
            }
            else
            {
                portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).BabyPortrait;
            }


            for (int i = 3; i <pResults.Count; i++)
            {
                GameObject result = Instantiate(ResultsPrefab, Content);

                growth = (BlossomData.BlossomGrowth)System.Enum.Parse(typeof(BlossomData.BlossomGrowth), DialogueLua.GetVariable(pBlossoms[i] + "Growth").asString);
                color = DialogueLua.GetVariable(pBlossoms[i] + "Color").asString;

                if (growth == BlossomData.BlossomGrowth.Adult)
                {
                    portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).AdultPortrait;
                }
                else
                {
                    portrait = Resources.Load<BlossomColor>("BlossomColors/" + color).BabyPortrait;
                }

                result.GetComponentInChildren<TextMeshProUGUI>().text = (i+1) + ": " + pResults[i];
            }

            base.Open(pTitle, pPrompt);
            CloseButton.onClick.AddListener(
                delegate ()
                {
                    Continue(pRank);
                }
                );
        }

        public void Continue(int pRank)
        {
            base.Close();
            BlossomCompetitionManager.Instance.ShowResultConversation(pRank);

        }

    }
}
