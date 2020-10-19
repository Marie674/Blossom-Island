using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

[System.Serializable]
public struct AffectedTerrain
{
    public string LowerLayerName;
    public string LayerName;
    public int BrushId;
}
public class ToolControllerTerrainItem : ToolControllerTileBased
{
    public List<AffectedTerrain> AffectedTerrains = new List<AffectedTerrain>();

    protected override IEnumerator UseCountdown()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameManager.Instance.Player.DoAction(CurrentTool.PlayerTrigger, CurrentTool.UseInterval, pos, 0, CurrentTool.ToolTrigger, true);
        List<Vector2> tiles = ToolCursorManager.Instance.GetTiles();

        yield return new WaitForSeconds(CurrentTool.UseInterval);
        ProceedUse(tiles);
    }
    protected override void ProceedUse(List<Vector2> pTiles)
    {


        NeedBase energyNeed = PlayerNeedManager.Instance.GetNeed("Energy");
        energyNeed.Change(-CurrentTool.EnergyCost * (ToolCursorManager.Instance.CursorIndex + 1));

        List<Vector2> tiles = pTiles;

        foreach (Vector2 tile in tiles)
        {
            if (CheckTileValidity(tile))
            {
                STETilemap affectedLayer = GetTileAffectedLayer(tile);

                uint rawData = affectedLayer.GetTileData(tile);
                TileData tileData = new TileData(rawData);
                foreach (AffectedTerrain terrain in AffectedTerrains)
                {
                    if (affectedLayer.name == terrain.LayerName)
                    {
                        tileData.brushId = terrain.BrushId;
                    }
                }
                if (affectedLayer.name == "Dirt")
                {
                    FarmPlot plot = GetPlot(tile);
                    if (plot != null)
                    {
                        Destroy(plot.gameObject);
                    }
                }

                affectedLayer.SetTileData(tile, tileData.BuildData());
                affectedLayer.UpdateMeshImmediate();

                FindObjectOfType<PlayerInventory>().RemoveFromStack(Toolbar.Instance.SelectedSlot.ReferencedItemStack, 1);
                if (Toolbar.Instance.SelectedSlot.ReferencedItemStack == null)
                {
                    return;
                }
            }
        }

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
    public override bool CheckTileValidity(Vector2 pTileWorldPos)
    {
        if (CheckTileNotOccupied(pTileWorldPos) == false)
        {
            return false;
        }
        STETilemap affectedLayer = GetTileAffectedLayer(pTileWorldPos);
        if (affectedLayer == null)
        {
            return false;
        }
        STETilemap lowerLayer = GetLowerLayer(affectedLayer);
        if (lowerLayer == null)
        {
            return false;
        }
        uint rawTileData = GetTileData(pTileWorldPos, lowerLayer);
        TileData tileData = new TileData(rawTileData);
        if (tileData.tileId == 65535)
        {
            return false;
        }

        return true;
    }

    STETilemap GetLowerLayer(STETilemap affectedLayer)
    {
        TilemapGroup CurrentMap = GameManager.Instance.LevelInfo.CurrentMap;
        string lowerLayerName = string.Empty;

        foreach (AffectedTerrain terrain in AffectedTerrains)
        {
            if (terrain.LayerName == affectedLayer.name)
            {
                lowerLayerName = terrain.LowerLayerName;
                break;
            }
        }
        foreach (STETilemap layer in CurrentMap.Tilemaps)
        {
            if (layer.name == lowerLayerName)
            {
                return layer;
            }
        }
        return null;
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

    protected override void GetLayers()
    {
        TilemapGroup CurrentMap = GameManager.Instance.LevelInfo.CurrentMap;
        AffectedLayers.Clear();
        foreach (STETilemap layer in CurrentMap.Tilemaps)
        {
            foreach (AffectedTerrain terrain in AffectedTerrains)
            {
                if (layer.gameObject.name == terrain.LayerName)
                {
                    AffectedLayers.Add(layer);
                }
            }
        }
    }


    protected override STETilemap GetTileAffectedLayer(Vector2 pTileWorldPos)
    {
        for (int i = 0; i < AffectedLayers.Count; i++)
        {
            uint rawTileData = GetTileData(pTileWorldPos, AffectedLayers[i]);
            TileData tileData = new TileData(rawTileData);
            if (tileData.tileId == 65535)
            {
                return AffectedLayers[i];
            }

        }
        return null;
    }

}
