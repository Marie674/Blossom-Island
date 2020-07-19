using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

[System.Serializable]
public class OccupySpace : MonoBehaviour
{

    public int OccupiedTileId = 0;
    private SpriteRenderer Sprite;
    private Vector2 SpriteTopLeft;

    [SerializeField]
    public List<TileCollisionRow> TileCollisions = new List<TileCollisionRow>();

    [SerializeField]
    public bool Init = false;
    [SerializeField]
    public int Width = 0;
    [SerializeField]
    public int Height = 0;
    [SerializeField]
    public List<Vector2> Tiles = new List<Vector2>();

    public float Units = 1f;

    public bool OccupyOnStart = true;

    private bool IsQuitting;

    public STETilemap OccupiedLayer;



    // Use this for initialization
    void OnEnable()
    {
        OccupiedLayer = GameObject.FindWithTag("Occupied Tiles").GetComponent<STETilemap>();
        if (OccupyOnStart)
        {
            OccupyTiles();
        }
    }


    Tile GetTopTile(Vector2 pTileWorldPos)
    {
        if (OccupiedLayer == null)
        {
            return null;
        }
        TilemapGroup group = OccupiedLayer.transform.parent.GetComponent<TilemapGroup>();
        if (group == null)
        {
            return null;
        }
        //       print(group.name);
        if (group.Tilemaps.Count < 1)
        {
            return null;
        }
        for (int i = group.Tilemaps.Count - 1; i > 0; i--)
        {

            Tile tile = group.Tilemaps[i].GetTile(pTileWorldPos);

            if (tile != null)
            {
                return tile;
            }

        }
        return null;
    }

    public List<string> AllowedTerrains = new List<string>();
    public bool CheckTerrain = false;
    bool ValidateTileLayer(Tile pTile)
    {
        if (AllowedTerrains.Count == 0)
        {
            return true;
        }
        string tileTerrain = pTile.paramContainer.GetStringParam("Terrain");
        if (tileTerrain == string.Empty)
        {
            return false;
        }
        print(tileTerrain);
        if (AllowedTerrains.Contains(tileTerrain))
        {
            return true;
        }
        return false;
    }

    public void OccupyTiles()
    {
        //First snap to grid
        if (GetComponent<ObjectPosition>() != null)
        {
            GetComponent<ObjectPosition>().AdjustPositions();
        }

        Sprite = GetComponent<SpriteRenderer>();
        SpriteTopLeft = new Vector2(Sprite.bounds.min.x, Sprite.bounds.max.y);
        //Iterate through all tiles
        //Row by row, from top-left to bottom-right
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //if tile is marked as occupied
                if (TileCollisions[i].Collisions[j] == true)
                {
                    Vector2 tileWorldPos = SpriteTopLeft;
                    tileWorldPos.x += (Units * j) + Units / 2f;
                    tileWorldPos.y += (Units * -i) - Units / 2f;
                    OccupyTile(tileWorldPos);
                }
            }
        }

    }


    void OccupyTile(Vector2 pTileWorldPos)
    {

        uint rawTileData = GetTileData(pTileWorldPos);
        TileData tileData = new TileData(rawTileData);
        tileData.tileId = OccupiedTileId;
        tileData.rot90 = false;
        SetTileData(pTileWorldPos, tileData);

        Tiles.Add(pTileWorldPos);
    }

    public void InitGrid()
    {
        TileCollisions.Clear();
        Sprite = GetComponent<SpriteRenderer>();
        Width = Mathf.RoundToInt(Sprite.bounds.size.x / Units);
        Height = Mathf.RoundToInt(Sprite.bounds.size.y / Units);
        for (int i = 0; i < Height; i++)
        {
            TileCollisions.Add(new TileCollisionRow());
            for (int j = 0; j < Width; j++)
            {
                TileCollisions[i].Collisions.Add(false);
            }
        }
        Init = true;
    }

    uint GetTileData(Vector2 pTileWorldPos)
    {
        return OccupiedLayer.GetTileData(pTileWorldPos);
    }

    void SetTileData(Vector2 pTileWorldPos, TileData pTileData)
    {
        OccupiedLayer.SetTileData(pTileWorldPos, pTileData.BuildData());
        OccupiedLayer.UpdateMeshImmediate();
    }

    void EraseTile(Vector2 pTileWorldPos)
    {
        OccupiedLayer.Erase(pTileWorldPos);
        OccupiedLayer.UpdateMeshImmediate();
    }

    public bool CheckTile(Vector2 pTileWorldPos)
    {
        uint rawTileData = GetTileData(pTileWorldPos);
        TileData tileData = new TileData(rawTileData);
        //if tile is already occupied
        if (tileData.tileId != 65535)
        {
            return false;
        }

        if (CheckTerrain)
        {
            Tile tile = GetTopTile(pTileWorldPos);
            if (tile != null)
            {
                if (ValidateTileLayer(tile) == false)
                {
                    return false;
                }
            }
        }

        return true;

    }


    public Vector2 GetAdjustedPos(Vector2 pSpritePos)
    {
        Vector2 position = pSpritePos;
        Vector2 pivot = transform.position;
        Sprite = GetComponent<SpriteRenderer>(); ;
        float differenceX = Mathf.Abs(Sprite.bounds.min.x - pivot.x);
        float differenceY = Mathf.Abs(Sprite.bounds.max.y - pivot.y);
        float x = position.x + differenceX;
        float y = position.y - differenceY;

        Vector2 newPivotPos = new Vector2(x, y);
        return newPivotPos;
    }

    public Vector2 GetSpriteTopLeft(Vector2 pSpritePos)
    {
        Vector2 position = pSpritePos;
        Sprite = GetComponent<SpriteRenderer>();

        float differenceX = position.x - Sprite.bounds.min.x;
        float differenceY = position.y - Sprite.bounds.min.y;

        float x = position.x - differenceX;
        float y = position.y - differenceY;

        Vector2 spriteTopLeft = new Vector2(x, y);
        return spriteTopLeft;
    }
    public bool CheckTiles(Vector2 pSpriteTopLeft)
    {
        //First snap to grid
        OccupiedLayer = GameObject.FindWithTag("Occupied Tiles").GetComponent<STETilemap>();

        if (GetComponent<ObjectPosition>() != null)
        {
            GetComponent<ObjectPosition>().AdjustPositions();
        }
        Sprite = GetComponent<SpriteRenderer>();

        //Iterate through all tiles
        //Row by row, from top-left to bottom-right
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                //if tile is marked as occupied
                if (TileCollisions[i].Collisions[j] == true)
                {
                    Vector2 tileWorldPos = pSpriteTopLeft;
                    //					print(tileWorldPos);
                    tileWorldPos.x += (Units * j) + (Units / 2f);
                    tileWorldPos.y += (Units * -i) - (Units / 2f);
                    //                    print(tileWorldPos);
                    Debug.DrawRay(tileWorldPos, Vector3.right, Color.red, 3f);

                    if (CheckTile(tileWorldPos) == false)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void UnOccupyTiles()
    {
        foreach (Vector2 tile in Tiles)
        {
            EraseTile(tile);
        }
    }

    void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    void OnDestroy()
    {
        if (IsQuitting != true)
        {
            if (OccupiedLayer != null)
                UnOccupyTiles();
        }
    }

}
