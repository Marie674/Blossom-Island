using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Data/Recipe")]
public class RecipeContainer : ScriptableObject
{
    public CraftingRecipe Recipe;
    public int Amount = 1;

    public string UniqueName;

}