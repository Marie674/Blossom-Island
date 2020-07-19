using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{

    public float SleepRegen = 8;
    public float EnergyRegen = 10;
    public float NeedChangeRate = 0.5f;
    private NeedBase EnergyNeed;
    private NeedBase SleepNeed;
    public AmountInputUI AmountUI;

    public int MinHours = 0;
    public int MaxHours = 12;
    public string UIText = "h";
    public string PromptText = "Sleep how long?";

    void Start()
    {
        AmountUI = FindObjectOfType<AmountInputUI>();
    }
    public void Sleep(int Hours)
    {
        EnergyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        SleepNeed = PlayerNeedManager.Instance.GetNeed("Sleep");

        SleepNeed.AllowChange = false;
        EnergyNeed.AllowChange = false;
        PlayerNeedManager.Instance.GlobalChangeRate = NeedChangeRate;

        TimeManager.Instance.PassTime(Hours * 60);

        SleepNeed.AllowChange = true;
        EnergyNeed.AllowChange = true;
        SleepNeed.Change(SleepRegen * Hours);
        EnergyNeed.Change(EnergyRegen * Hours);
        //	PlayerNeedManager.Instance.GlobalChangeRate= 1;
    }



    public void Interact()
    {

        AmountUI.AcceptButton.onClick.AddListener(delegate { Sleep((int)AmountUI.ValueInput.CurrentValue); AmountUI.Close(); });
        AmountUI.Open("Sleep", PromptText, MinHours, MaxHours, 8, UIText);
    }

}
