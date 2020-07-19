using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropStageTemplate")]
public class CropStageTemplate : ScriptableObject
{
    public int Length;
    public int MinWater;
    public int MaxWater;
    public int MinSun;
    public int MaxSun;
}
