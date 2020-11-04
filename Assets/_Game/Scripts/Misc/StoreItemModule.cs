using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

namespace Game.Stores
{
    [System.Serializable]
    public struct StoreShippedItemUnlockCondition
    {
        public ItemBase Item;
        public int Amount;
    }

    [CreateAssetMenu(menuName = "StoreItemModule")]
    public class StoreItemModule : ScriptableObject
    {
        [SerializeField]
        public List<StoreItem> Items = new List<StoreItem>();

        public List<StoreShippedItemUnlockCondition> ShippedItemConditions = new List<StoreShippedItemUnlockCondition>();

        public int TimedUnlockCondition = 0;

        public List<TimeManager.MonthNames> SeasonUnlockConditions = new List<TimeManager.MonthNames>();

        public bool CheckUnlock()
        {
            foreach (StoreShippedItemUnlockCondition condition in ShippedItemConditions)
            {
                if (GameManager.Instance.GetShippedItemAmount(condition.Item) < condition.Amount)
                {
                    return false;
                }
            }
            if (TimeManager.Instance.PassedDays < TimedUnlockCondition)
            {
                return false;
            }
            if (SeasonUnlockConditions.Count > 0 && !SeasonUnlockConditions.Contains(TimeManager.Instance.CurrentMonth.Name))
            {
                return false;
            }
            return true;
        }
    }
}