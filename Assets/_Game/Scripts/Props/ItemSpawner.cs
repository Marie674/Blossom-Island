using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class ItemSpawner : Singleton<ItemSpawner>
{



    public WorldItem WorldItemPrefab;

    public void SpawnItems(ItemBase pItem, Vector3 pPos, uint pAmount = 1)
    {
        ItemBase nullItem = ItemSystemUtility.GetItemCopy((int)GenericItems.NULL, ItemType.Generic);
        if (pItem.itemID == nullItem.itemID)
        {
            return;
        }
        WorldItem itemTemplate = WorldItemPrefab;
        WorldItem item = Instantiate(itemTemplate, pPos, this.transform.rotation);
        item.SetItem(pItem, pAmount);
    }
}
