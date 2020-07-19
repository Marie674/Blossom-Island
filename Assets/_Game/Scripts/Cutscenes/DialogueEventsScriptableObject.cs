using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(fileName = "ScriptableObject/DialogueEventsScriptableObject")]
public class DialogueEventsScriptableObject : ScriptableObject
{
    public void SelectTier()
    {
        DialogueManager.StopConversation();
        Game.Blossoms.BlossomCompetitionManager.Instance.ShowTierSelect();
    }

    public void SkipCompetition()
    {
        //  BlossomCompetitionManager.Instance.SkipCompetition();

    }

}
