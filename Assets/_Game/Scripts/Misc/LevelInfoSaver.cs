

using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

public class LevelInfoSaver : MonoBehaviour //<--Copy this file. Rename the file and class name.
{
    LevelInfo Parent;

    public void OnRecordPersistentData()
    {
        Parent = GetComponent<LevelInfo>();
        DialogueLua.SetVariable(Parent.Name + "LastMinutes", TimeManager.Instance.PassedMinutes);
        DialogueLua.SetVariable(Parent.Name + "LastHours", TimeManager.Instance.PassedHours);
        DialogueLua.SetVariable(Parent.Name + "LastDays", TimeManager.Instance.PassedDays);
        DialogueLua.SetVariable(Parent.Name + "LastMonths", TimeManager.Instance.PassedMonths);

    }

    public void Apply()
    {
        Parent = GetComponent<LevelInfo>();
        uint minutes = 0;
        uint hours = 0;
        uint days = 0;
        uint months = 0;

        if (DialogueLua.DoesVariableExist(Parent.Name + "LastMinutes"))
        {


            uint lastMinutes = (uint)DialogueLua.GetVariable(Parent.Name + "LastMinutes").asInt;
            uint lastHours = (uint)DialogueLua.GetVariable(Parent.Name + "LastHours").asInt;
            uint lastDays = (uint)DialogueLua.GetVariable(Parent.Name + "LastDays").asInt;
            uint lastMonths = (uint)DialogueLua.GetVariable(Parent.Name + "LastMonths").asInt;

            int lastSunny = DialogueLua.GetVariable(Parent.Name + "SunnyDays").asInt;
            int lastCloudy = DialogueLua.GetVariable(Parent.Name + "CloudyDays").asInt;
            int lastRainy = DialogueLua.GetVariable(Parent.Name + "RainyDays").asInt;
            int lastSnowy = DialogueLua.GetVariable(Parent.Name + "SnowyDays").asInt;

            minutes = (uint)TimeManager.Instance.PassedMinutes - lastMinutes;
            hours = (uint)TimeManager.Instance.PassedHours - lastHours;
            days = (uint)TimeManager.Instance.PassedDays - lastDays;
            months = (uint)TimeManager.Instance.PassedMonths - lastMonths;


        }
        else
        {
            minutes = (uint)TimeManager.Instance.PassedMinutes;
            hours = (uint)TimeManager.Instance.PassedHours;
            days = (uint)TimeManager.Instance.PassedDays;
            months = (uint)TimeManager.Instance.PassedMonths;

        }

        //        print("Passed minutes: " + minutes + " Passed hours: " + hours + " Passed days: " + days + " Passed months: " + months);
        TimeManager.Instance.CallEvents(minutes, hours, days, months);

    }

    public void OnApplyPersistentData()
    {

        StartCoroutine("ApplyTimer");

    }

    private IEnumerator ApplyTimer()
    {
        //        print("apply");
        yield return new WaitForSeconds(0.2f);
        Apply();

    }

    public void OnEnable()
    {
        Parent = GetComponent<LevelInfo>();

        // This optional code registers this GameObject with the PersistentDataManager.
        // One of the options on the PersistentDataManager is to only send notifications
        // to registered GameObjects. The default, however, is to send to all GameObjects.
        // If you set PersistentDataManager to only send notifications to registered
        // GameObjects, you need to register this component using the line below or it
        // won't receive notifications to save and load.
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        // Unsubscribe the GameObject from PersistentDataManager notifications:
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    //--- Uncomment this method if you want to implement it:
    //public void OnLevelWillBeUnloaded() 
    //{
    // This will be called before loading a new level. You may want to add code here
    // to change the behavior of your persistent data script. For example, the
    // IncrementOnDestroy script disables itself because it should only increment
    // the variable when it's destroyed during play, not because it's being
    // destroyed while unloading the old level.
    //}

}



/**/
