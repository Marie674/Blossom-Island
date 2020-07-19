using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;


[System.Serializable]
public struct LootItem
{
    public ItemContainer Item;
    [Tooltip("Max. 3 Decimals")]
    public float Chance;
}

[CreateAssetMenu(menuName = "LootTable")]
public class LootTable : ScriptableObject
{

    [SerializeField]
    public List<LootItem> Items;
    [SerializeField]
    public int MinDraws = 1;
    [SerializeField]
    public int MaxDraws = 1;

    [Tooltip("Will add the first item on the list to fill slots")]
    [SerializeField]
    public int MinSpawn = -1;

    public List<ItemBase> Output(int pMinSpawn = -1)
    {
        if (pMinSpawn != -1)
        {
            MinSpawn = pMinSpawn;
        }

        int spawned = 0;

        List<ItemBase> itemList = new List<ItemBase>();

        int totalChance = 0;

        float rand = Random.Range(0, 100);

        Dictionary<ItemBase, int> itemPool = new Dictionary<ItemBase, int>();

        foreach (LootItem loot in Items)
        {
            int lootChance = Mathf.RoundToInt(loot.Chance * 100);
            totalChance += lootChance;

            ItemBase lootItem = loot.Item.item;
            itemPool.Add(lootItem, lootChance);
        }
        if (totalChance > 10000)
        {
            totalChance = 10000;
        }

        ItemBase nullItem = ItemSystemUtility.GetItemCopy((int)GenericItems.NULL, ItemType.Generic);
        int remainingChance = 10000 - totalChance;
        itemPool.Add(nullItem, remainingChance);

        rand = Random.Range(MinDraws, MaxDraws + 1);

        Debug.Log(name + " drawing " + rand + " times");
        for (int i = 0; i < rand; i++)
        {
            ItemBase draw = WeightedRandomizer.From(itemPool).TakeOne();
            if (draw.itemID == nullItem.itemID)
            {
                itemList.Add(nullItem);
                Debug.Log(name + " drew: nothing");

            }
            else
            {
                spawned += 1;
                itemList.Add(draw);
                Debug.Log(name + " drew: " + draw.itemName);

            }
        }
        while (spawned < MinSpawn)
        {
            Debug.Log(name + " adding: " + Items[0].Item.item.itemName);

            itemList.Add(Items[0].Item.item);
            spawned += 1;
        }


        return itemList;
    }
}
