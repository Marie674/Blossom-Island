using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using TMPro;

[System.Serializable]
public struct StoreItem
{
    public ItemBase ContainedItem;
    public int Probability;
    public int CurrentAmount;
    public int MaxAmount;
    public bool SoldOut;
}

public class StoreItemSlot : MonoBehaviour
{
    public Sprite SoldOutSprite;
    public ItemBase CurrentItem;
    public int CurrentAmount = 0;
    public int MaxAmount = 99;
    public SpriteRenderer Sprite;
    public TextMeshProUGUI AmountText;
    public bool CanInteract = false;

    public bool Seasonal = false;

    [SerializeField] List<StoreItemModule> UpgradeModules = new List<StoreItemModule>();

    [SerializeField]
    public List<StoreItem> PossibleItems = new List<StoreItem>();
    [SerializeField]
    public List<StoreItem> SpringItems = new List<StoreItem>();
    [SerializeField]
    public List<StoreItem> SummerItems = new List<StoreItem>();
    [SerializeField]
    public List<StoreItem> FallItems = new List<StoreItem>();
    [SerializeField]
    public List<StoreItem> WinterItems = new List<StoreItem>();

    void OnEnable()
    {
        TimeManager.OnDayChanged += PopulateLists;

        TimeManager.OnDayChanged += PickItem;
    }

    void OnDisable()
    {
        TimeManager.OnDayChanged -= PopulateLists;

        TimeManager.OnDayChanged -= PickItem;
    }

    public void Interact()
    {

        if (CanInteract == false)
        {
            return;
        }


        BuyUI ui = FindObjectOfType<BuyUI>();

        float maxAmount = FindObjectOfType<PlayerInventory>().Gold / CurrentItem.Value;
        maxAmount = Mathf.Floor(maxAmount);

        if (maxAmount > CurrentAmount)
        {
            maxAmount = CurrentAmount;
        }
        // if (maxAmount <= 0)
        // {
        //     return;
        // }
        ui.Open(this, (int)maxAmount);

    }

    public void SoldOut()
    {
        CanInteract = false;
    }

    public void Buy(int pAmount)
    {
        ItemSpawner.Instance.SpawnItems(CurrentItem, GameManager.Instance.Player.transform.position, (uint)pAmount);
        CurrentAmount -= pAmount;
        FindObjectOfType<PlayerInventory>().ChangeGold(-(int)(CurrentItem.Value * pAmount));
        if (CurrentAmount == 0)
        {
            SoldOut();
        }
        UpdateVisuals();
    }

    void PopulateLists(int pCurrentDay)
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

    void PickItem(int pCurrentDay)
    {
        print("pick item");
        var weights = new Dictionary<StoreItem, int>();
        List<StoreItem> itemPool = new List<StoreItem>();

        foreach (StoreItem poolItem in PossibleItems)
        {
            if (!weights.ContainsKey(poolItem))
                weights.Add(poolItem, poolItem.Probability);
        }

        if (Seasonal == true)
        {
            switch (TimeManager.Instance.CurrentMonth.Name)
            {
                case TimeManager.MonthNames.Spring:
                    itemPool = SpringItems;
                    print("spring");
                    break;
                case TimeManager.MonthNames.Summer:
                    print("summer");
                    itemPool = SummerItems;
                    break;
                case TimeManager.MonthNames.Fall:
                    itemPool = FallItems;
                    print("fall");
                    break;
                case TimeManager.MonthNames.Winter:
                    itemPool = WinterItems;
                    print("winter");
                    break;
                default:
                    break;
            }
        }
        foreach (StoreItem poolItem in itemPool)
        {
            if (!weights.ContainsKey(poolItem))
                weights.Add(poolItem, poolItem.Probability);
        }
        if (weights.Count > 0)
        {

            StoreItem storeItem = WeightedRandomizer.From(weights).TakeOne();
            CurrentItem = ItemSystem.Instance.GetItemClone(storeItem.ContainedItem.ID);
            MaxAmount = storeItem.MaxAmount;
            CurrentAmount = storeItem.MaxAmount;
            CanInteract = true;
            UpdateVisuals();

        }
        GetComponent<StoreItemSlotSaver>().OnRecordPersistentData();
    }

    public void UpdateVisuals()
    {
        if (CurrentAmount > 0)
        {
            if (Sprite != null)
            {
                Sprite.color = Color.white;
                Sprite.sprite = CurrentItem.Icon;
            }
            if (AmountText != null)
            {
                AmountText.text = CurrentAmount.ToString();

            }

        }
        else
        {
            if (Sprite != null && SoldOutSprite != null)
            {
                Sprite.sprite = SoldOutSprite;

            }
            if (AmountText != null)
            {
                AmountText.text = string.Empty;

            }
        }
    }
}
