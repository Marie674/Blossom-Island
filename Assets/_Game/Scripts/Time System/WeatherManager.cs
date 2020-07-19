using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct WeatherType
{
    public WeatherManager.WeatherTypeName Name;
    public bool Rainy;
    public int SunLevel;
    public int SpringProbability;
    public int SummerProbability;
    public int FallProbability;
    public int WinterProbability;

}

public class WeatherManager : Singleton<WeatherManager>
{

    public enum WeatherTypeName
    {
        Sunny,
        Cloudy,
        Rainy,
        Snowy,
        None
    }

    public List<WeatherType> WeatherTypes;

    [SerializeField]
    public WeatherType CurrentWeather;
    private TimeManager.MonthNames CurrentMonthName = TimeManager.MonthNames.Spring;

    public List<WeatherType> PastWeathers = new List<WeatherType>();

    public void Start()
    {
        GetNewWeather();

    }

    void OnEnable()
    {
        TimeManager.OnMonthChanged += SetMonth;
    }
    void OnDisable()
    {

        TimeManager.OnMonthChanged -= SetMonth;
    }

    void SetMonth(Month pCurrentMonth)
    {
        CurrentMonthName = pCurrentMonth.Name;
    }

    public delegate void WeatherChange();

    public static event WeatherChange OnWeatherChanged;

    public void GetNewWeather()
    {
        CalendarEntry currentEntry = CalendarManager.Instance.CurrentEntry;

        if (currentEntry != null && currentEntry.Weather != WeatherManager.WeatherTypeName.None)
        {

            foreach (WeatherType weather in WeatherTypes)
            {
                if (weather.Name == currentEntry.Weather)
                {
                    CurrentWeather = weather;
                    PastWeathers.Add(CurrentWeather);
                }
            }
            if (CurrentWeather.Name != WeatherTypeName.None)
            {
                if (OnWeatherChanged != null)
                {
                    OnWeatherChanged();
                }
                return;
            }
        }
        var weights = new Dictionary<WeatherType, int>();

        switch (CurrentMonthName)
        {
            case TimeManager.MonthNames.Spring:
                foreach (WeatherType weather in WeatherTypes)
                {
                    weights.Add(weather, weather.SpringProbability);
                }
                break;
            case TimeManager.MonthNames.Summer:
                foreach (WeatherType weather in WeatherTypes)
                {
                    weights.Add(weather, weather.SummerProbability);
                }
                break;

            case TimeManager.MonthNames.Fall:
                foreach (WeatherType weather in WeatherTypes)
                {
                    weights.Add(weather, weather.FallProbability);
                }
                break;

            case TimeManager.MonthNames.Winter:
                foreach (WeatherType weather in WeatherTypes)
                {
                    weights.Add(weather, weather.WinterProbability);
                }
                break;
            default:
                break;
        }

        CurrentWeather = WeightedRandomizer.From(weights).TakeOne();
        PastWeathers.Add(CurrentWeather);

        if (OnWeatherChanged != null)
        {
            OnWeatherChanged();
        }
    }
}
