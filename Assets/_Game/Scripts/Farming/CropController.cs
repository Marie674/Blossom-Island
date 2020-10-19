using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
public class CropController : MonoBehaviour
{

    public CropTemplate Template;
    public CropModel Model;
    public CropView View;

    public FarmPlot Plot;

    public Collider2D HarvestCollider;
    public void Plant(bool pPlotWatered, FarmPlot pPlot)
    {
        PixelCrushers.MessageSystem.SendMessage(gameObject, "Plant", Template.name);

        if (pPlotWatered)
        {
            Water();
        }
        Plot = pPlot;
        CheckForSun();
    }

    public void Load()
    {
        View.UpdateWaterGraphic(Model.CurrentlyWatered);
        View.UpdateCropGraphic(Model.CurrentStage);
        if (Model.CurrentStage >= Template.Stages.Count - 1)
        {
            HarvestCollider.enabled = true;
        }
    }

    void OnEnable()
    {
        TimeManager.OnHourChanged += HourPass;
        TimeManager.OnDayChanged += DayPass;
    }

    void OnDisable()
    {
        TimeManager.OnHourChanged -= HourPass;
        TimeManager.OnDayChanged -= DayPass;
    }

    protected void HourPass()
    {
        Model.TimeSinceWatered++;
        if (Model.CurrentlyWatered && Model.TimeSinceWatered >= FarmingManager.Instance.WaterAbsorptionTime)
        {
            Dry();
            CheckForRain();
        }
    }

    protected void DayPass(int pDayIndex)
    {
        if (Model.CurrentWilt >= Template.MaxWilt)
        {
            Wilt();
        }
        if (Model.WateredToday == true)
        {
            GrowDay();
        }
        Dry();
        print("Processing day: " + pDayIndex);
        CheckWeather(pDayIndex);
    }

    void CheckWeather(int pDayIndex)
    {

        WeatherType currentWeather = WeatherManager.Instance.PastWeathers[pDayIndex];

        Model.StageSunLevel += currentWeather.SunLevel;
        if (currentWeather.Rainy == true)
        {
            Water();
        }
    }

    protected void CheckForSun()
    {
        WeatherType currentWeather = WeatherManager.Instance.CurrentWeather;
        Model.StageSunLevel += currentWeather.SunLevel;
    }
    protected void Wilt()
    {
        DestroyCrop();
    }
    protected void GrowDay()
    {
        if (Model.WateredToday == false && Model.CurrentStage < Template.Stages.Count - 1)
        {
            Model.CurrentWilt++;
        }
        Model.WateredToday = false;
        Model.CurrentStageGrowth++;
        if (Model.CurrentStageGrowth >= Template.Stages[Model.CurrentStage].Length && Model.CurrentStage < Template.Stages.Count - 1)
        {
            GrowStage();
        }

    }
    protected void GrowStage()
    {
        Model.CurrentStageGrowth = 0;
        Model.CurrentStage = Mathf.Clamp(Model.CurrentStage + 1, 0, Template.Stages.Count - 1);
        Model.StageSunLevel = 0;
        CheckForSun();
        Model.StageWaterLevel = 0;

        CropStageTemplate stage = Template.Stages[Model.CurrentStage];
        if (Model.StageSunLevel >= stage.MinSun && Model.StageSunLevel <= stage.MaxSun)
        {
            if (Model.StageWaterLevel >= stage.MinWater && Model.StageWaterLevel <= stage.MaxWater)
            {
                Model.Quality = Mathf.Clamp(Model.Quality + Template.QualityPerStage, 0f, 3f);
            }

        }

        View.UpdateCropGraphic(Model.CurrentStage);
        if (Model.CurrentStage >= Template.Stages.Count - 1)
        {
            HarvestCollider.enabled = true;
        }
    }

    protected void ResetStage()
    {
        HarvestCollider.enabled = false;
        Model.CurrentStage = Model.CurrentStage - 1;
        Model.CurrentStageGrowth = 0;
        Model.StageSunLevel = 0;
        Model.StageWaterLevel = 0;
        WeatherType currentWeather = WeatherManager.Instance.CurrentWeather;

        if (currentWeather.Rainy == true)
        {
            Model.StageWaterLevel++;
        }
        View.UpdateCropGraphic(Model.CurrentStage);
    }

    public void Hit(string pMessage)
    {
        if (pMessage == "Sickle")
        {
            SickleCrop();
        }
        if (pMessage == "Hoe")
        {
            HoeCrop();
        }
    }

    public void Interact()
    {
        GrabCrop();
    }

    protected void HoeCrop()
    {
        DestroyCrop();
    }
    protected void SickleCrop()
    {
        if (Template.SickleHarvest == true)
        {
            HarvestCrop();
        }
    }

    protected void GrabCrop()
    {
        if (Template.SickleHarvest == true)
        {
            return;
        }

        HarvestCrop();
    }
    protected void HarvestCrop()
    {
        if (Model.CurrentStage < Template.Stages.Count - 1)
        {
            return;
        }
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "HarvestCrop", Template.Name);
        Output();
        if (Template.Regrows == true)
        {
            ResetStage();
        }
        else
        {
            DestroyCrop();
        }
    }
    protected void DestroyCrop()
    {
        Plot.CropDestroyed();
        Destroy(this.gameObject);
    }
    protected void CheckForRain()
    {
        WeatherType currentWeather = WeatherManager.Instance.CurrentWeather;
        if (currentWeather.Rainy == true)
        {
            Water();
        }
    }

    protected bool CheckCanWater()
    {
        if (Model.CurrentlyWatered == true)
        {
            return false;
        }
        return true;
    }

    public void Water()
    {
        if (CheckCanWater() == false)
        {
            return;
        }
        PixelCrushers.MessageSystem.SendMessage(GameManager.Instance.Player, "WaterCrop", Template.Name);

        Model.WateredToday = true;
        Model.CurrentlyWatered = true;
        Model.TimeSinceWatered = 0;
        Model.StageWaterLevel++;
        Model.CurrentWilt = 0;
        View.UpdateWaterGraphic(true);
    }

    protected void Dry()
    {
        Model.CurrentlyWatered = false;
        View.UpdateWaterGraphic(false);
    }
    protected void Output()
    {

        foreach (LootTable list in Template.Outputs)
        {
            List<ItemBase> items = list.Output();

            foreach (ItemBase item in items)
            {
                ItemSpawner.Instance.SpawnItems(item, transform.position);
            }

        }

    }

}
