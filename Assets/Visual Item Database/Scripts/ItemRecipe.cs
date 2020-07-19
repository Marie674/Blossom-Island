using System;
using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{
    [Serializable]
    public class ItemRecipe : ItemBase
    {
        public RecipeContainer Recipe;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            ItemRecipe recipeItem = (ItemRecipe)itemToChangeTo;
            Recipe = recipeItem.Recipe;

        }

    }
}