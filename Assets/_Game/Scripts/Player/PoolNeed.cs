using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolNeed : NeedBase {

	public override void Init(){
		base.Init ();
		CurrentValue = CurrentMaxValue;
		TimeManager.OnMinuteChanged += Tick;
		Initialized = true;
		Tick ();
	}

	public override string GetValueText(){
		return Mathf.Round(CurrentValue).ToString() + "/"+ Mathf.Round(BaseMaxValue).ToString();
	}

	protected override void ApplyChanges(){
		CurrentValue = Mathf.Clamp ((CurrentValue += ( (CurrentChangeRate / 100) * CurrentMaxValue) * PlayerNeedManagerTarget.GlobalChangeRate), CurrentMinValue, CurrentMaxValue);
	}

}

