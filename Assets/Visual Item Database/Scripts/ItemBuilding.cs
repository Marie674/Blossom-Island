using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class ItemBuilding: ItemBase
    {


		public GameObject BuildingPrefab;


        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
			ItemBuilding building = (ItemBuilding)itemToChangeTo;
			BuildingPrefab = building.BuildingPrefab;
        }

    }
}