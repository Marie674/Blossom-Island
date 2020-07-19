using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectConditionNeed : StatusEffectConditionBase {

	public enum Operators
	{
		LessThan,
		GreaterThan,
		EqualTo,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo,
	}

	public NeedBase TargetNeed;
	public float NeedThreshold;
	public Operators Operator;
	public bool Percentage =false;


	void Start(){
		gameObject.name = TargetNeed.Name + " " + Operator.ToString() + " " + NeedThreshold;
	}
		

	public override bool IsMet(){
		if (Percentage) {
			switch (Operator) {
			case Operators.EqualTo:
				return TargetNeed.GetPercentage() == NeedThreshold ? true : false;
			case Operators.GreaterThan:
				return TargetNeed.GetPercentage() > NeedThreshold ? true : false;
			case Operators.LessThan:
				return TargetNeed.GetPercentage() < NeedThreshold ? true : false;
			case Operators.GreaterThanOrEqualTo:
				return TargetNeed.GetPercentage() >= NeedThreshold ? true : false;
			case Operators.LessThanOrEqualTo:
				return TargetNeed.GetPercentage() <= NeedThreshold ? true : false;
			default:
				return false;
			}
		} else {
			switch (Operator) {
			case Operators.EqualTo:
				return TargetNeed.CurrentValue == NeedThreshold ? true : false;
			case Operators.GreaterThan:
				return TargetNeed.CurrentValue > NeedThreshold ? true : false;
			case Operators.LessThan:
				return TargetNeed.CurrentValue < NeedThreshold ? true : false;
			case Operators.GreaterThanOrEqualTo:
				return TargetNeed.CurrentValue >= NeedThreshold ? true : false;
			case Operators.LessThanOrEqualTo:
				return TargetNeed.CurrentValue <= NeedThreshold ? true : false;
			default:
				return false;
			}
		}
	}

}
