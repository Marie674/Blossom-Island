using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using Sirenix.OdinInspector;

[System.Serializable]
public class CraftingRecipe
{

    public List<CraftingIngredient> Ingredients = new List<CraftingIngredient>();
    public List<ItemOutput> Outputs;
    public float CraftingTime = 2;
    public bool NeedsToBeLearned = false;

    public CraftingRecipe Copy()
    {
        CraftingRecipe copy = new CraftingRecipe();
        copy.Ingredients = this.Ingredients;
        copy.Outputs = this.Outputs;
        copy.CraftingTime = this.CraftingTime;
        copy.NeedsToBeLearned = this.NeedsToBeLearned;
        return copy;
    }

}
