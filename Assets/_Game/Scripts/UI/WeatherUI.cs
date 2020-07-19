using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherUI : MonoBehaviour {

	public TextMeshProUGUI WeatherText;
	public TextMeshProUGUI TempText;
	public Image WeatherIcon;

	private WeatherManager.WeatherTypeName  CurrentWeatherName = WeatherManager.WeatherTypeName.Sunny;

	void Start(){
		UpdateWeatherUI();
	}

	void OnEnable(){
		WeatherManager.OnWeatherChanged += UpdateWeatherUI;
	}

	void OnDisable(){
		WeatherManager.OnWeatherChanged -= UpdateWeatherUI;
	}

	public void UpdateWeatherUI(){
//		WeatherText.text = WeatherManager.Instance.CurrentWeather.Name.ToString ();

		Animator anim = WeatherIcon.GetComponent<Animator> ();

		ResetAnimBools ();

		switch (WeatherManager.Instance.CurrentWeather.Name) {

		case WeatherManager.WeatherTypeName.Cloudy:
			anim.SetBool ("cloudy",true);
			break;

		case WeatherManager.WeatherTypeName.Rainy:
			anim.SetBool ("rainy",true);
			break;

		case WeatherManager.WeatherTypeName.Snowy:
			anim.SetBool ("snowy",true);
			break;

		case WeatherManager.WeatherTypeName.Sunny:
			anim.SetBool ("sunny",true);
			break;

		default:
			break;
		}

	}

	private void ResetAnimBools(){
		Animator anim = WeatherIcon.GetComponent<Animator> ();
		anim.SetBool ("cloudy",false);
		anim.SetBool ("sunny",false);
		anim.SetBool ("rainy",false);
		anim.SetBool ("stormy",false);
		anim.SetBool ("snowy",false);
	}
}
