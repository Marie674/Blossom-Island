//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using ItemSystem;
//
//public class ToolManager : Singleton<ToolManager> {
//
//	public ToolControllerBase[] Tools;
//	public int CurrentToolIndex =0;
//	public ItemSystem.ItemBase CurrentTool;
//	private PlayerCharacter Player;
//
//
//	public override void Init () {
//		base.Init();
//		//print ("Init Tool Manager");
//		Player = GameObject.FindWithTag ("Player").GetComponent<PlayerCharacter> ();
//		Tools = GetComponentsInChildren<ToolControllerBase> ();
//		foreach (ToolControllerBase tool in Tools) {
//			tool.Init ();
//		}
//		//print ("End Init Tool Manager");
//		ChangeCurrentTool ();
//	}
//
//	void OnEnable(){
//		
//
//	}
//
//	void Update(){
//		
//		if(Input.GetButtonDown("Cycle Tools Up")){
//			CurrentToolIndex = ((CurrentToolIndex +1) % (Tools.Length));
//			ChangeCurrentTool ();
//		}
//		if(Input.GetButtonDown("Cycle Tools Down")){
//			CurrentToolIndex = (((CurrentToolIndex-1) + (Tools.Length)) % (Tools.Length));
//			ChangeCurrentTool ();
//		}
//
//
//			if (Input.GetButtonDown ("Use Tool")) {
//				UseTool ();
//			}
//
//	}
//
//	public delegate void ToolUse();
//	public static event ToolUse OnToolUsed;
//
//	void UseTool(){
//		if (Player.isUsingTool == false) {
//			if (OnToolUsed != null) {
//				Player.StartCoroutine ("SetToolCooldown", 0.25f);
//
//				OnToolUsed ();
//			}
//		}
//	}
//
//
//	public delegate void ToolChange();
//	public static event ToolChange OnToolChanged;
//
//	public void ChangeCurrentTool(){
//		
//		ItemBase currentItem = Tools [CurrentToolIndex].GetComponent<ItemContainer>().item;
//
//		if (currentItem.itemType==ItemType.Tool) {
//			CurrentTool = ItemSystemUtility.GetItemCopy (currentItem.itemName, ItemType.Tool) as ItemTool;
//		}
//		else if(currentItem.itemType==ItemType.GridItem){
//			CurrentTool = ItemSystemUtility.GetItemCopy (currentItem.itemName, ItemType.GridItem) as ItemGrid;
//		}
//
//		foreach (ToolControllerBase tool in Tools) {
//			if (tool.GetComponent<ItemContainer> ().item.itemID != CurrentTool.itemID) {
//				tool.gameObject.SetActive (false);
//			} else {
//				tool.gameObject.SetActive (true);
//			}
//		}
//
//		if (OnToolChanged != null) {
//			OnToolChanged ();
//		}
//
//	}
//}
