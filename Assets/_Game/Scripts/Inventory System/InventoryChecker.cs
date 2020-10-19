using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryChecker : MonoBehaviour
{
    public string InventoryID;

    public List<ContainedItem> WantedItems = new List<ContainedItem>();

    public UnityEvent Event;

    void OnEnable()
    {
        StorageObject.OnAddItem += CheckItems;
    }

    void OnDisable()
    {
        StorageObject.OnAddItem -= CheckItems;
    }

    public void Check(string pObjName)
    {
        CheckItems(GetInventory(pObjName));
    }

    StorageObject GetInventory(string pObjName)
    {
        GameObject obj = GameObject.Find(pObjName);
        if (obj != null)
        {
            StorageObject storage = obj.GetComponent<StorageObject>();
            if (storage != null)
            {
                return storage;
            }
        }
        return null;
    }

    void CheckItems(StorageObject pInventory)
    {
        if (pInventory == null)
        {
            return;
        }
        if (pInventory.UniqueID == InventoryID)
        {
            foreach (ContainedItem wantedTtem in WantedItems)
            {
                InventoryItemStack stack = pInventory.FindItemStack(wantedTtem.Item);
                if (stack == null)
                {
                    return;

                }
                else if (stack.Amount < wantedTtem.Amount)
                {
                    return;
                }
            }

            Event.Invoke();
        }

    }
}
