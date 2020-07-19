using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Blossoms
{

    [System.Serializable]
    public struct ColorOutput
    {
        public BlossomColor Output;
        public int Probability;
    }

    [CreateAssetMenu(menuName = "Blossoms/BlossomColorMix")]
    public class BlossomColorMix : ScriptableObject
    {
        public BlossomColor Input1;
        public BlossomColor Input2;
        [SerializeField]
        public List<ColorOutput> Outputs = new List<ColorOutput>();
    }
}

