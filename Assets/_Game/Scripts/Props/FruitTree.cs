using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class FruitTree : TreeBase
{

    public ItemBase Fruit;
    public List<TimeManager.MonthNames> ActiveSeasons;

    public int HarvestDelay = 4;

    private int DaysSinceHarvested = 0;

    private bool FruitSpawned = false;

    public Transform[] SpawnPoints;

    private TimeManager TimeManagerRef;

    private bool CanProduce = false;

    protected void Start()
    {
        DaysSinceHarvested = Random.Range(0, HarvestDelay);
        if (DaysSinceHarvested == HarvestDelay && ActiveSeasons.Contains(TimeManager.Instance.CurrentMonth.Name))
        {
            SpawnFruit();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SeasonChange(TimeManager.Instance.CurrentMonth);
        TimeManager.OnMonthChanged += SeasonChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TimeManager.OnMonthChanged -= SeasonChange;
    }

    protected override void DayChange(int pDayIndex)
    {
        base.DayChange(pDayIndex);
        if (CanProduce == true)
        {
            DaysSinceHarvested++;
            if (DaysSinceHarvested >= HarvestDelay)
            {
                SpawnFruit();
            }
        }
    }

    private void SeasonChange(Month pCurrentMonth)
    {
        if (!ActiveSeasons.Contains(pCurrentMonth.Name))
        {
            DaysSinceHarvested = Random.Range(0, HarvestDelay);
            CanProduce = false;
        }
        else
        {
            CanProduce = true;
        }
    }

    private void SpawnFruit()
    {
        DaysSinceHarvested = 0;
        //		foreach (Transform spawnPoint in SpawnPoints) {
        //			var dropObj = Instantiate (Fruit);
        //			dropObj.transform.SetParent(null); // Drop item into the world
        //			dropObj.transform.position = spawnPoint.position;
        //			dropObj.gameObject.layer = InventorySettingsManager.instance.settings.itemWorldLayer;
        //			if(dropObj.GetComponent<ItemAmount>()!=null){
        //				dropObj.GetComponent<ItemAmount> ().Amount = 1;
        //			}
        //			dropObj.gameObject.SetActive(true);
        //			dropObj.GetComponent<InventoryItemBase> ().currentStackSize = 1;
        //		}
    }
}
