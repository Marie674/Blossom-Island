using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using PixelCrushers.QuestMachine;
using PixelCrushers;
public class PlayerInventory : StorageObject
{
    public float Gold = 0;
    public float MaxGold = 1000000000;

    public override int Add(ItemBase item, uint pAmount, bool pDropLeftOvers = false)
    {

        if (item.itemName == "Coin")
        {
            ChangeGold(pAmount);
            for (int i = 0; i < pAmount; i++)
            {
                PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", item.itemName);

            }
            return (int)pAmount;
        }
        if (item.itemName == "HalfCoin")
        {
            ChangeGold(0.5f * pAmount);
            for (int i = 0; i < pAmount; i++)
            {
                PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", item.itemName);

            }
            return (int)pAmount;
        }

        else
        {
            int amt = base.Add(item, (uint)pAmount);
            if (amt > 0)
            {
                for (int i = 0; i < amt; i++)
                {
                    PixelCrushers.MessageSystem.SendMessage(gameObject, "GetItem", item.itemName);

                }
                KnowledgeManager.Instance.LearnItem(item);
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
