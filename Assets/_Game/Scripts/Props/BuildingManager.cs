using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> {

	public List<ConstructionSite> ReadySites;
	public void ShowConstructionMenu(){
		(GameObject.FindObjectOfType (typeof(ConstructionMenu)) as ConstructionMenu).Setup ();
	}
}
