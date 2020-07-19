using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedUI : MonoBehaviour {
	public bool Reverse;
	public NeedBase Target;

	public string NeedName;
	public TextMeshProUGUI ValueText;
	public Text NameText;
	public Slider FillImage;

	public virtual void Init(){

		if (Target == null) {
			return;
		}
		if (NameText != null) {
			NameText.text = Target.Name;
		}
		Target.OnNeedChanged += UpdateUI;

	}
	// Use this for initialization
	void OnEnable () {
		if(Target!=null)
		Target.OnNeedChanged += UpdateUI;
	}
	void OnDisable(){
		Target.OnNeedChanged -= UpdateUI;
	}
	// Update is called once per frame
	protected virtual void UpdateUI () {
		if (Target == null) {
			return;
		}
		if (ValueText != null) {
			ValueText.text = Target.GetValueText ();
		}
		if(Reverse){
			FillImage.value = MapRangeExtension.MapRange (Target.CurrentValue, Target.BaseMinValue, Target.BaseMaxValue, 1f, 0f);

		}
		else{
			FillImage.value = MapRangeExtension.MapRange (Target.CurrentValue, Target.BaseMinValue, Target.BaseMaxValue, 0f, 1f);
		}

	}
}
