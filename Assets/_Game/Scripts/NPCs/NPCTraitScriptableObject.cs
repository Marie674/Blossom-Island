using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "ScriptableObject/NPCTrait")]
public class NPCTraitScriptableObject : ScriptableObject
{
    public string Name;
    [SerializeField]
    public List<NPCTraitCompatibility> Compatibilities;

}