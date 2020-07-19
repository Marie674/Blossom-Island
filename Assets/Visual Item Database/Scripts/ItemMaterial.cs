using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class ItemMaterial : ItemBase
    {

		[SerializeField]
		public float burnTime=50;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
			ItemMaterial material = (ItemMaterial)itemToChangeTo;
			burnTime = material.burnTime;
        }

    }
}