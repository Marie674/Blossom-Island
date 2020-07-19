using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public void AddHour(){
        TimeManager.Instance.PassTime(60);
    }

    public void AddDay(){
        TimeManager.Instance.AddDays(1);
        TimeManager.Instance.PassTime(1);
    }

    public void AddMonth(){
        TimeManager.Instance.AddMonths(1);
        TimeManager.Instance.PassTime(1);
    }
}
