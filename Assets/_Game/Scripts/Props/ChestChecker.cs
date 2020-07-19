using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestChecker : MonoBehaviour
{
    public UnityEvent Event;
    // Start is called before the first frame update
    public string ChestID;

    // void OnEnable()
    // {
    //     StorageObject.OnChestOpen += CheckChest;
    // }

    // void OnDisable()
    // {
    //     StorageObject.OnChestOpen -= CheckChest;
    // }

    void CheckChest(StorageObject pChest)
    {
        if (pChest.UniqueID == ChestID)
            Event.Invoke();
    }
}
