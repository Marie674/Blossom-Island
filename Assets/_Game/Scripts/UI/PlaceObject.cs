// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using ItemSystem;
// using CreativeSpore.SuperTilemapEditor;

// public class PlaceObject : Singleton<PlaceObject> {

// 	private static PlaceObject s_Instance=null;

// 	GameObject Object;

// 	public bool IsPlaceableObject;

// 	public SpriteRenderer DisplaySprite;
// 	public bool CanPlace = false;
// 	public bool CanRotate=false;
// 	List<TileCollisionRow> TileCollisions;
// 	private bool Listening = false;

// 	private STETilemap Map;

// 	//Dimensions in tiles
// 	public int Width = 0;
// 	public int Height = 0;

// 	private Vector3 SpriteTopLeft;

// 	public override void Init(){
// 		base.Init ();
// 		Toolbar.Instance.OnSelectedSlotItemChanged += SetObject;
// 	}

// 	void OnDisable(){
// 		Listening = false;
// 		Toolbar.Instance.OnSelectedSlotItemChanged -= SetObject;
// 		ItemUseManager.OnUsePlaceableItem -= Place;
// 	}

// 	public void SetObject () {

// 		if (Toolbar.Instance.SelectedSlot.ReferencedItemStack==null) {
			
// 			IsPlaceableObject = false;
// 			Listening = false;

// 			UpdateVisuals (false);
// 			return;
// 		}
// 		ItemBase item;

// 		if(Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem.itemType == ItemType.PlaceableItem){
// 			item = ItemSystemUtility.GetItemCopy (Toolbar.Instance.SelectedSlot.ReferencedItemStack.ContainedItem.itemName, ItemType.PlaceableItem);
// 		}
// 		else{
// 			IsPlaceableObject = false;
// 			Listening = false;

// 			UpdateVisuals (false);
// 			return;
// 		}

// 		IsPlaceableObject = true;
// 		ItemPlaceable placeAbleItem = item as ItemPlaceable;
// 		Object = placeAbleItem.ObjectPrefab;
// 		CanRotate = placeAbleItem.CanRotate;
// 		UpdateVisuals (true,placeAbleItem.itemSprite);
// 		Width = Mathf.RoundToInt(DisplaySprite.bounds.size.x/0.32f);
// 		Height = Mathf.RoundToInt(DisplaySprite.bounds.size.y/0.32f);
// 		TileCollisions = Object.GetComponent<OccupySpace> ().TileCollisions;

// 	}



// 	public delegate void ObjectChange();
// 	public event ObjectChange OnObjectChanged;

// 	void UpdateVisuals(bool pToggle, Sprite sprite = null){
		
// 		DisplaySprite.sprite = sprite;

// 		if (pToggle == false) {
// 			DisplaySprite.color = new Color (1, 1, 1, 0);
// 		}
// 		else {
// 			DisplaySprite.color = new Color (1, 1, 1, 1);
// 		}

// 		if (OnObjectChanged != null) {
// 			OnObjectChanged ();
// 		}
// 	}


// 	void Update(){

// 		if (IsPlaceableObject == false) {
// 			CanPlace = false;
// 			Listening = false;
// 			return;
// 		}
// 		if (Input.GetButtonDown("PlaceObject Rotate")){
// 			RotateObject ();
// 		}

// 	}

// 	void RotateObject(){
		
// 			Vector3 spriteCenter = GetComponent<SpriteRenderer> ().sprite.bounds.center;
// 			spriteCenter = transform.TransformPoint (spriteCenter);
// 			transform.RotateAround (spriteCenter, new Vector3 (0, 0, 1), -90f);
// 			Vector3 newAnchorPos = transform.position;
// 			SetObjectOffset ();
// 			newAnchorPos.x = 0.32f * Mathf.Round (newAnchorPos.x / 0.32f);
// 			newAnchorPos.y = 0.32f * Mathf.Round (newAnchorPos.y / 0.32f);
// 			transform.position = newAnchorPos;

// 	}
// 	void LateUpdate(){
		
// 		if (IsPlaceableObject == false) {
// 			CanPlace = false;
// 			Listening = false;
// 			return;
// 		}

// 		CanPlace = true;

// 		if (CheckTiles (DisplaySprite.transform.position) == false) {
// 			CanPlace = false;
// 		}

// 		if (CanPlace == false) {
// 			Listening = false;
// 			DisplaySprite.color = new Color(1,0,0,0.5f);
// 			ItemUseManager.OnUsePlaceableItem -= Place;

// 		} else {
// 			DisplaySprite.color = new Color(0,1,0,0.5f);
// 			if (Listening == false) {
// 				ItemUseManager.OnUsePlaceableItem += Place;
// 			}
// 			Listening = true;
// 		}
// 	}


// 	public void SetObjectOffset(){
// 		if (DisplaySprite.sprite == null) {
// 			return;
// 		}
// 		Vector3 spriteExtents = DisplaySprite.sprite.bounds.extents;
// 		if (GetComponent<SpriteRenderer> ().sprite != null) {
// 			transform.localPosition = Vector3.zero;

// 			Vector3 objPivot = GetComponent<SpriteRenderer> ().sprite.pivot;
// 			objPivot = new Vector3(objPivot.x,objPivot.y,0);
// 			Vector3 objTopLeft = new Vector3 (0, (spriteExtents.y*2f)*100f, 0);

// 			Vector3 difference = objPivot - objTopLeft;
// 			Vector3 offsetPos = transform.position;

// 			if (transform.localRotation.eulerAngles.z == 0 || transform.localRotation.eulerAngles.z == 180) {
// 				offsetPos.x += difference.x / 100f;
// 				offsetPos.y += difference.y / 100f;
// 			} else {
// 				offsetPos.x += difference.y / 100f;
// 				offsetPos.y += difference.x / 100f;
// 			}
// 			transform.position = offsetPos;
// 		}
// 	}

		
// 	bool CheckTiles(Vector3 position){

// 		Map = GameObject.FindWithTag ("Occupied Tiles").GetComponent<STETilemap> ();
// //		int gridX;
// //		int gridY;
// 		Vector3 tilePosition = Vector3.zero;
// 		SpriteTopLeft = new Vector3 (DisplaySprite.bounds.min.x, DisplaySprite.bounds.max.y, 0);
// 		SpriteTopLeft.x =  0.32f * Mathf.Round (SpriteTopLeft.x / 0.32f);
// 		SpriteTopLeft.y =  0.32f * Mathf.Round (SpriteTopLeft.y / 0.32f);

// 		if (transform.localRotation.eulerAngles.z == 0) {
// 			for (int i = 0; i < Height; i++) {
// 				for (int j = 0; j < Width; j++) {
// 					if (TileCollisions [i].Collisions [j].IsOccupied == true) {
// 					tilePosition = new Vector3 (j, i, 0f);

// 					tilePosition = new Vector2 (SpriteTopLeft.x + (tilePosition.x * 0.32f), (SpriteTopLeft.y + (-tilePosition.y * 0.32f)));

// //					print (tilePosition.x + " " + tilePosition.y);
// 					if(CheckTile(Map,tilePosition)==false){
// 						return false;
// 					}
// 					}
// 				}
// 			}
// 		}

// 		//right
// 		else if (transform.localRotation.eulerAngles.z == 270){
// 			print ("right");
// 			for (int i = Height-1 ; i>=0; i--) {
	
// 				for (int j = 0 ; j <Width; j++) {
// 					if (TileCollisions [i].Collisions [j].IsOccupied == true) {
// 					tilePosition = new Vector3 (i, -j, 0f);
// 					tilePosition = new Vector2 (SpriteTopLeft.x + (tilePosition.x * 0.32f), (SpriteTopLeft.y + (tilePosition.y * 0.32f)));
// 					//print (Mathf.RoundToInt (tilePosition.x) + "," + Mathf.RoundToInt(tilePosition.y));
// 					if(CheckTile(Map,tilePosition)==false){
// 						return false;
// 					}
// 					}
// 				}
// 			}
// 		}
// 		//down
// 		else if (transform.localRotation.eulerAngles.z == 180){
// 			for (int i = Height-1; i >= 0; i--) {
// 				for (int j = 0; j< Width; j++) {
// 					if (TileCollisions [i].Collisions [j].IsOccupied == true) {
// 					tilePosition = new Vector3 (j, i, 0f);
// 					tilePosition = new Vector2 (SpriteTopLeft.x + (tilePosition.x * 0.32f), (SpriteTopLeft.y + (-tilePosition.y * 0.32f)));
// 					//print (tilePosition.x + " " + tilePosition.y);
// 					//print (Mathf.RoundToInt (tilePosition.x) + "," + Mathf.RoundToInt(tilePosition.y));

// 					//tilePosition = Map.transform.InverseTransformPoint (tilePosition);
// 					if(CheckTile(Map,tilePosition)==false){
// 						return false;
// 					}
// 					}
// 				}
// 			}
// 		}
// 		//left
// 		else if (transform.localRotation.eulerAngles.z == 90){
// 			print ("left");
// 			for (int i = 0; i <Height; i++) {
// 				for (int j = 0; j< Width; j++) {
// 					if (TileCollisions [i].Collisions [j].IsOccupied == true) {
// 					tilePosition = new Vector3 (i, j, 0f);
// 					tilePosition = new Vector2 (SpriteTopLeft.x + (tilePosition.x * 0.32f), (SpriteTopLeft.y + (-tilePosition.y * 0.32f)));
// 					//print (i + "," + j);
// 					print (tilePosition.x + " " + tilePosition.y);
// 					//print (Mathf.RoundToInt (tilePosition.x) + "," + Mathf.RoundToInt(tilePosition.y));
// 					if(CheckTile(Map,tilePosition)==false){
// 						return false;
// 					}
// 					}
// 				}
// 			}
// 		}
			
// 		return true;
// 	}

// 	bool CheckTile(STETilemap pMap, Vector3 pTilePos, bool gridYOffset=true){
// 		int gridX = TilemapUtils.GetGridX (Map, pTilePos);
// 		int gridY = TilemapUtils.GetGridY (Map, pTilePos);

// 		if (gridYOffset) {
// 			gridY -= 1;
// 		}

// 		Tile tile = Map.GetTile (gridX, gridY);
// 		uint rawData = Map.GetTileData (gridX,gridY);
// 		TileData tileData = new TileData (rawData);
// 		if (tileData.tileId==0) {
// 			return false;
// 		}
// 		return true;
// 	}

// 	public void Place(){
// 		print ("place");
// 		if (CanPlace) {
// 			GameObject spawnObj = Instantiate (Object, transform.position, transform.rotation);
// 			spawnObj.transform.position = transform.position;
// 			spawnObj.transform.rotation = transform.rotation;
// //			spawnObj.GetComponent<ObjectPosition> ().AdjustPositions ();
// //		print (InteractionManager.Instance.CurrentPlayerDirection.ToString ());
// 			SpriteTopLeft = new Vector3 (DisplaySprite.bounds.min.x, DisplaySprite.bounds.max.y, 0);
// 			spawnObj.GetComponent<ObjectPosition> ().AdjustPositions ();
// 			spawnObj.GetComponent<OccupySpace> ().OccupyTiles ();
// 		}
// 	}

//}
