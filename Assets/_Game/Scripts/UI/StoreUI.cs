using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;
using TMPro;

namespace Game.Stores
{

    [RequireComponent(typeof(StorageObject))]
    public class StoreUI : MonoBehaviour
    {

        public ItemStackUI StackUI;
        public StoreItemUI ItemUI;

        public TextMeshProUGUI WeightText;
        public TextMeshProUGUI SlotsText;

        public TextMeshProUGUI TotalPriceText;

        public Button BuyButton;
        private PlayerInventory PlayerInventory;

        private StorageObject BasketInventory;

        private StorageObject StoreInventory;

        private Store CurrentStore;
        public TextMeshProUGUI TitleText;

        public TextMeshProUGUI SoldOutText;

        public Transform StoreItemsContainer;
        public Transform BasketItemsContainer;

        private float BasketValue = 0;

        public void Open(StorageObject pStoreInventory, Store pCurrentStore)
        {

            PlayerInventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();
            BasketInventory = GetComponent<StorageObject>();
            StoreInventory = pStoreInventory;
            CurrentStore = pCurrentStore;
            TitleText.text = CurrentStore.Name;
            DrawStore();
            DrawBasket();
            GetComponent<WindowToggle>().Open();
        }

        public void Close()
        {
            if (StoreInventory != null)
            {
                foreach (InventoryItemStack item in BasketInventory.ContainedStacks)
                {
                    StoreInventory.Add(item.ContainedItem, (uint)item.Amount);
                }
            }
            BasketInventory.ContainedStacks.Clear();
            GetComponent<WindowToggle>().Close();
        }

        void DrawStore()
        {
            ClearStoreItems();
            foreach (InventoryItemStack itemStack in StoreInventory.ContainedStacks)
            {
                StoreItemUI itemUI = Instantiate(ItemUI, StoreItemsContainer);
                itemUI.ItemIcon.sprite = itemStack.ContainedItem.Icon;
                itemUI.ItemAmountText.text = itemStack.Amount.ToString();
                itemUI.ItemNameText.text = itemStack.ContainedItem.Name;
                float price = itemStack.ContainedItem.Value + (itemStack.ContainedItem.Value * (itemStack.ContainedItem.Markup / 100));
                itemUI.ItemPriceText.text = price.ToString("F2");
                itemUI.GetComponent<Button>().onClick.AddListener(
                    delegate ()
                    {
                        AddToBasket(itemStack);
                    }
                );

            }
            if (StoreInventory.ContainedStacks.Count > 0)
            {
                SoldOutText.gameObject.SetActive(false);
            }
            else
            {
                SoldOutText.gameObject.SetActive(true);
            }
        }

        void ClearStoreItems()
        {
            StoreItemUI[] drawnItems = StoreItemsContainer.GetComponentsInChildren<StoreItemUI>();
            for (int i = 0; i < drawnItems.Length; i++)
            {
                Destroy(drawnItems[i].gameObject);
            }
        }

        void ClearBasketItems()
        {
            ItemStackUI[] drawnItems = BasketItemsContainer.GetComponentsInChildren<ItemStackUI>();
            for (int i = 0; i < drawnItems.Length; i++)
            {
                Destroy(drawnItems[i].gameObject);
            }
        }

        public void AddToBasket(InventoryItemStack pStack)
        {
            StoreInventory.RemoveFromStack(pStack, 1);
            BasketInventory.Add(pStack.ContainedItem, 1);
            DrawStore();
            DrawBasket();
        }

        public void RemoveFromBasket(InventoryItemStack pStack)
        {
            StoreInventory.Add(pStack.ContainedItem, 1);
            BasketInventory.RemoveFromStack(pStack, 1);
            DrawStore();
            DrawBasket();
        }

        public void DrawBasket()
        {
            ClearBasketItems();
            BuyButton.onClick.RemoveAllListeners();
            float weight = 0;
            BasketValue = 0;
            int slotAmt = BasketInventory.ContainedStacks.Count;
            float availableWeight = PlayerInventory.MaxWeight - PlayerInventory.CurrentWeight;
            float availableStacks = PlayerInventory.MaxStacks - PlayerInventory.ContainedStacks.Count;
            bool canBuy = true;
            foreach (InventoryItemStack itemStack in BasketInventory.ContainedStacks)
            {
                ItemStackUI itemUI = Instantiate(StackUI, BasketItemsContainer);
                itemUI.ItemIcon.sprite = itemStack.ContainedItem.Icon;
                itemUI.ItemAmount.text = itemStack.Amount.ToString();
                float price = itemStack.Amount * ((itemStack.ContainedItem.Value) + (itemStack.ContainedItem.Value * (itemStack.ContainedItem.Markup / 100)));
                itemUI.ItemIcon.color = Color.white;
                itemUI.GetComponent<Button>().interactable = true;
                itemUI.GetComponent<Button>().onClick.AddListener(
                    delegate ()
                    {
                        RemoveFromBasket(itemStack);
                    }
                );
                weight += itemStack.Amount * itemStack.ContainedItem.Weight;
                BasketValue += price;
            }
            WeightText.text = "Weight: " + weight.ToString("F2") + "/" + availableWeight.ToString("F2");
            if (weight <= availableWeight)
            {
                WeightText.color = Color.green;
            }
            else
            {
                WeightText.color = Color.red;
                canBuy = false;
            }
            SlotsText.text = "Slots: " + slotAmt.ToString() + "/" + availableStacks.ToString();
            if (slotAmt <= availableStacks)
            {
                SlotsText.color = Color.green;
            }
            else
            {
                SlotsText.color = Color.red;
                canBuy = false;
            }
            TotalPriceText.text = "Total: " + BasketValue.ToString("F2") + "/" + PlayerInventory.Gold.ToString("F2");
            if (BasketValue <= PlayerInventory.Gold)
            {
                TotalPriceText.color = Color.green;
            }
            else
            {
                TotalPriceText.color = Color.red;
                canBuy = false;
            }

            if (canBuy && BasketInventory.ContainedStacks.Count > 0)
            {
                BuyButton.interactable = true;
                BuyButton.onClick.AddListener(delegate () { BuyBasket(); });
            }
            else
            {
                BuyButton.interactable = false;
            }
        }


        void BuyBasket()
        {
            foreach (InventoryItemStack item in BasketInventory.ContainedStacks)
            {
                PlayerInventory.Add(item.ContainedItem, (uint)item.Amount);
            }
            PlayerInventory.ChangeGold(-BasketValue);
            BasketInventory.ContainedStacks.Clear();
            Close();
        }

    }


}