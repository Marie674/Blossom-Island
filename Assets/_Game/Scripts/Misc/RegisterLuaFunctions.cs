using UnityEngine;
using PixelCrushers.DialogueSystem;

public class RegisterLuaFunctions : MonoBehaviour
{


    void OnEnable()
    {
        Lua.RegisterFunction("GetWeather", this, typeof(RegisterLuaFunctions).GetMethod("GetWeather"));
        Lua.RegisterFunction("GetCurrentLevel", this, typeof(RegisterLuaFunctions).GetMethod("GetCurrentLevel"));
        Lua.RegisterFunction("GetWeekDay", this, typeof(RegisterLuaFunctions).GetMethod("GetWeekDay"));
        Lua.RegisterFunction("GetDayRange", this, typeof(RegisterLuaFunctions).GetMethod("GetDayRange"));
        Lua.RegisterFunction("GetMinuteRange", this, typeof(RegisterLuaFunctions).GetMethod("GetMinuteRange"));
        Lua.RegisterFunction("GetHourRange", this, typeof(RegisterLuaFunctions).GetMethod("GetHourRange"));
        Lua.RegisterFunction("GetMonth", this, typeof(RegisterLuaFunctions).GetMethod("GetMonth"));
        Lua.RegisterFunction("GetDay", this, typeof(RegisterLuaFunctions).GetMethod("GetDay"));
        Lua.RegisterFunction("GetHour", this, typeof(RegisterLuaFunctions).GetMethod("GetHour"));
        Lua.RegisterFunction("GetMinute", this, typeof(RegisterLuaFunctions).GetMethod("GetMinute"));
        Lua.RegisterFunction("GetYear", this, typeof(RegisterLuaFunctions).GetMethod("GetYear"));
        Lua.RegisterFunction("SpawnNPC", this, typeof(RegisterLuaFunctions).GetMethod("SpawnNPC"));
        Lua.RegisterFunction("RemoveNPC", this, typeof(RegisterLuaFunctions).GetMethod("RemoveNPC"));
        Lua.RegisterFunction("ChangeNPCAcquaintance", this, typeof(RegisterLuaFunctions).GetMethod("ChangeNPCAcquaintance"));
        Lua.RegisterFunction("ChangeNPCAffection", this, typeof(RegisterLuaFunctions).GetMethod("ChangeNPCAffection"));
        Lua.RegisterFunction("GiveStarterBlossom", this, typeof(RegisterLuaFunctions).GetMethod("GiveStarterBlossom"));
        Lua.RegisterFunction("TeachRecipe", this, typeof(RegisterLuaFunctions).GetMethod("TeachRecipe"));
        Lua.RegisterFunction("SetQuestActive", this, typeof(RegisterLuaFunctions).GetMethod("SetQuestActive"));
        Lua.RegisterFunction("EventDone", this, typeof(RegisterLuaFunctions).GetMethod("EventDone"));
        Lua.RegisterFunction("FadeIn", this, typeof(RegisterLuaFunctions).GetMethod("FadeIn"));
        Lua.RegisterFunction("FadeOut", this, typeof(RegisterLuaFunctions).GetMethod("FadeOut"));


    }

    void OnDisable()
    {
        Lua.UnregisterFunction("GetWeather");
        Lua.UnregisterFunction("GetCurrentLevel");
        Lua.UnregisterFunction("GetWeekDay");
        Lua.UnregisterFunction("GetDayRange");
        Lua.UnregisterFunction("GetMinuteRange");
        Lua.UnregisterFunction("GetHourRange");
        Lua.UnregisterFunction("GetMonth");
        Lua.UnregisterFunction("GetDay");
        Lua.UnregisterFunction("GetHour");
        Lua.UnregisterFunction("GetMinute");
        Lua.UnregisterFunction("GetYear");
        Lua.UnregisterFunction("SpawnNPC");
        Lua.UnregisterFunction("RemoveNPC");
        Lua.UnregisterFunction("ChangeNPCAcquaintance");
        Lua.UnregisterFunction("ChangeNPCAffection");
        Lua.UnregisterFunction("GiveStarterBlossom");
        Lua.UnregisterFunction("ReachRecipe");
        Lua.UnregisterFunction("SetQuestActive");
        Lua.UnregisterFunction("EventDone");
        Lua.UnregisterFunction("FadeIn");
        Lua.UnregisterFunction("FadeOut");
    }

    public void FadeIn(float pTime)
    {
        GameManager.Instance.FadeIn(pTime);
    }
    public void FadeOut(float pTime)
    {
        GameManager.Instance.FadeOut(pTime);
    }
    public void EventDone()
    {
        EventManager.Instance.EventDone();
    }
    public void SetQuestActive(string pQuest)
    {
        PixelCrushers.MessageSystem.SendMessage(this, "StartQuest", pQuest);
    }
    public void SpawnNPC(string pNPCID, double pX, double pY, string pFacing)
    {
        Vector2 position = new Vector2((float)pX, (float)pY);
        Game.NPCs.NPCManager.Instance.SpawnDummyNPC(pNPCID, position, pFacing);
    }

    public void GiveStarterBlossom()
    {
        Game.NPCs.Blossoms.BlossomManager.Instance.GiveStarterBlossom();
    }

    public void TeachRecipe(string pRecipeName)
    {
        CraftingManager.Instance.TeachRecipe(pRecipeName);
    }

    public void RemoveNPC(string pNPCID)
    {

        Game.NPCs.NPCManager.Instance.RemoveSpawnedDummyNPC(pNPCID);
    }

    public void ChangeNPCAffection(float pAmount, string pNPCID)
    {
        Game.NPCs.NPCManager.Instance.ChangeNPCAffection(pAmount, pNPCID);

    }
    public void ChangeNPCAcquaintance(float pAmount, string pNPCID)
    {
        Game.NPCs.NPCManager.Instance.ChangeNPCAcquaintance(pAmount, pNPCID);

    }
    public string GetCurrentLevel()
    {
        if (GameManager.Instance.LevelInfo != null)
        {
            return GameManager.Instance.LevelInfo.Name;

        }
        return string.Empty;
    }
    public string GetWeather()
    {
        return WeatherManager.Instance.CurrentWeather.Name.ToString();
    }

    public string GetWeekDay()
    {
        return TimeManager.Instance.CurrentWeekDayName.ToString();
    }
    public bool GetDayRange(int pMin, int pMax)
    {
        int day = TimeManager.Instance.CurrentDay;
        if (day >= pMin && day <= pMax)
        {
            return true;
        }
        return false;
    }
    public bool GetMinuteRange(int pMin, int pMax)
    {
        int minute = TimeManager.Instance.CurrentMinute;
        if (minute >= pMin && minute <= pMax)
        {
            return true;
        }
        return false;
    }

    public bool GetHourRange(int pMin, int pMax)
    {
        int hour = TimeManager.Instance.CurrentHour;
        if (hour >= pMin && hour <= pMax)
        {
            return true;
        }
        return false;
    }

    public string GetMonth()
    {
        return TimeManager.Instance.CurrentMonth.Name.ToString();
    }
    public int GetDay()
    {
        return TimeManager.Instance.CurrentDay;
    }

    public int GetHour()
    {
        return TimeManager.Instance.CurrentHour;
    }

    public int GetMinute()
    {
        return TimeManager.Instance.CurrentMinute;
    }

    public int GetYear()
    {
        return TimeManager.Instance.CurrentYear;

    }

}