using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;



public class FarmingManager : Singleton<FarmingManager>
{
    public enum CropNames
    {
        Strawberry,
        Tomato,
    }

    public Dictionary<string, CropController> Crops = new Dictionary<string, CropController>();
    public int WaterAbsorptionTime = 7;

    public void Start()
    {
        Object[] crops = Resources.LoadAll("Crops", typeof(CropController)) as Object[];
        //		print(crops.Length);
        foreach (CropController crop in crops)
        {
            //			print(crop.Data);
            //			print("name: " + crop.Data.Name);
            Crops.Add(crop.Template.Name, crop);
        }
    }

    public CropController GetCrop(string pName)
    {
        CropController crop = null;
        Crops.TryGetValue(pName, out crop);
        return crop;
    }




}
