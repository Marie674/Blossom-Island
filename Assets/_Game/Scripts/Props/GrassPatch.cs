using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CreativeSpore.SuperTilemapEditor;
public class GrassPatch : MonoBehaviour
{

    public List<GameObject> GrassPatches;
    public List<GrassTuft> Children = new List<GrassTuft>();
    private List<GrassTuft> SleepingChildren = new List<GrassTuft>();
    public int AwakeChildren = 0;

    public int GrowChance = 50;

    public int SpreadChance = 10;

    public bool Pregrow = false;

    bool init = false;
    void OnEnable()
    {

        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        TimeManager.OnDayChanged += newDay;
        foreach (GrassTuft child in Children)
        {
            SleepingChildren.Add(child);
            child.gameObject.SetActive(false);
        }
        foreach (HarvestObject child in Children)
        {
            child.OnObjectDestroyed += ChildDestroyed;
        }
        if (Pregrow == true)
        {
            foreach (GrassTuft tuft in Children)
            {
                tuft.gameObject.SetActive(true);
                SleepingChildren.Clear();
                AwakeChildren = Children.Count;
            }
        }
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= newDay;

        Children = Children.Where(x => x != null).ToList();
        foreach (HarvestObject child in Children)
        {
            child.OnObjectDestroyed -= ChildDestroyed;
        }
    }

    void newDay(int pDay)
    {
        if (init == false)
        {
            GetComponent<GrassPatchSaver>().OnRecordPersistentData();
            GetComponent<GrassPatchSaver>().OnApplyPersistentData();
            init = true;
        }

        Grow();
        Spread();
    }
    private void ChildDestroyed()
    {
        AwakeChildren -= 1;
        if (AwakeChildren < 1)
        {
            Destroy(gameObject);
        }
    }

    public void Grow()
    {


        SleepingChildren = SleepingChildren.Where(x => x != null).ToList();
        if (SleepingChildren.Count > 0)
        {
            int chance = GrowChance;
            int rand = Random.Range(0, 101);
            if (AwakeChildren < 1)
            {
                chance = 100;
            }
            if (rand <= chance)
            {
                rand = Random.Range(0, SleepingChildren.Count);
                SleepingChildren[rand].gameObject.SetActive(true);
                SleepingChildren.Remove(SleepingChildren[rand]);
                AwakeChildren += 1;
            }
        }
    }

    private void Spread()
    {
        if (GameObject.FindWithTag("Occupied Tiles") == null)
        {
            return;
        }
        if (AwakeChildren < 3)
        {
            return;
        }
        List<Vector2> emptyTiles = new List<Vector2>();
        STETilemap occupied = GameObject.FindWithTag("Occupied Tiles").GetComponent<STETilemap>();
        int rand = Random.Range(0, 101);
        if (rand <= SpreadChance)
        {
            GetComponent<ObjectPosition>().AdjustPositions();
            Vector2 Center = GetComponent<SpriteRenderer>().bounds.center;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Vector2 pos = new Vector2(Center.x + x, Center.y + y);
                    uint rawData = occupied.GetTileData(pos);
                    TileData tileData = new TileData(rawData);
                    int tileID = tileData.tileId;
                    if (tileID == 65535)
                    {
                        emptyTiles.Add(pos);
                    }
                }
            }
        }
        if (emptyTiles.Count < 1)
        {
            return;
        }
        rand = Random.Range(0, emptyTiles.Count);
        Vector2 newGrassPos = emptyTiles[rand];
        rand = Random.Range(0, GrassPatches.Count);
        GrassPatch newPatch = GameObject.Instantiate(GrassPatches[rand], newGrassPos, transform.rotation).GetComponent<GrassPatch>();
        newPatch.Grow();
    }

}
