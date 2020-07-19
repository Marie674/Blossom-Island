using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using UnityEngine.UI;
using TMPro;
public class ItemPopUpUI : MonoBehaviour {

public TextMeshProUGUI Description;
public Image ItemIcon;

		public void Open(ItemBase pItem){

	
				ItemIcon.sprite = pItem.itemIcon;
				Description.text = "<b><align=center>"+pItem.itemName+"</align></b>";
				Description.text += "\n \n" + pItem.itemDescription;
				GetComponent<WindowToggle> ().Open();
				
			
	}

	public void Close(){
		GetComponent<WindowToggle>().Close();
	}

}
