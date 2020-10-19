using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class ToolControllerGridItem : ToolControllerBase
{

    //	public ItemGrid CurrentItem;
    //
    //	public ItemContainer DirtItem;
    //	public ItemContainer SandItem;
    //	public ItemContainer TurfItem;
    //
    //	public ToolSlotUI ToolUI;
    //
    //	protected override bool CheckConditions (STETilemap pLayer = null, Tile pTile = null, Vector2 pTilePos= new Vector2()){
    //		if (GridItem == null) {
    //			return false;
    //		}
    //
    //		if (CheckEnergy () != true) {
    //			return false;
    //		}
    //
    //		//check if tile is occupied
    //		STETilemap map = GameManager.Instance.CurrentMap.Tilemaps[0];
    //		int gridX = TilemapUtils.GetGridX (map, pTilePos);
    //		int gridY = TilemapUtils.GetGridY (map, pTilePos);
    //		Tile tile = map.GetTile (gridX, gridY);
    //		uint rawData = map.GetTileData (gridX,gridY);
    //		TileData tileData = new TileData (rawData);
    //		if (tileData.tileId==0) {
    //			return false;
    //		}
    //			
    //		if (pLayer.ParentTilemapGroup.FindTilemapByName(pLayer.name) != pLayer.ParentTilemapGroup.FindTilemapByName(GridItem.CheckLayer)) {
    //			return false;		
    //		}
    //
    //		return true;
    //	}
    //		
    //
    //	protected override void ApplyEffects (STETilemap pLayer = null, Tile pTile = null, Vector2 pTilePos= new Vector2())
    //	{
    //		base.ApplyEffects ();
    //
    //		if (pLayer != null) {
    //			if (GridItem.IsSeed == true) {
    //				PlantSeed (pTilePos);
    //			}
    //			else if (GridItem.itemName == "Dirt") {
    //				SmoothGround(pLayer, pTile,pTilePos);
    //			} else {
    //				AddTile (pLayer.ParentTilemapGroup.FindTilemapByName(GridItem.AffectedLayer), pTile, pTilePos);
    //			}
    //		}
    //	}
    //
    //	void PlantSeed(Vector2 pTilePos = new Vector2()){
    //
    //		RaycastHit2D hit = Physics2D.Raycast (pTilePos, Vector2.zero);
    //		FarmPlot plot = hit.collider.gameObject.GetComponent<FarmPlot>();
    //		plot.PlantCrop (GridItem.CropName);
    //		PlayerInventory.Instance.Remove (PlayerInventory.Instance.FindItemStack (GridItem.itemID), 1);
    //
    //	}
    //		
    //	void AddTile(STETilemap pLayer, Tile pTile,Vector2 pTilePos= new Vector2()){
    //		
    //		STETilemap layer = pLayer.ParentTilemapGroup.FindTilemapByName (GridItem.AffectedLayer);
    //
    //		int gridX = TilemapUtils.GetGridX (layer,pTilePos);
    //		int gridY = TilemapUtils.GetGridY (layer,pTilePos);
    //
    //		uint rawData = layer.GetTileData (gridX,gridY);
    //		TileData tileData = new TileData (rawData);
    //
    //		tileData.brushId = GridItem.BrushId;
    //		tileData.tileId = GridItem.TileId;
    //
    //		layer.SetTileData(gridX,gridY,tileData.BuildData());
    //		layer.UpdateMeshImmediate();
    //		PlayerInventory.Instance.Remove (PlayerInventory.Instance.FindItemStack (GridItem.itemID), 1);
    //
    //	}
    //
    //	void SmoothGround(STETilemap pLayer, Tile pTile,Vector2 pTilePos= new Vector2()){
    //		RaycastHit2D hit = Physics2D.Raycast (pTilePos, Vector2.zero);
    //		FarmPlot plot = hit.collider.gameObject.GetComponent<FarmPlot>();
    //		Destroy (plot.gameObject);
    //		STETilemap layer = pLayer.ParentTilemapGroup.FindTilemapByName (GridItem.AffectedLayer);
    //		PlayerInventory.Instance.Remove (PlayerInventory.Instance.FindItemStack (GridItem.itemID), 1);
    //		layer.Erase (pTilePos);
    //		pLayer.UpdateMeshImmediate();
    //
    //	}

}
