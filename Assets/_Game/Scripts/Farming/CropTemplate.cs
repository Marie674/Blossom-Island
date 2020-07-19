using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CropTemplate")]

public class CropTemplate : ScriptableObject
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public bool Regrows;

    [SerializeField]
    public bool SickleHarvest = false;
    [SerializeField]
    public int MaxWilt;
    public List<string> ActiveSeasons;
    [SerializeField]
    public List<CropStageTemplate> Stages;
    [SerializeField]

    public float QualityPerStage;
    public List<LootTable> Outputs;


}
