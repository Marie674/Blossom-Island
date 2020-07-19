using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenu : MonoBehaviour {

	public VerticalLayoutGroup ConstructionsContainer;
	public ConstructionSiteUI ConstructionSiteUI;

	private ConstructionSiteUI SelectedUI;

	public void Setup(){
		ConstructionSiteUI[] children = ConstructionsContainer.GetComponentsInChildren<ConstructionSiteUI> ();

		foreach (ConstructionSiteUI child in children) {
			if(child!=ConstructionsContainer.transform)
				Destroy (child.gameObject);
		}


		foreach (ConstructionSite site in BuildingManager.Instance.ReadySites) {
			ConstructionSiteUI siteUI = Instantiate (ConstructionSiteUI, ConstructionsContainer.transform);
			//site.ItemIcon.sprite = site.Building
			siteUI.NameText.text = site.Building.name + " - " + site.LevelName;
			siteUI.Site = site;
			siteUI.ConfirmButton.onClick.AddListener (  delegate {
				SelectConstructionSite(siteUI);
			} );
		}

		GetComponent<WindowToggle> ().Toggle ();
	}

	public void SelectConstructionSite(ConstructionSiteUI pSite){
	
		SelectedUI = pSite;
	
	}

	public void StartConstruction(){
		SelectedUI.Site.StartConstruction();

	}

}
