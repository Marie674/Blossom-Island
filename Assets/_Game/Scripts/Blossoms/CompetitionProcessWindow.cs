using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Game.NPCs.Blossoms
{


    public class CompetitionProcessWindow : MonoBehaviour
    {
        public TextMeshProUGUI ProcessText;

        public void Open(string pText)
        {
            ProcessText.text = pText;
            GetComponent<WindowToggle>().Open();
        }
        public void ShowResultScreen()
        {
            GetComponent<WindowToggle>().Close();
            BlossomCompetitionManager.Instance.ShowResultsScreen();
        }
    }
}