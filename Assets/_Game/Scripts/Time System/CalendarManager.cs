using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalendarManager : Singleton<CalendarManager>
{
    public CalendarEntry CurrentEntry = null;
    public Festival CurrentEvent = null;

    public CalendarEntry[] Entries;

    void OnEnable()
    {
        GameManager.OnSceneChanged += LevelChange;

    }

    void OnDisable()
    {
        GameManager.OnSceneChanged -= LevelChange;
    }

    public CalendarEntry SetEntry(TimeManager.MonthNames pMonth, int pDay)
    {
        Entries = Resources.LoadAll<CalendarEntry>("CalendarEntries/" + pMonth.ToString());

        foreach (CalendarEntry entry in Entries)
        {

            if (entry.Month == pMonth && entry.Day == pDay)
            {
                CurrentEntry = entry;
                CurrentEvent = CurrentEntry.Event;
                if (CurrentEvent != null)
                {
                    if (CurrentEvent.Competition != null)
                    {
                        Game.NPCs.Blossoms.BlossomCompetitionManager.Instance.SelectCompetition(CurrentEvent.Competition.Name, CurrentEvent.Presenter.transform);

                    }

                }
                if (CurrentEvent != null)
                {
                    foreach (FestivalProp obj in CurrentEvent.Props)
                    {
                        if (GameManager.Instance.LevelName == obj.Level)
                        {
                            Instantiate(obj.Prop, obj.Position, transform.rotation);

                        }
                    }
                }
                return entry;
            }
        }
        CurrentEntry = null;
        CurrentEvent = null;

        return null;
    }

    void LevelChange()
    {
        if (CurrentEvent != null)
        {
            foreach (FestivalProp obj in CurrentEvent.Props)
            {
                if (GameManager.Instance.LevelName == obj.Level)
                {
                    Instantiate(obj.Prop, obj.Position, transform.rotation);

                }
            }
        }
    }

    public void SetEventDone()
    {

    }
}
