using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CalendarEntry")]

public class CalendarEntry : ScriptableObject
{
    public int Day;
    public TimeManager.MonthNames Month;

    public WeatherManager.WeatherTypeName Weather;
    public Festival Event;
}
