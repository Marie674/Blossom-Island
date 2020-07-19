using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TemperatureThreshold
{
	public string Name;
	public float Temperature;
}

public class TemperatureNeed : NeedBase {

	public float OptimalValue=50;
	public float TargetValue;
	public float Insulation = 20;
	public string Suffix = "";

	public List<TemperatureThreshold> Thresholds = new List<TemperatureThreshold>();
	public TemperatureThreshold CurrentThreshold;

	public override void Init(){
		base.Init ();
		CurrentValue = OptimalValue;
		TimeManager.OnMinuteChanged += Tick;
		Initialized = true;
		Tick ();
	}
	protected override void ApplyChanges(){
		float newTarget = (BaseMaxValue/2) - ((OptimalValue-TargetValue) / Insulation);
//		print ("Target Value " + newTarget);
		CurrentValue = Mathf.Clamp (Mathf.Lerp (CurrentValue, newTarget, CurrentChangeRate * PlayerNeedManagerTarget.GlobalChangeRate), BaseMinValue, BaseMaxValue);
		foreach (TemperatureThreshold threshold in Thresholds) {
			if (CurrentValue <= threshold.Temperature) {
				CurrentThreshold = threshold;
				break;
			}
		}
	}

	public override void AddActiveEffect(StatusEffectNeedRelationship relationship){
		base.AddActiveEffect (relationship);
		Insulation += (relationship as StatusEffectNeedRelationshipTemperature).InsulationModification;
	}

	public override void RemoveActiveEffect(StatusEffectNeedRelationship relationship){
		base.RemoveActiveEffect (relationship);
		Insulation -= (relationship as StatusEffectNeedRelationshipTemperature).InsulationModification;
	}

	public override string GetValueText(){
		return CurrentThreshold.Name;
	}
}
