using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPCs
{
    [System.Serializable]

    [CreateAssetMenu(fileName = "Data/NPCTrait")]
    public class NPCTraitScriptableObject : ScriptableObject
    {
        public string Name;
        [SerializeField]
        public List<NPCTraitCompatibility> Compatibilities;

    }
}