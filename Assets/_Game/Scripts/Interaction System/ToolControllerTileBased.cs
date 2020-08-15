using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
public class ToolControllerTileBased : ToolControllerBase
{
    [SerializeField]
    public List<AffectedLayer> AffectedLayerNames = new List<AffectedLayer>();
    protected List<STETilemap> AffectedLayers = new List<STETilemap>();
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.OnSceneChanged += GetLayers;
        GetLayers();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnSceneChanged -= GetLayers;
    }


    public override bool CheckUseValidity()
    {
        if (base.CheckUseValidity() == false)
        {
            return false;
        }

        int affectedTileAmount = 0;

        List<Vector2> tiles = ToolCursorManager.Instance.GetTiles();

        foreach (Vector2 tile in tiles)
        {
            if (CheckTileValidity(tile))
            {
                affectedTileAmount += 1;
            }
        }
        // if (affectedTileAmount < 1)
        // {
        //     return false;
        // }

        return true;
    }

    protected virtual void GetLayers()
    {
        TilemapGroup CurrentMap = GameManager.Instance.LevelInfo.CurrentMap;
        AffectedLayers.Clear();
        foreach (STETilemap layer in CurrentMap.Tilemaps)
        {
            foreach (AffectedLayer layerName in AffectedLayerNames)
            {
                if (layer.gameObject.name == layerName.Name)
                {
                    AffectedLayers.Add(layer);
                }
            }
        }
    }


    protected virtual STETilemap GetTileAffectedLayer(Vector2 pTileWorldPos)
    {
        for (int i = AffectedLayers.Count - 1; i >= 0; i--)
        {
            uint rawTileData = GetTileData(pTileWorldPos, AffectedLayers[i]);
            TileData tileData = new TileData(rawTileData);
            if (tileData.tileId != 65535)
            {
                return AffectedLayers[i];
            }

        }

        return null;
    }


    protected uint GetTileData(Vector2 pTileWorldPos, STETilemap pLayer)
    {
        return pLayer.GetTileData(pTileWorldPos);
    }
}
