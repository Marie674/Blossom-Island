using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GrowthStage
{
    public int TargetGrowth;
    public GameObject StageProp;
}
public class PropGrowth : MonoBehaviour
{
    [SerializeField]
    public List<GrowthStage> GrowthStages = new List<GrowthStage>();
    public int CurrentGrowth;
    void OnEnable()
    {
        TimeManager.OnDayChanged += Grow;
    }
    void OnDisable()
    {
        TimeManager.OnDayChanged -= Grow;
    }


    void Grow(int pDay)
    {
        CurrentGrowth++;
        CheckStage();
    }

    public void CheckStage()
    {
        foreach (GrowthStage stage in GrowthStages)
        {
            stage.StageProp.SetActive(false);
        }
        for (int i = GrowthStages.Count - 1; i >= 0; i--)
        {
            if (CurrentGrowth >= GrowthStages[i].TargetGrowth)
            {
                GrowthStages[i].StageProp.SetActive(true);
                return;
            }
        }
    }
}
