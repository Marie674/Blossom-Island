using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGradient : MonoBehaviour
{
    void OnEnable()
    {
        TimeManager.OnMinuteChanged += Rotate;
    }

    void OnDisable()
    {
        TimeManager.OnMinuteChanged -= Rotate;
    }

    void Rotate()
    {
        // transform.Rotate(new Vector3(0, 0, 360f / (24 * 60)));
        int currentMinutes = (TimeManager.Instance.CurrentHour * 60) + TimeManager.Instance.CurrentMinute;
        float rot = MapRangeExtension.MapRange(currentMinutes, 0, 1440, 0, 360);
        transform.eulerAngles = new Vector3(0, 0, rot);
    }
}
