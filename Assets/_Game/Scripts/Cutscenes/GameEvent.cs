using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EventNPCLocation
{
    public string NPCID;
    public Vector2 Position;
    public Game.NPCs.CharacterDirection Facing;
}

[CreateAssetMenu(fileName = "GameEvent")]

public class GameEvent : ScriptableObject
{
    public string Name;

    [Tooltip("Conversation Database Title")]
    public string ConversationTitle;

    [Tooltip("Player start position for event")]
    public Vector2 PlayerPos;

    [Tooltip("Player start facing for event")]
    public PlayerCharacter.CharacterDirection PlayerFacing;

    [SerializeField]
    public List<EventNPCLocation> NPCLocations;

    public UnityEvent EventTriggers;

    [Tooltip("From when to when can this trigger? Set the start hour to -1 if it can start any time.")]
    [SerializeField]
    public HourSpan HourSpan;

    [Tooltip("What days can this trigger? Leave empty for any day.")]
    [SerializeField]
    public List<TimeManager.WeekDays> AllowedWeekdays = new List<TimeManager.WeekDays>();

    [Tooltip("What weathers can this trigger? Leave empty for any weather.")]

    [SerializeField]
    public List<WeatherType> AllowedWeathers = new List<WeatherType>();

    [Tooltip("What seasons can this trigger? Leave empty for any season.")]

    [SerializeField]
    public List<TimeManager.MonthNames> AllowedMonths = new List<TimeManager.MonthNames>();

    [Tooltip("Where does this event take place?")]

    public string Location;
    [Tooltip("What level does the player have to come from? Leave empty for any level.")]

    public string FromLocation = string.Empty;

    [Tooltip("Can this trigger during a festival?")]
    public bool FestivalAllowed = false;

    [Tooltip("Can this be added to the event queue and play right after another event?")]
    public bool CanQueue = true;

    [Tooltip("Can this be seen multiple times?")]
    public bool Replayable = false;

    [Tooltip("From when can this play?")]
    public int MinimumDays = 0;

    [Tooltip("List of events that must have been seen before this one.")]

    public List<GameEvent> PreRequisites = new List<GameEvent>();

    public bool CheckValidity()
    {
        if (CalendarManager.Instance.CurrentEvent != null && FestivalAllowed == false)
        {
            return false;
        }
        if (EventManager.Instance.PlayedEvents.Contains(this))
        {
            //    Debug.Log("already played");

            return false;
        }
        int currentDay = TimeManager.Instance.PassedDays;
        if (currentDay < MinimumDays)
        {
            //   Debug.Log("Too early");

            return false;
        }
        string currentLocation = GameManager.Instance.LevelName;
        string previousLocation = GameManager.Instance.PreviousLevelName;
        if (currentLocation != Location)
        {
            //    Debug.Log("wrong location");

            return false;
        }
        if (FromLocation != string.Empty && previousLocation != FromLocation)
        {
            //    Debug.Log("from wrong location");
            return false;
        }
        if (PreRequisites.Count > 0)
        {
            foreach (GameEvent prerequisite in PreRequisites)
            {
                if (EventManager.Instance.PlayedEvents.Contains(prerequisite) == false)
                {
                    //       Debug.Log("prerequisite not seen");

                    return false;
                }
            }
        }
        TimeManager.MonthNames currentMonth = TimeManager.Instance.CurrentMonth.Name;
        if (AllowedMonths.Count > 0 && AllowedMonths.Contains(currentMonth) == false)
        {
            //   Debug.Log("wrong season");

            return false;
        }
        TimeManager.WeekDays currentWeekday = TimeManager.Instance.CurrentWeekDayName;
        if (AllowedWeekdays.Count > 0 && AllowedWeekdays.Contains(currentWeekday) == false)
        {
            //    Debug.Log("wrong week day");

            return false;
        }
        WeatherType currentWeather = WeatherManager.Instance.CurrentWeather;
        if (AllowedWeathers.Count > 0 && AllowedWeathers.Contains(currentWeather) == false)
        {
            //    Debug.Log("wrong weather");

            return false;
        }
        int currentHour = TimeManager.Instance.CurrentHour;
        int currentMinute = TimeManager.Instance.CurrentMinute;
        if (HourSpan.StartHour != -1 && TimeManager.Instance.CheckWithinHourSpan(HourSpan, currentHour, currentMinute) == false)
        {
            //   Debug.Log("not within timespan");

            return false;
        }
        return true;
    }

    public void GiveStarterBlossom()
    {
        Game.NPCs.Blossoms.BlossomManager.Instance.GiveStarterBlossom();
    }

}
