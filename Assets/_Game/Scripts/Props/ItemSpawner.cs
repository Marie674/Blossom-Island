using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public class ItemSpawner : Singleton<ItemSpawner>
{
    public WorldItem WorldItemPrefab;
    public void SpawnItems(ItemBase pItem, Vector3 pPos, uint pAmount = 1)
    {

        if (pItem.ID < 0)
        {
            return;
        }
        WorldItem itemTemplate = WorldItemPrefab;
        WorldItem item = Instantiate(itemTemplate, pPos, this.transform.rotation);
        item.SetItem(pItem, pAmount);
    }
}
