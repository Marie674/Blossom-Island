using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeedItemEffect
{
	public float Amount;
	public float Ticks;
}

public class NeedBase : MonoBehaviour {

	public string Name;

	public float CurrentValue;
	protected float PreviousValue=-10000;

	public float BaseMinValue = 0;
	public float CurrentMinValue;

	public float BaseMaxValue = 0;
	public float CurrentMaxValue;

	public float BaseChangeRate=0;
	public float CurrentChangeRate;

	public bool AllowChange = true;

	protected bool Initialized = false;

	private List<StatusEffectNeedRelationship> ActiveStatusEffects = new List<StatusEffectNeedRelationship>();

	protected PlayerNeedManager PlayerNeedManagerTarget;

	private List<NeedItemEffect> ActiveItemEffects = new List<NeedItemEffect>();

	void Start(){
		gameObject.name = Name;
	}

	public virtual void Init(){
		PlayerNeedManagerTarget = transform.parent.GetComponent<PlayerNeedManager> ();
		CurrentMinValue = BaseMinValue;
		CurrentMaxValue = BaseMaxValue;
		CurrentChangeRate = BaseChangeRate;
		Tick ();
	}

	void OnEnable(){
		if (Initialized == true) {
			TimeManager.OnMinuteChanged += Tick;
			Tick ();
		}
	}

	void OnDisable(){
		TimeManager.OnMinuteChanged -= Tick;
	}

	public float GetPercentage(){
		return (CurrentValue / CurrentMaxValue) * 100;
	}

	public virtual string GetValueText(){
		return CurrentValue.ToString ();
	}

	public delegate void NeedChange ();
	public event NeedChange OnNeedChanged;

	protected virtual void Tick(){
		
		PreviousValue = CurrentValue;

		if (AllowChange) {
			ApplyChanges ();
		}
		ApplyItemEffects ();
		if (PreviousValue != CurrentValue) {
			PreviousValue = CurrentValue;
			if (OnNeedChanged != null) {
				OnNeedChanged ();
			}
		}
		//print (Name + ": " + CurrentValue);
	}

	protected virtual void ApplyChanges(){
	}

	private void ApplyItemEffects(){
		if (ActiveItemEffects.Count > 0) {
			foreach (var effect in ActiveItemEffects) {
				Change (effect.Amount);
				effect.Ticks--;
				if (effect.Ticks <= 0) {
					ActiveItemEffects.Remove (effect);
				}
			}
		}
	}

	public virtual void AddActiveEffect(StatusEffectNeedRelationship relationship){
		ActiveStatusEffects.Add (relationship);
		CurrentMaxValue += relationship.MaxValueModification;
		CurrentChangeRate += relationship.ChangerateModificationPercentage / 100 * BaseChangeRate;
		CurrentChangeRate += relationship.ChangerateModificationAbsolute;
	}

	public virtual void RemoveActiveEffect(StatusEffectNeedRelationship relationship){
		ActiveStatusEffects.Remove (relationship);
		CurrentMaxValue -= relationship.MaxValueModification;
		CurrentChangeRate -= relationship.ChangerateModificationPercentage / 100 * BaseChangeRate;
		CurrentChangeRate -= relationship.ChangerateModificationAbsolute;
	}

	public void AddChange(float amount, uint ticks=0){
		if (ticks <= 0) {
			Change (amount);
		} else {
			NeedItemEffect effect = new NeedItemEffect ();
			effect.Amount = amount;
			effect.Ticks = ticks;
			ActiveItemEffects.Add (effect);
		}
	}
	public bool Change(float amount){
		if (AllowChange == false) {
			return false;
		}
		CurrentValue = Mathf.Clamp (CurrentValue + amount, CurrentMinValue, CurrentMaxValue);
		if (OnNeedChanged != null) {
			OnNeedChanged ();
		}
		return true;
	}

}
