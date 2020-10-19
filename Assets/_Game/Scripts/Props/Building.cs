using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
[System.Serializable]
public struct BuildingMaterial
{
    public ItemBase ContainedItem;
    public int CurrentAmount;
    public int TargetAmount;
}

[System.Serializable]
public struct BuildingPhase
{
    public int BuildingTime;
    public GameObject Object;
}

public class Building : MonoBehaviour
{

    public List<BuildingPhase> Phases;
    public int CurrentPhase;
    public List<BuildingMaterial> Materials;
}
