using System;
using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{
    [Serializable]
    public class ItemCrafted : ItemBase
    {

		public List<CraftingIngredient> Ingredients = new List<CraftingIngredient>();

		public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
			ItemCrafted crafted = (ItemCrafted)itemToChangeTo;
			Ingredients = crafted.Ingredients;

        }
	

    }
}