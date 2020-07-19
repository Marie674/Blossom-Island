using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using ItemSystem;
using PixelCrushers.DialogueSystem;

public class ConstructionSign : MonoBehaviour {

	public ConstructionSite Site;
	// Use this for initialization
	private SupplyBoxUI SuppliesUI;

	void Start(){
		Site = GetComponentInParent<ConstructionSite> ();
	}

	public void Interact () {
		
		if (Site == null) {
			return;
		}

		if (Site.ReadyForContractor == false) {
			ShowSuppliesUI ();
			return;
		}

		else if (Site.ConstructionStarted == false) {
			DialogueManager.StartConversation ("Get Carpenter");
			return;
		}
	}

	void ShowSuppliesUI(){
		SuppliesUI = GameObject.FindObjectOfType<SupplyBoxUI>();
		SuppliesUI.TitleText.text = Site.name + " construction";
		SuppliesUI.Setup (Site.Building.Materials,Site);

	}

}
