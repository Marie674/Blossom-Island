using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
public class ObjectSpawnerSaver : PixelCrushers.SpawnedObject
{
    ObjectSpawner TargetSpawner;
    string VariableName;
    public override void Start()
    {
        base.Start();
        TargetSpawner = GetComponent<ObjectSpawner>();

    }

    public override void OnEnable()
    {
        base.OnEnable();
        VariableName = name + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnRecordPersistentData()
    {
        DialogueLua.SetVariable(VariableName + "Object Amount", TargetSpawner.SpawnedObjects.Count);

    }
    void OnApplyPersistentData()
    {
        TargetSpawner.SpawnedObjects = new List<GameObject>();

        Collider2D[] colliders = TargetSpawner.GetOverlapObjects();
        foreach (Collider2D collider in colliders)
        {
            string prefabName = collider.gameObject.name.Replace("(Clone)", string.Empty);
            if (collider.gameObject == gameObject)
            {
                continue;
            }
            
            if(collider.GetComponent<PixelCrushers.SpawnedObject>()!=null){
                if(collider.GetComponent<PixelCrushers.SpawnedObject>().key.Contains("PlayerPlaced")){
                    continue;
                }
            }

            if (TargetSpawner.SmallObjects.Find(x => x != null && string.Equals(x.name, prefabName)) != null && collider.isTrigger == false)
            {
                if (!TargetSpawner.SpawnedObjects.Contains(collider.gameObject))
                {
                    TargetSpawner.SpawnedObjects.Add(collider.gameObject);

                }
            }
            else if (TargetSpawner.LargeObjects.Find(x => x != null && string.Equals(x.name, prefabName)) != null && collider.isTrigger == false)
            {
                if (!TargetSpawner.SpawnedObjects.Contains(collider.gameObject))
                {
                    TargetSpawner.SpawnedObjects.Add(collider.gameObject);

                }

            }
        }

        //get objects
    }

    // public void RegisterObject(string pName)
    // {
    //     DialogueLua.SetVariable(VariableName + "Object" + ((TargetSpawner.SpawnedObjects.Count) - 1), pName);
    // }

    // public void UnRegisterObject(int pIndex)
    // {
    //     DialogueLua.SetVariable(VariableName + "Object" + pIndex, "");
    // }


}
