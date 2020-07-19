using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LightProp : MonoBehaviour
{

    public Light2D LightObject;
    public bool Flicker;
    public bool CanToggle;

    bool StartOff = false;
    public float TargetIntensity = 0.8f;
    public bool On = false;

    public bool AutoOnOff = false;

    [SerializeField]
    public List<DayPhaseManager.DayPhaseNames> OnPhases = new List<DayPhaseManager.DayPhaseNames>();

    public float FlickerIntensity = 0.2f;
    public float FlickerInterval = 0.075f;
    public float FlickerSpeed = 0.5f;

    float NewTarget = 0f;
    private void Start()
    {
        NewTarget = TargetIntensity;

        if (StartOff)
        {
            LightObject.intensity = 0;
        }
        else
        {
            StartCoroutine("TurnOn");
        }

    }

    void OnEnable()
    {
        TimeManager.OnHourChanged += CheckDayPhase;
    }
    void OnDisable()
    {
        TimeManager.OnHourChanged -= CheckDayPhase;
    }


    void CheckDayPhase()
    {
        if (AutoOnOff == false)
        {
            return;
        }
        if (OnPhases.Contains(DayPhaseManager.Instance.CurrentDayPhase.Name))
        {
            if (On == false)
            {
                StartCoroutine("TurnOn");
            }
        }
        else
        {
            if (On == true)
            {
                StartCoroutine("TurnOff");
            }
        }
    }
    public void Interact()
    {
        if (CanToggle)
        {
            Toggle();
        }
    }

    IEnumerator CheckFlicker()
    {
        StopCoroutine("DoFlicker");

        NewTarget = Random.Range(TargetIntensity - FlickerIntensity, TargetIntensity + FlickerIntensity);
        float randTime = Random.Range(FlickerInterval / 2, FlickerInterval * 2);
        StartCoroutine("DoFlicker", randTime);
        yield return null;
    }

    IEnumerator DoFlicker(float duration)
    {
        float startVal = LightObject.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            LightObject.intensity = Mathf.Lerp(startVal, NewTarget, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        LightObject.intensity = Mathf.Clamp(LightObject.intensity, 0, NewTarget);
        StartCoroutine("CheckFlicker");
    }


    void Toggle()
    {
        if (On)
        {
            StartCoroutine("TurnOff");
        }
        else
        {
            StartCoroutine("TurnOn");
        }
    }

    public void SetOn()
    {
        LightObject.intensity = TargetIntensity;
        if (Flicker == true)
        {
            NewTarget = TargetIntensity;

            StartCoroutine("CheckFlicker");
        }
        On = true;
    }

    public void SetOff()
    {
        if (Flicker == true)
        {
            StopCoroutine("CheckFlicker");
            StopCoroutine("DoFlicker");
        }

        LightObject.intensity = 0;
        On = false;
    }

    IEnumerator TurnOn()
    {
        while (LightObject.intensity < TargetIntensity)
        {
            LightObject.intensity = Mathf.Clamp(LightObject.intensity + Time.deltaTime * 10f, 0, TargetIntensity);
            yield return null;
        }
        if (Flicker == true)
        {
            NewTarget = TargetIntensity;

            StartCoroutine("CheckFlicker");
        }
        On = true;
        StopCoroutine("TurnOn");
    }

    IEnumerator TurnOff()
    {
        if (Flicker == true)
        {
            StopCoroutine("CheckFlicker");
            StopCoroutine("DoFlicker");
        }
        while (LightObject.intensity > 0)
        {
            LightObject.intensity = Mathf.Clamp(LightObject.intensity - Time.deltaTime * 10f, 0, TargetIntensity);
            yield return null;
        }
        On = false;

        StopCoroutine("TurnOff");
    }
}
