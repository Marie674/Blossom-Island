using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterNeed : NeedBase {

	public override void Init(){
		base.Init ();
		CurrentValue = CurrentMaxValue;
		TimeManager.OnMinuteChanged += Tick;
		Initialized = true;
		Tick ();
	}

	protected override void ApplyChanges(){
		CurrentValue = Mathf.Clamp ((CurrentValue += ( (CurrentChangeRate / 100) * CurrentMaxValue) * PlayerNeedManagerTarget.GlobalChangeRate), CurrentMinValue, CurrentMaxValue);
	}

	public override string GetValueText(){
		return GetPercentage ().ToString ("F1") +"%";
	}

}
