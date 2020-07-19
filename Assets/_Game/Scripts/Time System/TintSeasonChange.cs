using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintSeasonChange : MonoBehaviour {

	public List<Color> SeasonColors;

    public SwayShader ShaderScript;

	// Use this for initialization
	void Start () {
        ShaderScript = GetComponent<SwayShader>();
		UpdateColor (TimeManager.Instance.CurrentMonth);
	}
	
	void OnEnable () {
		TimeManager.OnMonthChanged += UpdateColor;
	}

	void OnDisable () {
		TimeManager.OnMonthChanged -= UpdateColor;
	}

	void UpdateColor(Month pCurrentMonth){
		if (ShaderScript != null) {
			ShaderScript.Tint = SeasonColors [TimeManager.Instance.CurrentMonthIndex];
		}
	}
}
