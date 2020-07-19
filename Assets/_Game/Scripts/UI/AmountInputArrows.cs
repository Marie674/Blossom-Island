using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmountInputArrows : MonoBehaviour {

	public float Minvalue=0;
	public float MaxValue = 12;

	public float CurrentValue=6;

	public string TextToAdd = "";

	public TextMeshProUGUI TextUI;

	public void AddBtn(){
		CurrentValue = Mathf.Clamp (CurrentValue + 1, Minvalue, MaxValue);
		UpdateText();
	}
	public void RemoveBtn(){
		CurrentValue = Mathf.Clamp (CurrentValue - 1, Minvalue, MaxValue);
		UpdateText();
	}

	public void UpdateText(){
		TextUI.text = CurrentValue.ToString () + TextToAdd;
	}
}
