using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using System.Linq;
using PixelCrushers.QuestMachine;
[System.Serializable]
public struct CraftingIngredient
{
    public ItemContainer Item;
    public int Amount;
    public bool Used;
}

[System.Serializable]
public struct ItemOutput
{
    public ItemContainer Item;
    public int Amount;
    public int Chance;
    public bool ByProduct;
}

public class CraftingManager : Singleton<CraftingManager>, PixelCrushers.IMessageHandler
{

    public enum ItemTags
    {
        FireStarter,
        Food,
        Sweet,
        Spicy,
        Fruit,
        Vegetable,
        Drink,
        Material,
        Ore,
        Stone,
        Wood,
        Flower,
        None
    }

    public List<RecipeContainer> KnownRecipes;

    public float CraftingSpeed = 1f;

    public List<CraftingStation> CraftingStations;


    void Start()
    {
        //PixelCrushers.DialogueSystem.DialogueLua.SetVariable("CraftingManager RecipeAmount", KnownRecipes.Count);

    }


    void OnEnable()
    {
        PixelCrushers.MessageSystem.AddListener(this, "TeachRecipe", "pItemName");
    }
    void OnDisable()
    {
        PixelCrushers.MessageSystem.RemoveListener(this, "TeachRecipe", "pItemName");
    }
    public List<RecipeContainer> CheckRecipeMatches(List<CraftingIngredient> pIngredients, List<RecipeContainer> pRecipes = null)
    {
        List<RecipeContainer> recipeList = new List<RecipeContainer>();

        if (pRecipes == null)
        {
            pRecipes = KnownRecipes;
        }

        //iterate through all recipes.
        foreach (RecipeContainer recipe in pRecipes)
        {
            //iterate through recipe ingredients.
            bool match = true;
            int amount = 1;
            List<int> amounts = new List<int>();
            //print("Trying recipe " + recipe.Recipe.Outputs[0].Item.item.itemName);

            if (recipe.Recipe.NeedsToBeLearned == true && KnownRecipes.Contains(recipe) == false)
            {
                //print("Recipe " + recipe.Recipe.Outputs[0].Item.item.itemName + " is not known");
                match = false;
            }

            foreach (CraftingIngredient recipeIngredient in recipe.Recipe.Ingredients)
            {

                //  print("Checking recipe ingredient: " + recipeIngredient.Item.item.itemName);
                //if the ingredient amount is not high enough, break and try another recipe.

                bool foundIngredient = false;
                foreach (CraftingIngredient inputIngredient in pIngredients)
                {

                    if (inputIngredient.Item.item.itemID == recipeIngredient.Item.item.itemID)
                    {
                        foundIngredient = true;
                        if (inputIngredient.Amount % recipeIngredient.Amount != 0)
                        {
                            match = false;
                            break;
                        }

                        int newAmount = (inputIngredient.Amount / recipeIngredient.Amount);

                        amounts.Add(newAmount);
                        if (recipeIngredient.Amount > inputIngredient.Amount)
                        {
                            // print("Ingredient " + recipeIngredient.Item.item.itemName + "found, but too few");
                            match = false;
                            break;
                        }
                    }

                }

                if (foundIngredient == false)
                {
                    match = false;
                    // print("Ingredient " + recipeIngredient.Item.item.itemName + " not contained");
                    break;
                }
            }
            bool ingredientMismatch = false;

            foreach (CraftingIngredient inputIngredient in pIngredients)
            {
                bool wrongIngredient = true;
                foreach (CraftingIngredient recipeIngredient in recipe.Recipe.Ingredients)
                {
                    if (inputIngredient.Item.item.itemID == recipeIngredient.Item.item.itemID)
                    {
                        wrongIngredient = false;
                        break;
                    }

                }
                if (wrongIngredient)
                {
                    // print("Ingredient Mismatch: " + inputIngredient.Item.item.itemName);

                    ingredientMismatch = true;
                    break;
                }
            }

            if (ingredientMismatch == true)
            {
                match = false;
                // break;
            }

            if (match == true)
            {
                int prevAmount = amounts[0];
                foreach (int amt in amounts)
                {
                    if (amt != prevAmount)
                    {
                        match = false;
                    }
                    prevAmount = amt;

                }
            }



            if (match == true)
            {
                //  print("Adding recipe " + recipe.Recipe.Outputs[0].Item.item.itemName);

                amount = amounts.ToArray().Min();
                recipe.Amount = amount;
                recipeList.Add(recipe);
            }

        }

        return recipeList;

    }

    public void OnMessage(PixelCrushers.MessageArgs messageArgs)
    {
        switch (messageArgs.message)
        {
            case "TeachRecipe":
                TeachRecipe(messageArgs.firstValue.ToString());
                break;
        }
    }

    public ItemBase CraftItem(ItemBase pItem, List<CraftingIngredient> pIngredients)
    {
        ItemBase item = ItemSystemUtility.GetItemCopy(pItem.itemName, pItem.itemType);
        item.Ingredients = pIngredients;
        return item;
    }

    public bool CheckIfKnows(RecipeContainer pRecipe)
    {
        if (KnownRecipes.Contains(pRecipe))
        {
            return true;
        }
        return false;
    }

    public RecipeContainer GetRecipeByName(string pUniqueName)
    {
        RecipeContainer[] allRecipes = Resources.LoadAll<RecipeContainer>("Recipes");

        foreach (RecipeContainer recipe in allRecipes)
        {
            if (recipe.UniqueName == pUniqueName)
            {
                return recipe;
            }
        }

        return null;

    }

    public bool TeachRecipe(string pUniqueName)
    {
        return TeachRecipe(GetRecipeByName(pUniqueName));
    }


    public bool TeachRecipe(RecipeContainer pRecipe)
    {
        if (pRecipe == null)
        {
            return false;
        }
        if (!KnownRecipes.Contains(pRecipe))
        {
            KnownRecipes.Add(pRecipe);
            string itemName = pRecipe.Recipe.Outputs[0].Item.item.itemName;
            PixelCrushers.DialogueSystem.DialogueManager.ShowAlert("Learned new recipe: " + itemName);
            PixelCrushers.DialogueSystem.DialogueLua.SetVariable("CraftingManager Recipe" + KnownRecipes.Count, itemName);
            PixelCrushers.DialogueSystem.DialogueLua.SetVariable("CraftingManager RecipeAmount", KnownRecipes.Count);

            return true;
        }
        return false;
    }


}
