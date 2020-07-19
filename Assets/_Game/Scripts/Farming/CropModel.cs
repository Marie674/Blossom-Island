using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;


public class CropModel : MonoBehaviour
{
    public int CurrentStage;
    public int CurrentStageGrowth;
    public bool WateredToday;
    public bool CurrentlyWatered;
    public int StageWaterLevel;
    public int StageSunLevel;
    public int CurrentWilt;
    public float Quality;
    public int TimeSinceWatered;
}
