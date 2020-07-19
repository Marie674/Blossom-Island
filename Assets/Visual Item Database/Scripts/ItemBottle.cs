using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class ItemBottle : ItemBase
    {

		[SerializeField]
		public int maxCharge=50;

		[SerializeField]
		public int currentCharge=50;

		public ItemFood HeldLiquid;
		public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
			ItemBottle bottle = (ItemBottle)itemToChangeTo;

			HeldLiquid = bottle.HeldLiquid;
			maxCharge = bottle.maxCharge;
			currentCharge = bottle.currentCharge;
        }
			

    }
}