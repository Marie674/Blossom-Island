using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusEffectUI : MonoBehaviour {

	public StatusEffect Target;
	public GameObject Widget;
	public TextMeshProUGUI NameText;
	StatusEffectLayout Layout;

	void Start(){
		Layout = transform.parent.GetComponent<StatusEffectLayout> ();
		Layout.Sort();
	}
	// Use this for initialization
	void OnEnable () {
		if (Target == null) {
			return;
		}

		if (NameText != null) {
			NameText.text = Target.Name;
		}
		Target.OnStatusEffectActivated += ShowWidget;
		Target.OnStatusEffectDeactivated += HideWidget;
	}
	void OnDisable(){
		if (ApplicationIsQuitting) {
			return;
		}
		if(Target!=null){
		Target.OnStatusEffectActivated -= ShowWidget;
		Target.OnStatusEffectDeactivated -= HideWidget;	
		}
	}


	void ShowWidget(){
		Widget.SetActive (true);
		Layout.Sort();
	}

	void HideWidget(){
		Widget.SetActive (false);
		Layout.Sort();
	}

	private static bool ApplicationIsQuitting = false;

	public void OnDestroy () {
		ApplicationIsQuitting = true;
	}
}
