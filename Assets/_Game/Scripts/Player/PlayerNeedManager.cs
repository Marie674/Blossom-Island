using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeedManager : Singleton<PlayerNeedManager> {

	private NeedBase[] Needs;
	private StatusEffectNeedRelationship[] Relationships;
	private StatusEffect[] Effects;
	private StatusEffectConditionBase[] Conditions;

	[Tooltip ("Global need change rate, 0 to 1")]
	public float GlobalChangeRate = 1f;

	void Start () {
		Needs = transform.GetComponentsInChildren<NeedBase> ();
		foreach (NeedBase need in Needs) {
			need.Init ();
		}
		Conditions = transform.GetComponentsInChildren<StatusEffectConditionBase> ();
		foreach (StatusEffectConditionBase condition in Conditions) {
			condition.Init ();
		}
		Relationships = transform.GetComponentsInChildren<StatusEffectNeedRelationship> ();
		foreach (StatusEffectNeedRelationship relationship in Relationships) {
			relationship.Init ();
		}
		Effects = transform.GetComponentsInChildren<StatusEffect> ();
		foreach (StatusEffect effect in Effects) {
			effect.Init ();
		}
		//print ("End Need Manager");
	}

	public NeedBase GetNeed(string pNeedName){
		foreach (NeedBase need in Needs) {
			if (need.Name == pNeedName) {
				return need;
			}
		}
		return null;
	}


}
