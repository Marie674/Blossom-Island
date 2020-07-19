using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blossoms
{

    [System.Serializable]
    public struct Stat
    {
        public enum StatName
        {
            Null,
            Strength,
            Agility,
            Intellect,
            Charm
        }
        public StatName Name;
        public float Value;
        public float Potential;
        public float LearningSpeed;
    }
}
