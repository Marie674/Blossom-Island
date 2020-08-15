using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPCs
{


    [System.Serializable]
    public struct NPCTraitCompatibility
    {
        public PlayerTraitScriptableObject Trait;
        public float Modifier;
    }
}