using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

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
        if (KnownItems.Contains(pItem.ID))
        {
            return;
        }
        KnownItems.Add(pItem.ID);
        //	ItemUI.Open(pItem);
    }

}
