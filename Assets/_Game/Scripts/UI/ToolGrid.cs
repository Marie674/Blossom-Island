using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

public class ToolGrid : Singleton<ToolGrid>
{

    //
    //	public GameObject CursorGridPrefab;
    //	private bool UseGrid= false;
    //	private uint GridWidth = 0;
    //	private uint GridHeight = 0;
    //	private ItemSystem.ItemBase CurrentTool;
    //	public List<Transform> GridChildren = new List<Transform> ();
    //
    //	public override void Init () {
    //		base.Init ();
    //
    //	}

    //	void OnEnable(){
    //		ToolManager.OnToolChanged += ToolChange;
    //		ToolChange ();
    //	}
    //
    //	void OnDisable(){
    //		ToolManager.OnToolChanged -= ToolChange;
    //	}

    // Update is called once per frame
    //	void Update () {
    //		if (UseGrid==true) {
    //			if (Input.GetButtonDown ("Grid Width Up")) {
    //				AdjustGridSize (1, 0);
    //			}
    //			if (Input.GetButtonDown ("Grid Width Down")) {
    //				AdjustGridSize (-1, 0);
    //			}
    //			if (Input.GetButtonDown ("Grid Height Up")) {
    //				AdjustGridSize (0, 1);
    //			}
    //			if (Input.GetButtonDown ("Grid Height Down")) {
    //				AdjustGridSize (0, -1);
    //			}
    //		}
    //
    //		Snap ();
    //	}
    //
    //	void Snap(){
    //		Vector3 newPosition = transform.parent.position;
    //		float xOffset = 0.16f;
    //		float yOffset = 0.16f;
    //		newPosition.x = 0.32f * Mathf.Round (newPosition.x / 0.32f);
    //		newPosition.y = 0.32f * Mathf.Round (newPosition.y / 0.32f);
    //		newPosition.x += xOffset;
    //		newPosition.y += yOffset;
    //
    //		transform.position = newPosition;
    //	}
    //	void ToolChange(){
    //		CurrentTool = ToolManager.Instance.CurrentTool;
    //		bool usesGrid = false;
    //		if (CurrentTool.itemType==ItemType.Tool) {
    //			usesGrid = (CurrentTool as ItemTool).usesGrid;
    //		}
    //		else if(CurrentTool.itemType==ItemType.GridItem){
    //			usesGrid = (CurrentTool as ItemGrid).usesGrid;
    //		}
    //		if (CurrentTool != null &&  usesGrid==true) {
    //			UseGrid = true;
    //			GridHeight = 1;
    //			GridWidth = 1;
    //			AdjustGridSize (0, 0);
    //		} else {
    //			UseGrid = false;
    //			RemoveGridChildren ();
    //		}
    //	}
    //
    //	void AdjustGridSize(int pWidth=0, int pHeight=0){
    //		uint maxGridWidth = 0;
    //		uint maxGridHeight = 0;
    //
    //		if (CurrentTool.itemType==ItemType.Tool) {
    //			maxGridWidth = (CurrentTool as ItemTool).maxGridWidth;
    //			maxGridHeight = (CurrentTool as ItemTool).maxGridHeight;
    //		}
    //		else if(CurrentTool.itemType==ItemType.GridItem){
    //			maxGridWidth = (CurrentTool as ItemGrid).maxGridWidth;
    //			maxGridHeight = (CurrentTool as ItemGrid).maxGridHeight;
    //		}
    //		GridWidth = (uint)Mathf.Clamp ((int)GridWidth + pWidth, 1, maxGridWidth);
    //		GridHeight = (uint)Mathf.Clamp ((int)GridHeight + pHeight, 1, maxGridHeight);
    //		PopulateGrid ();
    //	}
    //
    //	void PopulateGrid(){
    //		RemoveGridChildren ();
    //		for (int i = 0; i < GridWidth; i++) {
    //			for (int j = 0; j < GridHeight; j++) {
    //				Vector3 newPosition = transform.position;
    //				newPosition.x = i * 0.32f;
    //				newPosition.y = -(j * 0.32f);
    //				newPosition.z = 0f;
    //				GameObject newGridChild = GameObject.Instantiate (CursorGridPrefab, newPosition, transform.rotation);
    //				newGridChild.transform.parent = transform;
    //				newGridChild.transform.localPosition = newPosition;
    //				GridChildren.Add (newGridChild.transform);
    //			}
    //		}
    //	}
    //
    //	void RemoveGridChildren(){
    //		foreach (Transform gridChild in GridChildren) {
    //			Destroy (gridChild.gameObject);
    //		}
    //		GridChildren.Clear ();
    //	}
    //
    //	void ToggleGrid(bool pToggle){
    //		foreach (Transform gridChild in GridChildren) {
    //			gridChild.gameObject.SetActive (pToggle);
    //		}
    //	}
}
