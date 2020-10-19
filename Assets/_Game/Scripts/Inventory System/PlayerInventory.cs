using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.QuestMachine;
using PixelCrushers;
using Game.Items;
public class PlayerInventory : StorageObject
{
    public float Gold = 0;
    public float MaxGold = 1000000000;

    public override int Add(ItemBase pItem, uint pAmount, bool pDropLeftOvers = false)
    {

        if (pItem.Name == "Coin")
        {
            ChangeGold(pAmount);
            for (int i = 0; i < pAmount; i++)
            {
                PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", pItem.Name);

            }
            return (int)pAmount;
        }
        if (pItem.Name == "HalfCoin")
        {
            ChangeGold(0.5f * pAmount);
            for (int i = 0; i < pAmount; i++)
            {
                PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", pItem.Name);

            }
            return (int)pAmount;
        }

        else
        {
            int amt = base.Add(pItem, (uint)pAmount);
            if (amt > 0)
            {
                for (int i = 0; i < amt; i++)
                {
                    PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", pItem.Name);
                }
                KnowledgeManager.Instance.LearnItem(pItem);
                return amt;
            }
            else
            {
                return 0;
            }
        }
    }

    public delegate void GoldChange();
    public event GoldChange OnGoldChange;

    public float ChangeGold(float pAmount)
    {
        float goldToAdd = MaxGold - Gold;
        goldToAdd = Mathf.Clamp(goldToAdd, 0, pAmount);

        Gold = Mathf.Clamp(Gold + goldToAdd, 0, MaxGold);
        if (OnGoldChange != null)
        {
            OnGoldChange();
        }
        return goldToAdd;

    }

}
