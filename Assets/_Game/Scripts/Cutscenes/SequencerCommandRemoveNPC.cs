

using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandRemoveNPC : SequencerCommand
    { // Rename to SequencerCommand<YourCommand>

        public void Start()
        {
            string npc = GetParameter(0);
            //            print(npc);
            Game.NPCs.NPCManager.Instance.RemoveSpawnedNPC(npc);
            Stop();
        }


        public void OnDestroy()
        {
            // Add your finalization code here. This is critical. If the sequence is cancelled and this
            // command is marked as "required", then only Start() and OnDestroy() will be called.
            // Use it to clean up whatever needs cleaning at the end of the sequencer command.
            // If you don't need to do anything at the end, you can delete this method.
        }

    }

}


/**/
