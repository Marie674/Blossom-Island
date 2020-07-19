using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConditionBool : StatusEffectConditionBase {

	public bool MetBool = false;

	public override bool IsMet(){
		return MetBool;
	}

}
