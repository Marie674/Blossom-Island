using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffect : MonoBehaviour
{
    public List<WeatherManager.WeatherTypeName> WeatherTypes;
    public List<LevelInfo.LevelType> LevelTypes;

private ParticleSystem Particles;

    void Start(){
        Particles =   GetComponent<ParticleSystem>();
    }

    void OnEnable(){
		WeatherManager.OnWeatherChanged+= ToggleVFX;
        GameManager.OnSceneChanged+=ToggleVFX;
	}
	void OnDisable(){
		WeatherManager.OnWeatherChanged-= ToggleVFX;
        GameManager.OnSceneChanged-=ToggleVFX;
	}

    void ToggleVFX(){
        Particles =   GetComponent<ParticleSystem>();

        if(Particles == null){
           // Debug.LogError("Particle system does not exist on " + gameObject.name);
            return;
        }
        bool show = true;
        if(!WeatherTypes.Contains(WeatherManager.Instance.CurrentWeather.Name)){
            show=false;
        }
        if(!LevelTypes.Contains(GameManager.Instance.LevelInfo.Type)){
            show=false;
        }

//        print(gameObject.name + show);
        if(show==false){
           Particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else{
            Particles.Play(true);
        }

    }

}
