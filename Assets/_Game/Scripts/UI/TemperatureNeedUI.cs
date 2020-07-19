using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemperatureNeedUI : NeedUI {


	public Image Arrow;

	public override void Init(){
		Target = PlayerNeedManager.Instance.GetNeed (NeedName);
		ValueText = transform.Find ("Value").GetComponent<TextMeshProUGUI>();
		NameText = transform.Find ("Name").GetComponent<Text>();
	//	Arrow = transform.Find ("TempArrow").GetComponent<Image>();

		if (Target == null) {
			return;
		}
		if (NameText != null) {
			NameText.text = Target.Name;
		}
		Target.OnNeedChanged += UpdateUI;

	}

	// Update is called once per frame
	protected override void UpdateUI () {
		if (Target == null) {
			return;
		}
		if (ValueText != null) {
			ValueText.text = Target.GetValueText ();
		}
		Vector2 pos = Arrow.GetComponent<RectTransform> ().anchoredPosition;
		pos.x = MapRangeExtension.MapRange (Target.CurrentValue, 37f, 47f, -32f, 32f);
		Arrow.GetComponent<RectTransform> ().anchoredPosition = pos;
	}
}
