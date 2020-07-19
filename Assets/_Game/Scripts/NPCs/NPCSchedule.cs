using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScheduleSlot
{
    public int Hour;
    public int Minute;
    public string Level;
    public Vector2 Position;
}
[CreateAssetMenu(menuName = "NPC Schedule")]
public class NPCSchedule : ScriptableObject
{
    [SerializeField]
    public List<ScheduleSlot> Schedule = new List<ScheduleSlot>();

    public List<TimeManager.WeekDays> ActiveDays = new List<TimeManager.WeekDays>();
}
