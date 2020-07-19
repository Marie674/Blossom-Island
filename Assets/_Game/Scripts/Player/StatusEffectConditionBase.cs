using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConditionBase : MonoBehaviour {

	private StatusEffect TargetEffect;

	bool Initialized = false;

	public void Init(){
		TargetEffect = transform.parent.GetComponent<StatusEffect> ();
		if (!TargetEffect.Conditions.Contains (this)) {
			TargetEffect.Conditions.Add (this);
		}
		Initialized = true;
	}

	void OnEnable(){
		if (Initialized == true) {
			if (!TargetEffect.Conditions.Contains (this)) {
				TargetEffect.Conditions.Add (this);
			}
		}
	}

	void OnDisable(){
		if(TargetEffect.Conditions.Contains(this)){
			TargetEffect.Conditions.Remove (this);
		}
	}

	public virtual bool IsMet(){
		return false;
	}

}
