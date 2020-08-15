using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPCs
{


    public class NPCData : MonoBehaviour
    {
        public string NPCID;
        public string CurrentLevel;
        public Vector2 CurrentPosition;
        public CharacterDirection CurrentFacing;
        public bool Met = false;
        public bool GreetedToday = false;
        public float CurrentAffection;
        public float CurrentAcquaintance;

        public List<NPCSchedule> Schedules = new List<NPCSchedule>();
        NPCSchedule CurrentSchedule;

        public ScheduleSlot CurrentSlot;

        public ScheduleSlot PreviousSlot;

        public ScheduleSlot NextSlot;

        ScheduleSlot PastSlot;

        public bool SlotChanged = false;

        void Start()
        {
            GetCurrentPosition();
        }
        void GetCurrentSchedule()
        {
            for (int i = 0; i < Schedules.Count; i++)
            {
                NPCSchedule schedule = Schedules[i];
                TimeManager.WeekDays currentDay = TimeManager.Instance.CurrentWeekDayName;
                if (schedule.ActiveDays.Contains(currentDay))
                {
                    CurrentSchedule = schedule;
                    return;
                }
            }
        }

        void GetCurrentSlot()
        {
            if (CurrentSchedule == null)
            {
                return;
            }
            SlotChanged = false;

            int currentHour = TimeManager.Instance.CurrentHour;
            int currentMinute = TimeManager.Instance.CurrentMinute;
            int totalMinutes = (currentHour * 60) + currentMinute;
            for (int i = 0; i < CurrentSchedule.Schedule.Count; i++)
            {
                ScheduleSlot slot = CurrentSchedule.Schedule[i];
                ScheduleSlot nextSlot;


                //if this is the last slot
                if (i + 1 >= CurrentSchedule.Schedule.Count)
                {
                    CurrentSlot = slot;
                    PreviousSlot = CurrentSchedule.Schedule[i - 1];
                    nextSlot = CurrentSchedule.Schedule[0];

                    if (PastSlot.Level != CurrentSlot.Level || PastSlot.Position != CurrentSlot.Position)
                    {
                        SlotChanged = true;
                    }

                    PastSlot = CurrentSlot;
                    return;
                }
                else
                {
                    nextSlot = CurrentSchedule.Schedule[i + 1];
                }
                int scheduleMinutes = (slot.Hour * 60) + slot.Minute;
                int nextScheduleMinutes = (nextSlot.Hour * 60) + nextSlot.Minute;
                if (totalMinutes >= scheduleMinutes && totalMinutes < nextScheduleMinutes)
                {
                    CurrentSlot = slot;
                    if (i == 0)
                    {
                        PreviousSlot = CurrentSchedule.Schedule[CurrentSchedule.Schedule.Count - 1];
                    }
                    else
                    {
                        PreviousSlot = CurrentSchedule.Schedule[i - 1];

                    }
                    NextSlot = CurrentSchedule.Schedule[i + 1];

                    if (PastSlot.Level != CurrentSlot.Level || PastSlot.Position != CurrentSlot.Position)
                    {
                        SlotChanged = true;
                    }

                    PastSlot = CurrentSlot;
                    return;
                }
            }
        }

        public void GetCurrentPosition()
        {
            GetCurrentSchedule();
            GetCurrentSlot();
            if (CurrentSchedule == null)
            {
                return;
            }

            CurrentPosition = CurrentSlot.Position;
            CurrentFacing = CurrentSlot.Facing;
            CurrentLevel = CurrentSlot.Level;
        }

        public ScheduleSlot GetPreviousPosition()
        {
            if (CurrentSchedule == null)
            {
                Debug.LogError("No schedule defined");
            }
            GetCurrentSlot();
            int prevSlotIndex = 0;

            for (int i = 0; i < CurrentSchedule.Schedule.Count; i++)
            {
                ScheduleSlot slot = CurrentSchedule.Schedule[i];
                if (slot.Level == CurrentSlot.Level && slot.Position == CurrentSlot.Position)
                {
                    if (i == 0)
                    {
                        prevSlotIndex = CurrentSchedule.Schedule.Count - 1;
                    }
                    else
                    {
                        prevSlotIndex = i - 1;
                    }
                }
            }
            return CurrentSchedule.Schedule[prevSlotIndex];
        }

        public ScheduleSlot GetNextPosition()
        {
            if (CurrentSchedule == null)
            {
                Debug.LogError("No schedule defined");
            }
            GetCurrentSlot();
            int nextSlotIndex = 0;

            for (int i = 0; i < CurrentSchedule.Schedule.Count; i++)
            {
                ScheduleSlot slot = CurrentSchedule.Schedule[i];
                if (slot.Level == CurrentSlot.Level && slot.Position == CurrentSlot.Position)
                {
                    if (i == CurrentSchedule.Schedule.Count - 1)
                    {
                        nextSlotIndex = 0;
                    }
                    else
                    {
                        nextSlotIndex = i + 1;
                    }
                }
            }
            return CurrentSchedule.Schedule[nextSlotIndex];
        }

    }
}