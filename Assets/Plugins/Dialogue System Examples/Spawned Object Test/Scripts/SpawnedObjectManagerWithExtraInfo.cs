using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers;

// This subclass of SpawnedObjectManager specially-handles spawned objects
// that use the SpawnedObjectWithExtraInfo class.
public class SpawnedObjectManagerWithExtraInfo : SpawnedObjectManager
{

    // For each spawned object that uses the SpawnedObjectWithExtraInfo class, record extra info:
    [Serializable]
    public class SpawnedObjectExtraInfoDataList
    {
        public List<string> list = new List<string>();
    }

    private const string SpecialDivider = "%%%";

    public override string RecordData()
    {
        // Record the regular spawned object manager data:
        var regularData = base.RecordData();

        // Record extra data for our special objects:
        var extraInfoDataList = new SpawnedObjectExtraInfoDataList();
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            var objectWithExtraInfo = spawnedObjects[i].GetComponent<SpawnedObjectWithExtraInfo>();
            if (objectWithExtraInfo != null)
            {
                // If this is a special object, gets its extra info:
                extraInfoDataList.list.Add(objectWithExtraInfo.GetExtraInfo());
            }
        }
        var extraData = SaveSystem.Serialize(extraInfoDataList);

        // Return a combo of regular data + our extra data:
        return regularData + SpecialDivider + extraData;
    }

    public override void ApplyData(string data)
    {
        if (string.IsNullOrEmpty(data)) return;

        // Split data into regular data and our extra data:
        string[] parts = data.Split(new string[] { SpecialDivider }, StringSplitOptions.None);
        var regularData = parts[0];
        var extraData = parts[1];

        // Apply regular data, which instantiates the spawned objects:
        base.ApplyData(regularData);

        // We need to wait until the spawned objects have initialized, then apply the extra data:
        StartCoroutine(ApplyExtraData(extraData));
    }

    private IEnumerator ApplyExtraData(string extraData)
    {
        // Wait for end of frame, when spawned objects have initialized:
        yield return new WaitForEndOfFrame();

        // Apply extra data to special objects:
        var extraInfoDataList = SaveSystem.Deserialize<SpawnedObjectExtraInfoDataList>(extraData);
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            var objectWithExtraInfo = spawnedObjects[i].GetComponent<SpawnedObjectWithExtraInfo>();
            if (objectWithExtraInfo != null)
            {
                // If this is a main object, apply the next extra data in the list:
                objectWithExtraInfo.ApplyExtraInfo(extraInfoDataList.list[0]);

                // Then remove that extra data from the list:
                extraInfoDataList.list.RemoveAt(0);
            }
        }
    }

}