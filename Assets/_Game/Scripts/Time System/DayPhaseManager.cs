using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
[System.Serializable]
public struct DayPhase
{
    public DayPhaseManager.DayPhaseNames Name;
    public Color SunColor;
    public float SunIntensity;
    public int TemperatureModifier;
    public List<int> DayPhaseHours;
    public DayPhaseManager.DayTiers Tier;
}

public class DayPhaseManager : Singleton<DayPhaseManager>
{

    public DayPhase CurrentDayPhase;

    public DayPhase PreviousDayPhase;

    private DayPhase NextDayPhase;

    public float LerpDuration = 5f;

    private float LerpTime = 5f;

    private bool CanLerp = false;

    Color TargetColor = Color.white;
    float TargetIntensity = 1f;

    Light2D Sun;

    public AK.Wwise.State DayState;
    public AK.Wwise.State NightState;

    public enum DayPhaseNames
    {
        Dawn,
        Morning,
        Midday,
        Afternoon,
        Evening,
        Night
    }

    public enum DayTiers
    {
        Morning,
        Day,
        Evening,
        Night
    }

    public List<DayPhase> Phases;

    //	void Awake(){
    //		Init ();
    //	}

    // Use this for initialization
    void Start()
    {
        LerpTime = 0;
        GetCurrentPhase();
    }

    void OnEnable()
    {
        Sun = GameManager.Instance.Sun;
        TimeManager.OnHourChanged += GetCurrentPhase;
    }

    void OnDisable()
    {
        TimeManager.OnHourChanged -= GetCurrentPhase;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float currentMinutes = (TimeManager.Instance.CurrentHour * 60) + TimeManager.Instance.CurrentMinute;
        float targetMinutes = NextDayPhase.DayPhaseHours[0] * 60f;

        float phaseStartTime = 0;
        float phaseEndTime = CurrentDayPhase.DayPhaseHours.Count * 60;
        currentMinutes = MapRangeExtension.MapRange(currentMinutes, 0, 24 * 60, phaseStartTime, phaseEndTime);

        float targetRed = MapRangeExtension.MapRange(currentMinutes, phaseStartTime, phaseEndTime, CurrentDayPhase.SunColor.r, NextDayPhase.SunColor.r);
        float targetGreen = MapRangeExtension.MapRange(currentMinutes, phaseStartTime, phaseEndTime, CurrentDayPhase.SunColor.g, NextDayPhase.SunColor.g);
        float targetBlue = MapRangeExtension.MapRange(currentMinutes, phaseStartTime, phaseEndTime, CurrentDayPhase.SunColor.b, NextDayPhase.SunColor.b);
        Sun.intensity = MapRangeExtension.MapRange(currentMinutes, phaseStartTime, phaseEndTime, CurrentDayPhase.SunIntensity, NextDayPhase.SunIntensity);
        Sun.color = new Color(targetRed, targetGreen, targetBlue);
    }

    public delegate void PhaseChange();
    public static event PhaseChange OnPhaseChange;

    public void GetCurrentPhase()
    {
        int i = 0;
        int nextPhaseIndex = 0;
        int previousPhaseIndex = 0;
        bool foundPhase = false;

        foreach (DayPhase phase in Phases)
        {

            if (phase.DayPhaseHours.Contains(TimeManager.Instance.CurrentHour))
            {
                CurrentDayPhase = phase;
                //				print (CurrentDayPhase.Name);
                foundPhase = true;
            }

            previousPhaseIndex = i - 1;
            nextPhaseIndex = i + 1;
            i++;
            if (foundPhase)
            {
                break;
            }

        }

        //Get next phase index
        if (nextPhaseIndex > Phases.Count - 1)
        {
            nextPhaseIndex = 0;
        }

        //Get previous phase index
        if (previousPhaseIndex < 0)
        {
            previousPhaseIndex = Phases.Count - 1;
        }

        PreviousDayPhase = Phases[previousPhaseIndex];

        NextDayPhase = Phases[nextPhaseIndex];

        TargetColor = CurrentDayPhase.SunColor;
        TargetIntensity = CurrentDayPhase.SunIntensity;

        //		print("Current Color: " + Sun.color);
        //		print("Target Color: " + TargetColor);

        if (CurrentDayPhase.Name == DayPhaseNames.Dawn || CurrentDayPhase.Name == DayPhaseNames.Evening || CurrentDayPhase.Name == DayPhaseNames.Night)
        {
            NightState.SetValue();
        }
        else { DayState.SetValue(); }

        if (OnPhaseChange != null)
        {
            OnPhaseChange();
        }

    }

    void LerpColor()
    {
        // Lerp from the previous phase's color to the current phase's color upon changing phases
        RenderSettings.ambientLight = Color.Lerp(PreviousDayPhase.SunColor, CurrentDayPhase.SunColor, LerpTime);
        if (LerpTime < LerpDuration)
        {
            LerpTime += Time.deltaTime / LerpDuration;
        }
        else
        {
            if (CanLerp == true)
            {
                CanLerp = false;
            }
        }
        //		print (LerpTime);

    }

}
