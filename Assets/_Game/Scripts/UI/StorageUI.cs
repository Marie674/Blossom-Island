// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using ItemSystem;
// using UnityEngine.UI;
// using TMPro;

// public class StorageUI : MonoBehaviour {

// 	public TextMeshProUGUI Title;
// 	public TextMeshProUGUI TabDescription;
// 	public Transform ItemUIContainer;
// 	public ItemStackUI ItemUIPrefab;
// 	protected WindowToggle Window;
// 	protected List<ItemStackUI> CurrentItems = new List<ItemStackUI>();
// 	public ItemBase SelectedItem=null;
// 	public InventoryTabUI SelectedTab;
// 	public GameObject TabContainer;
// 	protected InventoryTabUI[] Tabs;
// 	public StorageObject CurrentStorage;
// 	protected int SelectedTabId=0;
// 	protected InventoryItemStack SelectedStack;

// 	protected virtual void Awake(){
// 		Tabs = TabContainer.GetComponentsInChildren<InventoryTabUI> ();
// 		Window = GetComponent<WindowToggle> ();
// 		SelectedTab = Tabs [0];
// 	}


// 	//Set listeners for window open and close
// 	protected void OnEnable(){

// 		Window.Window.onOpen.AddListener (delegate {
// 			OnWindowOpen();	
// 		});
// 		Window.Window.onClose.AddListener (delegate {
// 			OnWindowClose();	
// 		});

// 	}
// 	protected void OnDisable(){
// 		Window.Window.onOpen.RemoveAllListeners();
// 		Window.Window.onClose.RemoveAllListeners();
// 	}

// 	public void Open(){
// 		Window.Open();
// 	}

// 	//when window is opened
// 	protected virtual void OnWindowOpen(){
// 	//Set target storage for tabs	
// 		foreach (InventoryTabUI Tab in Tabs) {
// 			Tab.StorageStacks = CurrentStorage.ContainedStacks;
// 		}
// 	//Listen to storage item change
// 		CurrentStorage.OnItemChanged += Redraw;
// 	//Draw the current tab
// 		Draw (SelectedTab);
// 	}


// 	// when the window closes - clear items, remove item change listener
// 	protected virtual void OnWindowClose(){
// 		Clear ();
// 		CurrentStorage.OnItemChanged -= Redraw;
// 	}

// 	//destroy everything,clear list of drawn items
// 	protected void Clear(){
// 		foreach (ItemStackUI itemUI in CurrentItems) {
// 			Destroy (itemUI.gameObject);
// 		}
// 		CurrentItems.Clear ();
// 	}

// 	protected void Draw(InventoryTabUI SelectedTab){
// 		SelectedTab.GetItems ();

// 		foreach (InventoryItemStack itemStack in SelectedTab.ItemStacks) {
// 			ItemStackUI itemUI = Instantiate (ItemUIPrefab, ItemUIContainer);
// 			itemUI.CurrentUI = this;

// 			CurrentItems.Add (itemUI);
// 			itemUI.ItemStack = itemStack;
// 			itemUI.Draw ();
// 		}

// 		if (SelectedTab.ItemStacks.Count > 0) {
// 			//SetSelectedStack (Content.transform.GetChild(0).GetComponent<InventoryItemUI>());
// 			TabDescription.text = string.Empty;

// 		} else {
// 			TabDescription.text = "No items in this category";
// 		}

// 		Reselect ();
// 	}

// 	public void Redraw(){
// 		Clear ();

// 		ChangeTabs (Tabs [SelectedTabId]);
// 		Reselect ();
// 	}


// 	public void ChangeTabs(InventoryTabUI Tab){
// 		SelectedTab = Tab;
// 		for (int i = 0; i < Tabs.Length; i++) {
// 			if (Tabs [i] == SelectedTab) {
// 				SelectedTabId = i;
// 			}
// 		}
// 		Clear ();
// 		Draw (SelectedTab);
// 		Reselect ();
// 	}

// 	public void TransferToStorage(){
		
// 		ItemBase inventorySelection = PlayerInventoryUI.SelectedItem;

// 		if (PlayerInventoryUI.SelectedStack==null || PlayerInventoryUI.SelectedStack.Amount < 1) {
// 			return;
// 		}
// 		if (CurrentStorage.Add (inventorySelection) == true) {
// 			PlayerInventory.Remove (PlayerInventoryUI.SelectedStack, 1);
// 			if (PlayerInventoryUI.SelectedStack.Amount < 1) {
// 				PlayerInventoryUI.SetSelectedItem (null);
// 				PlayerInventoryUI.Redraw ();
// 			}
// 		}

// 	}

// 	public void TransferToInventory(){
// 		ItemBase storageSelection = SelectedItem;
// 		if (SelectedStack==null || SelectedStack.Amount < 1) {
// 			return;
// 		}
// 		if (PlayerInventory.Add (storageSelection) == true) {
// 			CurrentStorage.RemoveFromStack (SelectedStack, 1);
// 			if (SelectedStack.Amount < 1) {
// 				SetSelectedItem (null);
// 			}
// 		}
// 	}

// 	protected void Reselect(){
		
// 		//check if stack still exists, set it to that one if so.
// 		if (SelectedItem.itemID != 0) {
// //			print ("selecting item: " + SelectedItem.itemName);
// 			SetSelectedItem (SelectedItem);
	
// 		}
// 		//if stack does not exist anymore, select first stack if it exists
// 		else if (CurrentItems.Count > 0) {
// //			print ("selecting first item: " + CurrentItems[0].ItemStack.Item.itemName);
// 			SetSelectedItem (CurrentItems[0].ItemStack.ContainedItem);
// 		} else {
// 			print ("no items");
// 			SetSelectedItem (null);
// 		}

// 	}


// 	public void SetSelectedItem(ItemBase pItem){
// 		SelectedItem = pItem;
// 		if (SelectedItem == null) {
// 			SelectedItem = new ItemBase ();
// 			SelectedStack = null;
// 		}

// 		if(SelectedItem.itemID != 0){
// 			foreach (ItemStackUI itemUI in CurrentItems) {
// 				if (itemUI.ItemStack.ContainedItem.itemID != SelectedItem.itemID) {
// 					itemUI.FrameImage.enabled = false;
// 				} else {
// 					SelectedStack = itemUI.ItemStack;
// 					itemUI.GetComponent<Button> ().Select ();
// 					itemUI.FrameImage.enabled = true;
// 				}
// 			}
// 		}

// 	}

// }
