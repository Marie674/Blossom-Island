using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class StoreDoor : DoorSceneWarp
{

    public string openingHour = "8AM";
    public string closingHour = "8PM";

    [SerializeField]
    public List<int> OpeningHours = new List<int>();
    public List<TimeManager.WeekDays> OpeningDays = new List<TimeManager.WeekDays>();
    public override void Interact()
    {
        int currentHour = TimeManager.Instance.CurrentHour;
        TimeManager.WeekDays currentDay = TimeManager.Instance.CurrentWeekDayName;
        if (OpeningHours.Contains(currentHour) && OpeningDays.Contains(currentDay))
        {
            base.Warp();
        }
        else
        {
            PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("Opening hours: " + openingHour + " - " + closingHour);
        }

    }

}
