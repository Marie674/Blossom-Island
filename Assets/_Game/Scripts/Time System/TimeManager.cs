using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using CreativeSpore.SuperTilemapEditor;

[System.Serializable]
public struct Month
{
    public TimeManager.MonthNames Name;
    public int Length;
    public int AverageTemperature;
    public Texture2D TerrainTexture;
}

public class TimeManager : Singleton<TimeManager>
{



    public enum MonthNames
    {
        Spring,
        Summer,
        Fall,
        Winter,
    }

    public string getHourAMPM()
    {
        string hourAMPM = "AM";
        if (CurrentHour >= 12)
        {
            hourAMPM = "PM";
        }
        return hourAMPM;
    }

    public List<Month> Months;

    private Dictionary<int, MonthNames> MonthNameByIndex = new Dictionary<int, MonthNames>(){
        {0,MonthNames.Spring},
        {1,MonthNames.Summer},
        {2,MonthNames.Fall},
        {3,MonthNames.Winter},
    };

    public enum WeekDays
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }

    private Dictionary<int, WeekDays> WeekDayNameByIndex = new Dictionary<int, WeekDays>(){
        {0,WeekDays.Sunday},
        {1,WeekDays.Monday},
        {2,WeekDays.Tuesday},
        {3,WeekDays.Wednesday},
        {4,WeekDays.Thursday},
        {5,WeekDays.Friday},
        {6,WeekDays.Saturday},
    };

    //Time passed since game beginning, these are never reset
    public int PassedMinutes = 0;
    public int PassedHours = 0;
    public int PassedDays = 0;
    public int PassedMonths = 0;
    public int PassedYears = 0;



    //Current time
    public int CurrentMinute = 59;
    public int CurrentHour = 10;
    public int CurrentDay = 1;
    public int CurrentWeekDay = 1;
    public int CurrentWeek = 1;
    public int CurrentMonthIndex = 0;
    public int CurrentYear = 1;

    public Month CurrentMonth;
    public WeekDays CurrentWeekDayName = WeekDays.Sunday;


    public List<MonthNames> PastSeasons = new List<MonthNames>();

    public AK.Wwise.State SpringState;
    public AK.Wwise.State SummerState;
    public AK.Wwise.State FallState;
    public AK.Wwise.State WinterState;


    void Start()
    {
        //	print ("Init Time Manager");
        CurrentMonth = Months[CurrentMonthIndex];

        // Time passes 60 times faster than in real life.
        PastSeasons.Add(CurrentMonth.Name);
        CalendarManager.Instance.SetEntry(CurrentMonth.Name, CurrentDay);
        AddDays(1);

    }

    public void ToggleTime(bool pToggle)
    {
        if (pToggle == true)
        {
            InvokeRepeating("TimePass", 0f, 1f);
        }
        else
        {
            CancelInvoke("TimePass");
        }
    }

    /////////////////////////////////////Does not assess needs, except for minutes. Use PassTime() or only increase minutes
    public void AddTime(int pMins = 0, int pHours = 0, int pDays = 0, int pMonths = 0, int pYears = 0)
    {

        for (int i = 0; i < pMins; i++)
        {
            IncreaseMinutes();
        }
        for (int i = 0; i < pHours; i++)
        {
            IncreaseHours();
        }
        for (int i = 0; i < pDays; i++)
        {
            IncreaseDays();
        }
        for (int i = 0; i < pMonths; i++)
        {
            IncreaseMonths();
        }
        for (int i = 0; i < pYears; i++)
        {
            IncreaseYears();
        }

    }



    public void AddHours(int pHours)
    {
        for (int i = 0; i < pHours; i++)
        {
            IncreaseHours();
        }
    }

    public void AddDays(int pDays)
    {
        for (int i = 0; i < pDays; i++)
        {
            IncreaseDays();
        }
    }

    public void AddMonths(int pMonths)
    {
        for (int i = 0; i < pMonths; i++)
        {
            IncreaseMonths();
        }
    }
    ////////////////////////////////////////////////////////

    public void PassTime(int pMins)
    {
        for (int i = 0; i < pMins; i++)
        {
            IncreaseMinutes();
        }
    }


    //Called once per second
    private void TimePass()
    {
        IncreaseMinutes();
    }

    public delegate void MinuteChange();
    public static event MinuteChange OnMinuteChanged;


    private void IncreaseMinutes()
    {
        PassedMinutes++;
        CurrentMinute++;
        if (CurrentMinute == 60)
        {
            CurrentMinute = 0;
            IncreaseHours();
        }
        if (OnMinuteChanged != null)
        {
            OnMinuteChanged();
        }
    }

    public void CallEvents(uint pMins, uint pHours, uint pDays, uint pMonths)
    {
        for (int i = 0; i < pMins; i++)
        {
            if (OnMinuteChanged != null)
            {
                OnMinuteChanged();
            }
        }

        for (int i = 0; i < pHours; i++)
        {
            if (OnHourChanged != null)
            {
                OnHourChanged();
            }
        }


        for (int i = (int)PassedDays + 1 - (int)pDays; i <= (int)PassedDays; i++)
        {
            if (OnDayChanged != null)
            {
                OnDayChanged(i);
            }
        }


        for (int i = 0; i < pMonths; i++)
        {
            if (OnMonthChanged != null)
            {
                OnMonthChanged(CurrentMonth);
            }
        }



    }

    public delegate void HourChange();
    public static event HourChange OnHourChanged;

    private void IncreaseHours()
    {
        PassedHours++;
        CurrentHour++;
        if (CurrentHour == 24)
        {
            CurrentHour = 0;
            IncreaseDays();
        }
        if (OnHourChanged != null)
        {
            OnHourChanged();
        }
    }

    public delegate void DayChange(int pDayIndex);
    public static event DayChange OnDayChanged;

    public delegate void WeekChange();
    public static event WeekChange OnWeekChanged;

    private void IncreaseDays()
    {
        PassedDays++;
        CurrentWeekDay++;
        CurrentDay++;
        if (CurrentWeekDay == 7)
        {
            CurrentWeekDay = 0;
            CurrentWeek++;
            if (OnWeekChanged != null)
            {
                OnWeekChanged();
            }
        }
        if (CurrentDay == CurrentMonth.Length + 1)
        {
            CurrentDay = 0;
            IncreaseMonths();
        }
        WeekDayNameByIndex.TryGetValue(CurrentWeekDay, out CurrentWeekDayName);

        CalendarManager.Instance.SetEntry(CurrentMonth.Name, CurrentDay);
        WeatherManager.Instance.GetNewWeather();
        PastSeasons.Add(CurrentMonth.Name);
        if (OnDayChanged != null)
        {
            OnDayChanged(PassedDays);
        }

    }

    public delegate void MonthChange(Month pCurrentMonth);
    public static event MonthChange OnMonthChanged;

    private void IncreaseMonths()
    {
        PassedMonths++;
        CurrentMonthIndex++;
        if (CurrentMonthIndex == Months.Count)
        {
            CurrentMonthIndex = 0;
            IncreaseYears();
        }
        CurrentMonth = Months[CurrentMonthIndex];

        switch (CurrentMonth.Name)
        {
            case MonthNames.Spring:
                SpringState.SetValue();
                break;
            case MonthNames.Summer:
                SummerState.SetValue();
                break;
            case MonthNames.Fall:
                FallState.SetValue();
                break;
            case MonthNames.Winter:
                FallState.SetValue();
                break;
            default:
                SpringState.SetValue();
                break;

        }
        if (OnMonthChanged != null)
        {
            OnMonthChanged(CurrentMonth);
        }
    }

    private void IncreaseYears()
    {
        PassedYears++;
        CurrentYear++;
    }

}