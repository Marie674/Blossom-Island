using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.NPCs.Blossoms
{
    public class TrainingProp : MonoBehaviour
    {

        public Stat.StatName TargetStat;
        public float Training;

        public void Interact()
        {
            FindObjectOfType<TrainingBlossomPickerUI>().Open(TargetStat, this);
        }

        public void Train(string pBlossomID)
        {
            if (BlossomManager.Instance.GetSpawnedBlossom(pBlossomID) != null)
            {
                BlossomController blossom = BlossomManager.Instance.GetSpawnedBlossom(pBlossomID).GetComponent<BlossomController>();
                blossom.Train(this);
            }

        }

    }

}