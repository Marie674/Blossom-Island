using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;


[System.Serializable]
public class CraftingRecipe  {

	public List<CraftingIngredient> Ingredients = new List<CraftingIngredient>();
	public List<ItemOutput> Outputs;
	public float CraftingTime;
	public bool NeedsToBeLearned = false;

	public CraftingRecipe Copy(){
		CraftingRecipe copy = new CraftingRecipe ();
		copy.Ingredients = this.Ingredients;
		copy.Outputs = this.Outputs;
		copy.CraftingTime = this.CraftingTime;
		copy.NeedsToBeLearned = this.NeedsToBeLearned;
		return copy;
	}

}
