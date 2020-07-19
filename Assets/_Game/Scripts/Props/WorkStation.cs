// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using PixelCrushers.DialogueSystem;
// using ItemSystem;

// public class WorkStation : MonoBehaviour
// {

//     public List<RecipeContainer> StationRecipes;
//     public bool AutomaticProgression = false;
//     public SpriteRenderer RecipeSprite;
//     public GameObject FillBar;
//     public Image FillSprite;

//     public StorageObject InputStorage;
//     public StorageObject OutputStorage;
//     bool RecipeSet = false;
//     bool Proceeding = false;

//     //	private int Amount;


//     private float CurrentProgression = 0;
//     private float TargetProgression;

//     public RecipeContainer CurrentRecipe;


//     //Player interaction
//     public void Interact()
//     {
//         //If the recipe is not set, check inputs for a matching recipe
//         if (RecipeSet == false)
//         {
//             CheckInput();
//         }
//         //If the recipe is set, proceed the recipe
//         else if (RecipeSet == true && AutomaticProgression == false)
//         {
//             Proceed();
//         }
//     }

//     //Proceed the recipe
//     private void Proceed()
//     {

//         CurrentProgression += GameManager.Instance.Player.GetComponent<ProximitySelector>().TimeBeforeHold;
//         FillSprite.fillAmount = MapRangeExtension.MapRange(CurrentProgression, 0f, TargetProgression, 0f, 1f);


//         //If the progression is done
//         if (CurrentProgression >= TargetProgression)
//         {

//             //If the output is not null and is full, return
//             if (OutputStorage != null)
//             {

//                 foreach (ItemOutput output in CurrentRecipe.Recipe.Outputs)
//                 {

//                     if (OutputStorage.CheckAdd(output.Item.item, output.Amount) == false)
//                     {

//                         if (AutomaticProgression == false)
//                         {
//                             DialogueManager.ShowAlert("Output is full.");
//                         }

//                         return;

//                     }

//                 }

//             }
//             //Output the items and reset the station
//             RecipeDone();

//         }

//     }

//     //Check input items for a recipe match
//     bool CheckInput()
//     {


//         //Build a list of ingredients from the input's items
//         List<CraftingIngredient> ingredientList = new List<CraftingIngredient>();
//         ingredientList.Clear();
//         foreach (InventoryItemStack stack in InputStorage.ContainedStacks)
//         {
//             CraftingIngredient ingredient = new CraftingIngredient();
//             ItemContainer container = new ItemContainer();
//             container.item = ItemSystemUtility.GetItemCopy(stack.ContainedItem.itemID, stack.ContainedItem.itemType);
//             ingredient.Item = container;
//             ingredient.Amount = stack.Amount;
//             if (ingredient.Item.item.itemID != 0)
//             {
//                 ingredientList.Add(ingredient);
//             }
//         }


//         if (CraftingManager.Instance.CheckRecipeMatch(ingredientList, StationRecipes) == null)
//         {
//             DialogueManager.ShowAlert("No matching recipe.");
//             return false;
//         }
//         CurrentRecipe = new RecipeContainer();
//         CurrentRecipe.Recipe = CraftingManager.Instance.CheckRecipeMatch(ingredientList, StationRecipes).Recipe.Copy();

//         //No recipe has matched
//         // if (CurrentRecipe == null) {
//         // 	DialogueManager.ShowAlert ("No matching recipe.");
//         // 	return false;
//         // } 

//         //A recipe was found, setting up the station

//         // print ("recipe found");
//         //CurrentRecipe.Recipe.Ingredients = ingredientList;
//         Setup(1);
//         return true;


//     }

//     //Setup the crafting station
//     public void Setup(int pAmount)
//     {

//         //Set the recipe
//         Proceeding = true;
//         RecipeSprite.sprite = CurrentRecipe.Recipe.Outputs[0].Item.item.itemIcon;
//         //Amount = pAmount;
//         TargetProgression = CurrentRecipe.Recipe.CraftingTime;
//         CurrentProgression = 0f;
//         FillSprite.fillAmount = 0f;
//         FillBar.SetActive(true);
//         RecipeSet = true;

//         //Remove items from input shelf
//         if (InputStorage != null)
//         {
//             foreach (CraftingIngredient ingredient in CurrentRecipe.Recipe.Ingredients)
//             {
//                 InputStorage.RemoveFromStack(InputStorage.FindItemStack(ingredient.Item.item), (uint)ingredient.Amount);
//             }
//         }

//         //If the station's progression is automatic, start progression coroutine
//         if (AutomaticProgression)
//         {
//             StartCoroutine("ProceedAutomatic");
//         }
//     }

//     private IEnumerator ProceedAutomatic()
//     {
//         while (Proceeding == true)
//         {
//             Proceed();
//             yield return new WaitForSeconds(GameManager.Instance.Player.GetComponent<ProximitySelector>().TimeBeforeHold);
//         }
//     }

//     //When the recipe is done
//     void RecipeDone()
//     {
//         //Stop automatic proceeding
//         Proceeding = false;
//         StopCoroutine("ProceedAutomatic");

//         OutputItems();

//         //Reset station
//         CurrentRecipe = null;
//         RecipeSet = false;
//         RecipeSprite.sprite = null;
//         //Amount = 0;
//         CurrentProgression = 0f;
//         FillSprite.fillAmount = 0f;
//         FillBar.SetActive(false);

//     }

//     void OutputItems()
//     {
//         //Output items
//         foreach (ItemOutput output in CurrentRecipe.Recipe.Outputs)
//         {


//             ItemBase item = output.Item.item as ItemBase;

//             if (output.ByProduct == false)
//             {
//                 item = CraftingManager.Instance.CraftItem(output.Item.item, CurrentRecipe.Recipe.Ingredients);
//             }
//             //If output shelf is null, spawn the items in the world
//             if (OutputStorage == null)
//             {
//                 ItemSpawner.Instance.SpawnItems(item, GameManager.Instance.Player.transform.position, (uint)output.Amount);
//             }
//             else
//             {
//                 //If output shelf is not null, add items to it
//                 for (int i = 0; i < output.Amount; i++)
//                     OutputStorage.Add(item);
//             }
//         }
//     }

// }
