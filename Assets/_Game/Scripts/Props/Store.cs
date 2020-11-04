using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
namespace Game.Stores
{

    [System.Serializable]
    public struct StoreItem
    {
        public ItemBase ContainedItem;
        public int Amount;
    }

    [RequireComponent(typeof(StorageObject))]
    public class Store : MonoBehaviour
    {

        public string Name = "Store";
        public List<StoreItem> PossibleItems = new List<StoreItem>();
        public List<StoreItem> SpringExclusives = new List<StoreItem>();
        public List<StoreItem> SummerExclusives = new List<StoreItem>();
        public List<StoreItem> FallExclusives = new List<StoreItem>();
        public List<StoreItem> WinterExclusives = new List<StoreItem>();
        public List<StoreItemModule> UpgradeModules = new List<StoreItemModule>();
        public List<StoreItem> CurrentItems = new List<StoreItem>();

        private StorageObject Storage;
        void OnEnable()
        {
            TimeManager.OnDayChanged += Populate;
            Storage = GetComponent<StorageObject>();
        }

        void OnDisable()
        {
            TimeManager.OnDayChanged -= Populate;
        }

        public void Interact()
        {
            FindObjectOfType<StoreUI>().Open(Storage, this);
        }

        void Populate(int pDayIndex)
        {
            PopulateLists();
            TimeManager.MonthNames currentSeason = TimeManager.Instance.CurrentMonth.Name;
            CurrentItems = PossibleItems;
            List<StoreItem> seasonalItems = new List<StoreItem>();
            switch (currentSeason)
            {
                case TimeManager.MonthNames.Spring:
                    seasonalItems = SpringExclusives;
                    break;
                case TimeManager.MonthNames.Summer:
                    seasonalItems = SummerExclusives;
                    break;
                case TimeManager.MonthNames.Fall:
                    seasonalItems = FallExclusives;
                    break;
                case TimeManager.MonthNames.Winter:
                    seasonalItems = WinterExclusives;
                    break;
                default:
                    break;
            }

            foreach (StoreItem item in seasonalItems)
            {
                if (!CurrentItems.Contains(item))
                {
                    CurrentItems.Add(item);
                }

            }

            PopulateStorage();
        }

        void PopulateLists()
        {
            print("populate from modules");
            foreach (StoreItemModule module in UpgradeModules)
            {
                if (module.CheckUnlock() == true)
                {
                    foreach (StoreItem item in module.Items)
                    {
                        if (!PossibleItems.Contains(item))
                        {
                            PossibleItems.Add(item);

                        }
                    }
                }
            }
        }

        void PopulateStorage()
        {
            Storage.ContainedStacks.Clear();
            foreach (StoreItem item in PossibleItems)
            {
                print(Storage.Add(item.ContainedItem, (uint)item.Amount));
            }
        }

    }
}