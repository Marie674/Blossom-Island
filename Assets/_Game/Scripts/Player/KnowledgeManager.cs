using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;


public class KnowledgeManager : Singleton<KnowledgeManager>
{

    public List<int> KnownItems = new List<int>();
    public ItemPopUpUI ItemUI;

    void Start()
    {
        ItemUI = GameObject.FindObjectOfType<ItemPopUpUI>();

    }

    public void LearnItem(ItemBase pItem)
    {
        if (KnownItems.Contains(pItem.itemID))
        {
            return;
        }
        KnownItems.Add(pItem.itemID);
        //	ItemUI.Open(pItem);
    }

}
