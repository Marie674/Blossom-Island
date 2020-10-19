using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Game.Items
{
    [System.Serializable]
    public class ItemRecipe : ItemBase
    {
        [Header("Specific Properties")]

        [PreviewField]
        public RecipeContainer Recipe;
        public override ItemBase Clone(ItemBase itemToClone)
        {
            ItemRecipe itemRecipe = itemToClone as ItemRecipe;
            ItemRecipe newItem = base.Clone(itemToClone) as ItemRecipe;
            if (newItem == null)
            {
                return null;
            }
            if (itemRecipe.Recipe != null)
            {
                newItem.Recipe = itemRecipe.Recipe;
            }
            return newItem;
        }
        public override void Use()
        {

        }
    }
}
