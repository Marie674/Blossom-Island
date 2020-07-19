using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[System.Serializable]
public struct BuildingMaterial
{
	public ItemContainer Item;
	public int CurrentAmount;
	public int TargetAmount;
}

[System.Serializable]
public struct BuildingPhase
{
	public int BuildingTime;
	public GameObject Object;
}

public class Building : MonoBehaviour {

	public List<BuildingPhase> Phases;
	public int CurrentPhase;
	public List<BuildingMaterial> Materials;
}
