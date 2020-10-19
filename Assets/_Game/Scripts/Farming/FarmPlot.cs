using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using PixelCrushers;

public class FarmPlot : MonoBehaviour
{

    public SpriteRenderer WaterSprite;

    public CropController Crop;
    public bool CurrentlyWatered;
    public int TimeSinceWatered;
    void Start()
    {
        CheckForRain();
    }
    void OnEnable()
    {
        TimeManager.OnDayChanged += DayChange;
        TimeManager.OnHourChanged += HourPass;
    }
    void OnDisable()
    {

        TimeManager.OnDayChanged -= DayChange;
        TimeManager.OnHourChanged -= HourPass;
    }

    public void Load()
    {
        SetCurrentlyWatered(CurrentlyWatered);
    }
    public bool PlantCrop(ItemTool pTool)
    {
        // if crop is already planted
        if (Crop != null)
        {
            return false;
        }

        string cropName = pTool.CropName;

        CropController CropPrefab = FarmingManager.Instance.GetCrop(cropName);
        if (CropPrefab == null)
        {
            Debug.LogError("Crop not found in manager");
            return false;
        }

        Crop = Instantiate(CropPrefab, transform.position, transform.rotation);
        Crop.Plant(CurrentlyWatered, this);

        return true;
    }

    public void CropDestroyed()
    {
        Crop = null;
    }

    void SetCurrentlyWatered(bool pToggle)
    {
        CurrentlyWatered = pToggle;
        WaterSprite.enabled = (pToggle);
    }

    protected void CheckForRain()
    {
        WeatherType currentWeather = WeatherManager.Instance.CurrentWeather;
        if (currentWeather.Rainy == true)
        {
            Water();
        }
    }

    public void Water()
    {
        SetCurrentlyWatered(true);
        TimeSinceWatered = 0;
    }

    protected void Dry()
    {
        SetCurrentlyWatered(false);
    }

    protected void HourPass()
    {
        TimeSinceWatered++;
        if (CurrentlyWatered && TimeSinceWatered >= FarmingManager.Instance.WaterAbsorptionTime)
        {
            Dry();
            CheckForRain();
        }
    }


    public void DayChange(int pDayIndex)
    {
        Dry();
        CheckForRain();
    }

}
