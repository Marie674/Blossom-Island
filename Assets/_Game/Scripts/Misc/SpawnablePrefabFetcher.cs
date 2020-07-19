
#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEditor;
public class SpawnablePrefabFetcher : MonoBehaviour
{
    //stop trying to make fetch happen
    public string[] Folders;
    public void Fetch()
    {

        string[] allObjects = AssetDatabase.FindAssets("t:Prefab", Folders);
        SpawnedObjectManager manager = GetComponent<PixelCrushers.SpawnedObjectManager>();
        manager.spawnedObjectPrefabs.Clear();
        foreach (string obj in allObjects)
        {

            string assetPath = AssetDatabase.GUIDToAssetPath(obj);
            SpawnedObject asset = (SpawnedObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(SpawnedObject));
            if (asset != null)
            {
                manager.spawnedObjectPrefabs.Add(asset);

            }

        }
    }
}

#endif