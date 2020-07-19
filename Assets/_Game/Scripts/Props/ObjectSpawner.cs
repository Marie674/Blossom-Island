using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
using PixelCrushers.DialogueSystem;
using UnityEditor;
using System.Linq;
public class ObjectSpawner : MonoBehaviour
{

    public bool ReplaceObjects;

    public int MaxObjects = 0;
    public List<GameObject> SmallObjects = new List<GameObject>();

    public List<GameObject> LargeObjects = new List<GameObject>();

    public int SmallObjectsAmount;

    public int LargeObjectAmount;

    public TimeManager.MonthNames Season;
    public bool RestrictSeason = false;

    public WeatherManager.WeatherTypeName Weather;
    public bool RestrictWeather = false;



    public int TryAmount = 5;

    float minX;
    float maxX;

    float minY;
    float maxY;

    STETilemap OccupiedTiles;

    public List<GameObject> SpawnedObjects = new List<GameObject>();
    void Start()
    {
        OccupiedTiles = GameObject.FindGameObjectWithTag("Occupied Tiles").GetComponent<STETilemap>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        Vector2 extents = collider.bounds.extents;
        Vector2 center = collider.bounds.center;
        Vector2 topLeft = new Vector2(center.x - extents.x, center.y + extents.y);
        Vector2 snapPos = new Vector2(1f * Mathf.Round(topLeft.x / 1f), 1f * Mathf.Round(topLeft.y / 1f));
        Vector3 posDifference = topLeft - snapPos;
        Vector3 newPos = transform.position - posDifference;
        transform.position = newPos;

        minX = collider.bounds.min.x;
        maxX = collider.bounds.max.x;
        minY = collider.bounds.min.y;
        maxY = collider.bounds.max.y;

        //        print("Min: " + minX + "," + minY + " Max:" + maxX + "," + maxY);
    }

    void OnEnable()
    {
        TimeManager.OnDayChanged += NewDay;
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= NewDay;
    }

    void SpawnObject(GameObject pObj, Vector2 pPos, bool pInspector = false)
    {

#if (UNITY_EDITOR) 
        GameObject newObj =  PrefabUtility.InstantiatePrefab(pObj) as GameObject;

#else
print("aaa");
        GameObject newObj =  Instantiate(pObj);
        
#endif
        newObj.transform.position = pPos;
        newObj.transform.parent = transform;
        //newObj.GetComponent<SpawnedObjectData>().KeyName = pObj.name + newObj.transform.position.x.ToString("F2") + newObj.transform.position.y.ToString("F2");
        SpawnedObjects.Add(newObj);
        newObj.GetComponent<OccupySpace>().OccupiedLayer = GameObject.FindWithTag("Occupied Tiles").GetComponent<STETilemap>();

        if (newObj.GetComponent<PixelCrushers.SpawnedObject>() != null)
        {
            newObj.GetComponent<PixelCrushers.SpawnedObject>().key = newObj.name + SpawnedObjects.Count;

        }
        if (newObj.GetComponent<PersistentDestructible>() != null)
        {
            newObj.GetComponent<PersistentDestructible>().variableName = newObj.name + SpawnedObjects.Count;

        }
        newObj.GetComponent<OccupySpace>().OccupyTiles();
        //GetComponent<ObjectSpawnerSaver>().RegisterObject(newObj.GetComponent<SpawnedObjectData>().KeyName);
        //add on object destroyed listener, subscribe to removeobject
        //newObj.GetComponent<SpawnedObjectData>().OnObjectDestroyed += RemoveObject;

        if (pInspector == true)
        {
            DestroyImmediate(newObj.GetComponent<PixelCrushers.SpawnedObject>());
        }
    }

    public Collider2D[] GetOverlapObjects()
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        return Physics2D.OverlapBoxAll(coll.bounds.center, coll.size, 0f);
    }
    void RemoveObject(GameObject pObj)
    {
        //get real index
        //int index = SpawnedObjects.IndexOf(pObj);
        //GetComponent<ObjectSpawnerSaver>().UnRegisterObject(index);
        SpawnedObjects.Remove(pObj);
    }

    public void EditorGenerate()
    {
        OccupiedTiles = GameObject.FindGameObjectWithTag("Occupied Tiles").GetComponent<STETilemap>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        Vector2 extents = collider.bounds.extents;
        Vector2 center = collider.bounds.center;
        Vector2 topLeft = new Vector2(center.x - extents.x, center.y + extents.y);
        Vector2 snapPos = new Vector2(1 * Mathf.Round(topLeft.x / 1), 1 * Mathf.Round(topLeft.y / 1f));
        Vector3 posDifference = topLeft - snapPos;
        Vector3 newPos = transform.position - posDifference;
        transform.position = newPos;

        minX = collider.bounds.min.x;
        maxX = collider.bounds.max.x;
        minY = collider.bounds.min.y;
        maxY = collider.bounds.max.y;

        Generate(true);
    }

    void NewDay(int pDayIndex)
    {
        Generate();
    }

    void Generate(bool pInspector = false)
    {
        if (ReplaceObjects)
        {
            DestroyObjects();
        }

        if (RestrictSeason == true && TimeManager.Instance.CurrentMonth.Name != Season)
        {
            return;
        }
        if (RestrictWeather == true && WeatherManager.Instance.CurrentWeather.Name != Weather)
        {
            return;
        }

        for (int i = 0; i < LargeObjectAmount; i++)
        {
            //            print("object " + i);

            if (MaxObjects > 0 && SpawnedObjects.Count >= MaxObjects)
            {
                return;
            }
            GameObject obj = GetLargeObject();
            int tries = 0;
            while (tries < TryAmount)
            {

                float newMaxX = maxX;
                float newMinY = minY;

                float spriteWidth = obj.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

                float spriteHeight = obj.GetComponent<SpriteRenderer>().bounds.extents.y * 2;



                newMaxX -= spriteWidth;

                newMinY += spriteHeight;

                //print("width: " + spriteWidth);

                float x = Random.Range(minX, newMaxX);
                float y = Random.Range(newMinY, maxY);
                x = 1 * (Mathf.Round(x / 1f));
                y = 1 * (Mathf.Round(y / 1f));

                Vector2 topLeftPosition = new Vector2(x, y);
                Vector2 pivotPosition = obj.GetComponent<OccupySpace>().GetAdjustedPos(topLeftPosition);

                //print("Corner: " + topLeftPosition.ToString("F3"));
                //print("Pivot: " + pivotPosition.ToString("F3"));
                // float tileX = BrushUtil.GetGridX(position, new Vector2(0, 32f));
                // float tileY = BrushUtil.GetGridY(position, new Vector2(0, 32f));
                // Vector2 tilePosition = new Vector2(tileX, tileY);

                //                print("Position: " + pivotPosition);
                if (CheckCanSpawn(obj, topLeftPosition))
                {
                    //spawn object
                    SpawnObject(obj, pivotPosition, pInspector);
                    //                    print("Spawned" + obj.name);
                    break;
                }
                //               print("Cannot spawn at position");
                tries += 1;
            }

        }
        //print("Generating small objects");
        for (int i = 0; i < SmallObjectsAmount; i++)
        {
            if (MaxObjects > 0 && SpawnedObjects.Count >= MaxObjects)
            {
                return;
            }
            GameObject obj = GetSmallObject();
            //print("Object " + i + ": " + obj.name);

            int tries = 0;
            while (tries < TryAmount)
            {
                //                print("Try " + tries);
                float newMaxX = maxX;
                float newMinY = minY;

                float spriteWidth = obj.GetComponent<SpriteRenderer>().bounds.extents.x * 2;

                float spriteHeight = obj.GetComponent<SpriteRenderer>().bounds.extents.y * 2;



                newMaxX -= spriteWidth;

                newMinY += spriteHeight;

                float x = Random.Range(minX, newMaxX);
                float y = Random.Range(newMinY, maxY);

                x = 1f * (Mathf.Round(x / 1));
                y = 1f * (Mathf.Round(y / 1));


                Vector2 topLeftPosition = new Vector2(x, y);
                Vector2 pivotPosition = obj.GetComponent<OccupySpace>().GetAdjustedPos(topLeftPosition);


                //print("Corner: " + topLeftPosition.ToString("F3"));
                //print("Pivot: " + pivotPosition.ToString("F3"));
                // float tileX = BrushUtil.GetGridX(position, new Vector2(0, 32f));
                // float tileY = BrushUtil.GetGridY(position, new Vector2(0, 32f));
                // Vector2 tilePosition = new Vector2(tileX, tileY);


                if (CheckCanSpawn(obj, topLeftPosition))
                {
                    //spawn object
                    SpawnObject(obj, pivotPosition, pInspector);

                    //print("Spawned" + obj.name);
                    break;
                }
                // print("Cannot spawn at position");
                tries += 1;
            }
            //print("All tries spent");
        }


    }

    void DestroyObjects()
    {
        SpawnedObjects = SpawnedObjects.Where(i => i != null).ToList();

        foreach (GameObject obj in SpawnedObjects)
        {
            Destroy(obj);
        }
        SpawnedObjects = new List<GameObject>();
    }

    bool CheckCanSpawn(GameObject pObj, Vector2 pPos)
    {
        return pObj.GetComponent<OccupySpace>().CheckTiles(pPos);
    }

    GameObject GetSmallObject()
    {
        int rand = Random.Range(0, SmallObjects.Count);
        return SmallObjects[rand];
    }

    GameObject GetLargeObject()
    {
        int rand = Random.Range(0, LargeObjects.Count);
        return LargeObjects[rand];
    }
    void TryGenerate(GameObject pObj)
    {

    }
}
