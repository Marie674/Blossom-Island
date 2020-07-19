// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using ItemSystem;
// using PixelCrushers.DialogueSystem;

// public class ItemShelf : MonoBehaviour {

// 	public bool PlayerCanPut = true;
// 	public bool PlayerCanTake=true;

// 	public List<ShelfSlot> Slots = new List<ShelfSlot>();

// 	void Start(){
		
// 		Slots.Clear ();

// 		ShelfSlot[] slots = GetComponentsInChildren<ShelfSlot> ();

// 		for (int i = 0; i <= slots.Length - 1; i++) {
// 			Slots.Add (slots [i]);
// 		}

// 	}


// 	public void Interact(){

// 		if (Slots.Count <= 0) {
// 			return;
// 		}

// 		if (PlayerCanPut) {
// 			AddItem ();
// 		} else {
// 			DialogueManager.ShowAlert ("Cannot add items here.");
// 		}

// 	}

// 	public void AltInteract(){

// 		if (Slots.Count <= 0) {
// 			return;
// 		}
// 		if (PlayerCanTake) {
// 			RemoveItem ();
// 		}

// 	}

// 	void RemoveItem(){

// 		//Get the first occupied slot
// 		ShelfSlot currentSlot = GetFirstOccupiedSlot ();

// 		//If no slot is occupied, return
// 		if (currentSlot == null) {
// 			DialogueManager.ShowAlert ("No more items to take.");
// 			return;
// 		}

// 		//Remove item from slot
// 		currentSlot.CurrentAmount -= 1;

// 		//Spawn item in world
// 		ItemSpawner.Instance.SpawnItems (currentSlot.Item.item, GameManager.Instance.Player.transform.position);

// 		//If slot is empty, reset contained item
// 		if (currentSlot.CurrentAmount <= 0) {
// 			currentSlot.Item.item.itemID=0;
// 		}

// 		//Update visuals
// 		currentSlot.UpdateVisuals ();
// 		return;

// 	}

// 	//Remove all items for a matching recipe
// 	public void RemoveRecipeItems(CraftingRecipe pRecipe){

// 		//Iterate through all of the recipe's ingredients
// 		foreach (CraftingIngredient recipeIngredient in pRecipe.Ingredients) {

// 			//Get the slot containing the ingredient
// 			ShelfSlot slot = GetSlot (recipeIngredient.Item.item);

// 			//Remove an item for each of the recipe amount
// 			for(int i =0; i<recipeIngredient.Amount;i++){
// 				slot.CurrentAmount -= 1;

// 				//is slot amount is 0, reset contained item
// 				if (slot.CurrentAmount <= 0) {
// 					slot.Item.item.itemID=0;
// 				}

// 				//update visuals
// 				slot.UpdateVisuals ();
// 			}

// 		}
// 	}

// 	public bool CheckAddOutput(ItemBase pItem, int pAmount){
// 		for (int i = 0; i < pAmount; i++) {
// 			ShelfSlot currentSlot = GetSlot (pItem);

// 			//no existing slot with that item
// 			if (currentSlot ==null) {

// 				currentSlot = GetEmptySlot();

// 				//no unoccupied slot
// 				if (currentSlot ==null) {
// 					return false;
// 				}

// 				//valid
// 				return true;
// 			}

// 			//Slot is full
// 			if (currentSlot.CurrentAmount >= currentSlot.MaxAmount) {	
// 				return false;
// 			}

// 		}
// 		//valid
// 		return true;
// 	}

// 	public void AddOutput(ItemBase pItem, int pAmount){
		
// 		for (int i = 0; i < pAmount; i++) {
// 			ShelfSlot currentSlot = GetSlot (pItem);

// 			//no existing slot with that item
// 			if (currentSlot ==null) {

// 				currentSlot = GetEmptySlot();

// 				//no empty slot
// 				if (currentSlot ==null) {
// 					print("no empty slot");
// 					return;
// 				}

// 				//add item to empty slot
// 				print("adding new item to empty slot");
// 				currentSlot.Item.item = ItemSystemUtility.GetItemCopy (pItem.itemID, pItem.itemType);
// 				currentSlot.CurrentAmount = 1;
// 				currentSlot.UpdateVisuals ();
// 				return;
// 			}

// 			//Slot is full
// 			if (currentSlot.CurrentAmount >= currentSlot.MaxAmount) {
// 				print("all slots are full");	
// 				return;
// 			}
// 			print("adding item to stack");	
// 			currentSlot.CurrentAmount += 1;
// 			currentSlot.UpdateVisuals ();
// 		};
// 	}

// 	void AddItem(){
		
// 		InventoryItemStack currentItem = Toolbar.Instance.SelectedSlot.ReferencedItemStack;

// 		if (currentItem.ContainedItem.itemID == 0) {
// 			return;
// 		}

// 		ShelfSlot currentSlot = GetSlot (currentItem.ContainedItem);

// 		//no existing slot with that item
// 		if (currentSlot ==null) {
			
// 			currentSlot = GetEmptySlot();

// 			//no empty slot
// 			if (currentSlot ==null) {
// 				print("no empty slot");
// 				return;
// 			}

// 			//add item to empty slot
// 			print("adding new item to empty slot");
// 			currentSlot.Item.item = ItemSystemUtility.GetItemCopy (currentItem.ContainedItem.itemID, currentItem.ContainedItem.itemType);
// 			currentSlot.CurrentAmount = 1;
// 			PlayerInventory.Instance.Remove (currentItem);
// 			currentSlot.UpdateVisuals ();
// 			return;
// 		}

// 		//Slot is full
// 		if (currentSlot.CurrentAmount >= currentSlot.MaxAmount) {
// 			print("all slots are full");	
// 			return;
// 		}
// 		print("adding item to stack");	
// 		currentSlot.CurrentAmount += 1;
// 		currentSlot.UpdateVisuals ();
// 		PlayerInventory.Instance.Remove (currentItem);

// 	}

// 	ShelfSlot GetFirstOccupiedSlot(){
// 		for (int i=Slots.Count-1; i >= 0; i--) {
// 			if (Slots[i].Item.item.itemID != 0) {
// 				return Slots[i];
// 			}
// 		}
// 		return null;
// 	}

// 	ShelfSlot GetSlot(ItemBase pItem){
// 		for (int i=0; i <= Slots.Count-1; i++) {
// 			if (Slots[i].Item.item.itemID == pItem.itemID) {
// 				return Slots[i];
// 			}
// 		}
// 		return null;
// 	}

// 	ShelfSlot GetEmptySlot(){
// 		for (int i=0; i <= Slots.Count-1; i++) {
// 			print ("slot " + i +" "+Slots [i].Item);
// 			if (Slots[i].Item.item.itemID==0) {
				
// 				return Slots[i];
// 			}
// 		}
// 		return null;
// 	}

// }
