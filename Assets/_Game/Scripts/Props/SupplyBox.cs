using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;



public class SupplyBox : MonoBehaviour {
	
	public List<BuildingMaterial> SupplySlots;

	public void Interact(){
		if (SupplySlots.Count <= 0) {
			return;
		}
		InventoryItemStack currentItem = Toolbar.Instance.SelectedSlot.ReferencedItemStack;
		if (currentItem == null) {
			return;
		}
		BuildingMaterial currentSlot = new BuildingMaterial ();
		bool foundSlot = false;
		int i = 0;
		for (i=i; i < SupplySlots.Count-1; i++) {
			if (SupplySlots[i].Item.item.itemID == currentItem.ContainedItem.itemID) {
				if (SupplySlots[i].CurrentAmount >= SupplySlots[i].TargetAmount) {
					return;
				}
				break;
			}
		}
		currentSlot = SupplySlots [i];
		currentSlot.CurrentAmount += 1;
		SupplySlots[i]=currentSlot;
		FindObjectOfType<PlayerInventory>().RemoveFromStack(currentItem);

	}

	public void SetMaterials(List<BuildingMaterial> pMaterials){
		SupplySlots = pMaterials;
	}

	public bool CheckMaterials(){
		foreach (BuildingMaterial slot in SupplySlots) {
			if (slot.CurrentAmount < slot.TargetAmount) {
				return false;
			}
		}
		return true;
	}

}
