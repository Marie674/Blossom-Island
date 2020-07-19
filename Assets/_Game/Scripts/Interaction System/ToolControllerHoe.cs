using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using CreativeSpore.SuperTilemapEditor;
public class ToolControllerHoe : ToolControllerTileBased
{

    public GameObject PlotPrefab;

    protected override IEnumerator UseCountdown()
    {
        Vector2 pos = ToolCursorManager.Instance.CurrentCursor.transform.position;
        GameManager.Instance.Player.DoAction(CurrentTool.trigger, CurrentTool.useInterval, pos, 0, CurrentTool.toolTrigger, true);
        List<Vector2> tiles = ToolCursorManager.Instance.GetTiles();

        yield return new WaitForSeconds(CurrentTool.useInterval);
        ProceedUse(tiles);
    }
    protected override void ProceedUse(List<Vector2> pTiles)
    {

        //Spend player energy
        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        energyNeed.Change(-CurrentTool.energyCost * (ToolCursorManager.Instance.CursorIndex + 1));

        List<Vector2> tiles = pTiles;

        foreach (Vector2 tile in tiles)
        {
            if (CheckTileValidity(tile))
            {
                FarmPlot plot = GetPlot(tile);
                if (plot != null && plot.Crop != null)
                {
                    plot.Crop.SendMessage("Hit", "Hoe");
                    return;
                }

                STETilemap affectedLayer = GetTileAffectedLayer(tile);
                affectedLayer.Erase(tile);
                affectedLayer.UpdateMeshImmediate();
                if (affectedLayer.name == "Dirt")
                {
                    GameObject.Instantiate(PlotPrefab, tile, this.transform.rotation);
                }
                foreach (AffectedLayer layer in AffectedLayerNames)
                {
                    if (layer.Name == affectedLayer.name)
                    {
                        int rand = Random.Range(0, 100);
                        if (rand <= layer.Output.Chance)
                        {
                            ItemSpawner.Instance.SpawnItems(layer.Output.Item.item, tile, (uint)layer.Output.Amount);
                        }
                    }
                }

            }

        }

    }
    public override bool CheckTileValidity(Vector2 pTileWorldPos)
    {
        FarmPlot plot = GetPlot(pTileWorldPos);
        if (CheckTileNotOccupied(pTileWorldPos) == false && plot == null || plot != null && plot.Crop == null)
        {
            return false;
        }
        STETilemap affectedLayer = GetTileAffectedLayer(pTileWorldPos);
        if (affectedLayer == null && plot == null)
        {
            return false;
        }
        return true;
    }

    FarmPlot GetPlot(Vector2 pTileWorldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pTileWorldPos, Vector2.zero, 0.5f, LayerMask.GetMask("Farm"));
        if (hit.collider == null)
        {
            return null;
        }
        if (hit.collider.GetComponent<FarmPlot>() == null)
        {
            return null;
        }

        FarmPlot farmPlot = hit.collider.gameObject.GetComponent<FarmPlot>();
        return farmPlot;
    }

    bool CheckTileNotOccupied(Vector2 pTileWorldPos)
    {
        STETilemap occupiedLayer = GameObject.FindGameObjectWithTag("Occupied Tiles").GetComponent<STETilemap>();
        uint rawTileData = GetTileData(pTileWorldPos, occupiedLayer);
        TileData tileData = new TileData(rawTileData);
        //if tile is already occupied
        if (tileData.tileId != 65535)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


}
