using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour {

	public string Name;
	public string Description;

	public bool Active=false;

	bool Initialized = false;

	public List<StatusEffectConditionBase> Conditions;

	void Start(){
		gameObject.name = Name;
	}

	public void Init(){
		Initialized = true;
		TimeManager.OnMinuteChanged += Tick;
	}

	void OnEnable(){
		if (Initialized == true) {
			TimeManager.OnMinuteChanged += Tick;
		}
	}
		
	void OnDisable(){
		if (Active) {
			Deactivate ();
		}
		TimeManager.OnMinuteChanged -= Tick;
	}
		
	void Tick(){
		CheckConditions ();
	}
	public delegate void StatusEffectActive ();
	public event StatusEffectActive OnStatusEffectActivated;

	void Activate(){
		Active = true;
		if (OnStatusEffectActivated != null) {
			OnStatusEffectActivated();
		}
//		print ("Activate effect " + Name);
	}

	public delegate void StatusEffectDeactivate ();
	public event StatusEffectActive OnStatusEffectDeactivated;

	void Deactivate(){
		Active = false;
		if (OnStatusEffectDeactivated != null) {
			OnStatusEffectDeactivated();
		}
//		print ("Deactivate effect " + Name);
	}

	//Check if all conditions are met
	void CheckConditions(){
		foreach(StatusEffectConditionBase condition in Conditions){
			if (condition.IsMet() == false){
				if (Active == true) {
					Deactivate ();
				}
				return;
			}
		}
		if (Active == false) {
			Activate ();
		}
	}

}
