using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectNeedRelationship:MonoBehaviour
{
	public StatusEffect Effect;
	[Tooltip ("Change rate modification, in percentage, relative to the base change rate. For example, 100 doubles the change rate.")]
	public float ChangerateModificationPercentage;
	[Tooltip ("Change rate modification, per tick.")]
	public float ChangerateModificationAbsolute;
	[Tooltip ("Modififation to the current max value.")]
	public float MaxValueModification;

	private NeedBase TargetNeed;

	bool Initialized = false;

	void Start(){
		gameObject.name = Effect.name;
		TargetNeed = transform.parent.GetComponent<NeedBase> ();
	}

	public void Init(){
		Initialized = true;
		Effect.OnStatusEffectActivated += AddEffect;
		Effect.OnStatusEffectDeactivated += RemoveEffect;
	}

	void OnEnable(){
		if (Initialized == true) {
			Effect.OnStatusEffectActivated += AddEffect;
			Effect.OnStatusEffectDeactivated += RemoveEffect;
		}
	}

	void OnDisable(){
		Effect.OnStatusEffectActivated -= AddEffect;
		Effect.OnStatusEffectDeactivated -= RemoveEffect;
	}

	void AddEffect(){
		TargetNeed.AddActiveEffect (this);
	}

	void RemoveEffect(){
		TargetNeed.RemoveActiveEffect (this);
	}
}
