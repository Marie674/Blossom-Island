using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Blossoms
{

    [System.Serializable]
    public struct StatInfluence
    {
        public Stat.StatName StatName;
        public Trait.StatInfluenceType InfluenceType;
        public float PercentageChange;
        public float ValueChange;

    }

    [CreateAssetMenu(menuName = "Blossoms/BlossomTrait")]

    public class Trait : ScriptableObject
    {

        public enum StatInfluenceType
        {
            Potential,
            LearningSpeed
        }
        public string Name;

        public int Probabilty;

        [SerializeField]
        public List<StatInfluence> StatInfluences = new List<StatInfluence>();
        public int EnergyChange;

    }
}
