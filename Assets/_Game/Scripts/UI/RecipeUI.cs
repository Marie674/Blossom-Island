// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using ItemSystem;

// public class RecipeUI : MonoBehaviour {

// 	public AmountInputArrows ValueInput;
// 	public Button CraftingButton;
// 	public CraftingRecipe Recipe;

// 	public Image ItemIcon;
// 	public Text ItemName;
// 	public Text IngredientText;

// 	private PlayerCharacter Player;

// 	private PlayerInventory PlayerInventory;

// 	WindowToggle Window;

// 	// Use this for initialization
// 	public void Init (WindowToggle pWindow, bool pInstant=false, WorkStation pStation=null) {
// 		PlayerInventory = FindObjectOfType<PlayerInventory>();
// 		Player = GameObject.FindWithTag ("Player").GetComponent<PlayerCharacter>();
// 		ItemIcon.sprite = Recipe.Outputs[0].Item.item.itemSprite;
// 		ItemName.text = Recipe.Outputs[0].Item.item.itemName;
// 		Window = pWindow;

// 		CraftingButton.interactable = false;

// 		if (CheckInventory () == true) {
// 			print ("Recipe for " + Recipe.Outputs [0].Item.item.itemName + " has all materials");
// 			CraftingButton.interactable = true;

// 			if (pInstant == true) {
// 				print ("instant");
// 				CraftingButton.onClick.AddListener (delegate {
// 					CraftButtonInstant ();
// 				});
// 			} else {

// 				print ("not instant");
// 				CraftingButton.onClick.AddListener (delegate {
// 					CraftButton (pStation);
// 				});
// 			}

// 		} else {
// 			print ("Recipe for " + Recipe.Outputs[0].Item.item.itemName + " does not have all materials");
// 		}

// 	}

// 	public void CraftButtonInstant(){
// 		int amount = (int)ValueInput.CurrentValue;
// 		if (CheckInventory () == true) {
// 			foreach (var output in Recipe.Outputs) {

// 				ItemBase item = output.Item.item as ItemBase;

// 				if(output.ByProduct==false){
// 					item = CraftingManager.Instance.CraftItem (output.Item.item, Recipe.Ingredients);
// 				}

// 				ItemSpawner.Instance.SpawnItems (item, Player.transform.position, (uint)(output.Amount * amount));
// 			}
// 			RemoveItems ();
// 		}
// 		Window.Toggle ();
// 	}

// 	public void CraftButton(WorkStation pStation){
// 		int amount = (int)ValueInput.CurrentValue;
// 		if (CheckInventory () == true) {
// 			RemoveItems ();
// 			pStation.Setup (amount);
// 			print ("setup");

// 		}

// 		Window.Toggle ();
// 	}

// 	private bool CheckInventory(){
// 		bool missingIngredient = false;
// 		foreach (CraftingIngredient ingredient in Recipe.Ingredients) {
// 			bool hasItems = true;
// 			IngredientText.text += ingredient.Item.item.itemName + " x" + ingredient.Amount + " ";

// 			InventoryItemStack itemStack = PlayerInventory.FindItemStack (ingredient.Item.item);
// 			if (itemStack == null) {
// 				hasItems = false;
// 			}
// 			else if (itemStack.Amount < ingredient.Amount) {
// 				hasItems = false;
// 			}
// 			if (hasItems == true) {
// 				IngredientText.color = Color.black;
// 			} else {
// 				missingIngredient = true;
// 				IngredientText.color = Color.red;
// 			}

// 		}
// 		if (missingIngredient == true) {
// 			return false;
// 		} else {
// 			return true;
// 		}
// 	}


// 	private void RemoveItems(){
// 		foreach (CraftingIngredient ingredient in Recipe.Ingredients) {
// 			if (ingredient.Used == true) {
// 				InventoryItemStack itemStack = PlayerInventory.FindItemStack (ingredient.Item.item);
// 				PlayerInventory.RemoveFromStack(itemStack, (uint)ingredient.Amount);
// 			}
// 		}
// 	}
// }
