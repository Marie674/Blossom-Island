using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGradient : MonoBehaviour
{
    void OnEnable(){
        TimeManager.OnMinuteChanged += Rotate;
    }

    void OnDisable(){
        TimeManager.OnMinuteChanged -= Rotate;
    }

    void Rotate(){
        transform.Rotate(new Vector3(0,0,360f/(24*60)));
    }
}
