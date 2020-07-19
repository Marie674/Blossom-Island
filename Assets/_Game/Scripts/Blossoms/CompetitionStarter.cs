using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


namespace Game.Blossoms
{


    public class CompetitionStarter : MonoBehaviour
    {
        public Actor ConversationActor;

        public void Interact()
        {
            if (BlossomCompetitionManager.Instance.CurrentCompetition == null)
            {
                return;
            }
            DialogueLua.SetVariable("CurrentCompetitionName", BlossomCompetitionManager.Instance.CurrentCompetition.Name.ToString());
            DialogueManager.StopConversation();
            if (BlossomCompetitionManager.Instance.CompetitionDone)
            {
                DialogueManager.StartConversation("Blossom Competition Over", transform);

            }
            else
            {
                DialogueManager.StartConversation("Blossom Competition", transform);

            }

        }
    }
}