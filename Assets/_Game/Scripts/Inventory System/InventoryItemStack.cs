using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
public class InventoryItemStack
{
	public ItemBase ContainedItem;
	public int Amount;

	public delegate void ItemChange();
	public event ItemChange OnItemChanged;

	public void Add(int pAmount){
		Amount += pAmount;
		if (OnItemChanged != null) {
			OnItemChanged ();
		}
	}

	public void Remove(int pAmount){
		Amount -= pAmount;
		if (OnItemChanged != null) {
			OnItemChanged ();
		}
	}

	public void Delete(){
		if (OnItemChanged != null) {
			OnItemChanged ();
		}
	}
}