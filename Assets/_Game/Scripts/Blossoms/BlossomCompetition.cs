using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPCs.Blossoms
{
    [CreateAssetMenu(menuName = "Blossoms/Competition")]
    public class BlossomCompetition : ScriptableObject
    {
        public BlossomCompetitionManager.CompetitionNames Name;
        public Stat.StatName Stat1;
        public Stat.StatName Stat2;
        public Stat.StatName Stat3;
        public Stat.StatName Stat4;

        [SerializeField]
        public List<string> StartingText = new List<string>();

        public List<string> BadResultText = new List<string>();

        public List<string> GoodResultText = new List<string>();

        public List<string> EndText = new List<string>();
    }
}

