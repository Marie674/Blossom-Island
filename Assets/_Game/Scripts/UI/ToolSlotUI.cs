using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSystem;

public class ToolSlotUI : MonoBehaviour {

	private Text AmountText;
	private Image ItemIcon;

	public ToolControllerGridItem Tool;

	public InventoryItemStack ReferencedItemStack = null;
	public bool IsActiveSlot=false;
	public GameObject ActiveFrame;
	public bool IsInit=false;
	private bool IsQuitting;

	void OnApplicationQuit(){
		IsQuitting = true;
	}

	// Get the components and update current item info and visuals
	public void Init () {
		AmountText = transform.Find ("AmountText").GetComponent<Text>();
		ItemIcon = transform.Find ("ItemIcon").GetComponent<Image> ();
		ActiveFrame = transform.Find ("ActiveFrame").gameObject;
		ChangeItem (null);
		IsInit = true;
	}

	void OnEnable(){
		if (IsInit == false) {
			Init ();
		}
	}

	void OnDisable(){
		if (!IsQuitting) {
			ReferencedItemStack.OnItemChanged -= UpdateItem;
			IsInit = false;
		}

	}

	// Event broadcast upon changing item

	public delegate void SlotItemChange();
	public event SlotItemChange OnSlotItemChanged;

	public void ChangeItem(InventoryItemStack pItemStack){

		if (pItemStack != null && pItemStack.ContainedItem.itemType != ItemType.GridItem) {
			return;
		}
	//	print (pItemStack);
		//If an item stack is specified
		if (pItemStack != null && pItemStack.Amount>0) {

			// If the slot already referenced an item stack, unsubscribe from its item change
			if (ReferencedItemStack != null) {
				ReferencedItemStack.OnItemChanged -= UpdateItem;
			}
			// Set the reference stack to the specified stack and subscribe to its item change event
			ReferencedItemStack = pItemStack;
			ReferencedItemStack.OnItemChanged += UpdateItem;

			UpdateVisuals ();

			// If stack was referenced by another slot, remove the reference
			foreach (ToolbarSlotUI slot in Toolbar.Instance.Slots) {
				if (slot != this && slot.ReferencedItemStack == this.ReferencedItemStack) {
					slot.ChangeItem (null);
				}
			}
		} 
		else if (pItemStack == null || pItemStack.Amount <= 0) {
			// unsubscribe from item change event
			if (ReferencedItemStack != null) {
				ReferencedItemStack.OnItemChanged -= UpdateItem;
			}

			//		print(gameObject.name + " set null item stack ref");
			// remove reference and update visuals
			ReferencedItemStack = null;
			UpdateVisuals ();
		}

//		if (ReferencedItemStack != null) {
//			Tool.GridItem = ItemSystemUtility.GetItemCopy (ReferencedItemStack.Item.itemID, ItemType.GridItem) as ItemGrid;
//		} else {
//			Tool.GridItem = null;
//		}
		// Call item change event
		if (OnSlotItemChanged != null) {
			OnSlotItemChanged ();
		}
	}



	// If an item stack is referenced, set the visual info to match, if not, erase visuals
	void UpdateVisuals(){
		if (ReferencedItemStack != null) {
			ItemIcon.color = Color.white;
			ItemIcon.sprite = ReferencedItemStack.ContainedItem.itemIcon;
			if (ReferencedItemStack.Amount > 1) {
				AmountText.text = ReferencedItemStack.Amount.ToString();
			} else {
				AmountText.text = string.Empty;
			}
		}
		else{
			ItemIcon.sprite = null;
			ItemIcon.color = new Color (1, 1, 1, 0);
			AmountText.text = string.Empty;
		}
	}

	void UpdateItem(){
		ChangeItem (ReferencedItemStack);

	}

	// Set is active to the toggle and show/hide the frame
	public void Activate(bool pToggle){
		IsActiveSlot = pToggle;
		if(ActiveFrame != null){
			ActiveFrame.SetActive(pToggle);
		}
	}
	

}
