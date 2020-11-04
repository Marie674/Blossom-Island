using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;
using TMPro;


public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public TextMeshProUGUI TabDescription;

    public Transform ItemUIContainer;

    public ItemStackUI ItemUIPrefab;

    public WindowToggle Window;

    public InventoryTabUI SelectedTab;
    protected InventoryTabUI[] Tabs;
    protected int SelectedTabId = 0;
    public GameObject TabContainer;

    public InventoryItemStack SelectedStack;
    public ItemBase SelectedItem = null;
    public StorageObject CurrentStorage;

    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemDescriptionText;

    public TextMeshProUGUI ItemValueText;

    public TextMeshProUGUI ItemWeightText;

    public TextMeshProUGUI InventoryWeightText;
    public bool IsOpen = false;

    public Sprite TabSprite;
    public Sprite SelectedTabSprite;

    StorageTransferUI TransferUI;
    CraftingInputUI CraftingInputUI;

    List<InventoryItemStack> TabItems = new List<InventoryItemStack>();

    public List<ItemStackUI> DrawnItems = new List<ItemStackUI>();

    public List<InventoryItemStack> StorageStacks;

    public ScrollRect Scroll;

    protected virtual void Awake()
    {
        TransferUI = GameObject.FindObjectOfType<StorageTransferUI>();
        CraftingInputUI = GameObject.FindObjectOfType<CraftingInputUI>();
        Tabs = TabContainer.GetComponentsInChildren<InventoryTabUI>();
        SelectedTab = Tabs[0];
    }

    // void OnEnable()
    // {
    //     WindowToggle window = GetComponent<WindowToggle>();
    //     if (window != null)
    //     {
    //         window.Window.onOpen.AddListener(delegate ()
    //         {
    //             Open();
    //         });
    //     }
    // }

    // void OnDisable()
    // {
    //     WindowToggle window = GetComponent<WindowToggle>();
    //     if (window != null)
    //     {
    //         window.Window.onOpen.RemoveAllListeners();
    //     }
    // }


    public void Open()
    {
        //        print(CurrentStorage.gameObject.name);
        TransferUI = GameObject.FindObjectOfType<StorageTransferUI>();
        CraftingInputUI = GameObject.FindObjectOfType<CraftingInputUI>();

        Tabs = TabContainer.GetComponentsInChildren<InventoryTabUI>();
        SelectedTab = Tabs[0];
        ChangeTabs(SelectedTab);
        CurrentStorage.OnItemChanged += Draw;
        IsOpen = true;
        foreach (InventoryTabUI tab in Tabs)
        {
            tab.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ChangeTabs(tab);
            });
        }

    }

    public void Close()
    {


        SelectedItem = null;
        Clear();
        if (CurrentStorage != null)
        {
            CurrentStorage.OnItemChanged -= Draw;
        }
        if (Tabs != null)
        {
            foreach (InventoryTabUI tab in Tabs)
            {
                tab.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        IsOpen = false;
    }

    public void ThrowItems(int pAmount = 1)
    {
        for (int i = 0; i < pAmount; i++)
        {
            CurrentStorage.RemoveFromStack(SelectedStack, 1);
        }
    }

    public void DropItems(int pAmount = 1)
    {
        InventoryItemStack stack = SelectedStack;
        CurrentStorage.DropItem(stack, (uint)pAmount);
    }


    public void Clear()
    {
        foreach (ItemStackUI stack in DrawnItems)
        {
            stack.Button.onClick.RemoveAllListeners();
            Destroy(stack.gameObject);
        }

        DrawnItems.Clear();
    }


    void GetTabItems()
    {
        TabItems.Clear();

        foreach (InventoryItemStack itemStack in StorageStacks)
        {

            if (SelectedTab.Types.Count > 0)
            {
                foreach (ItemSystem.ItemTypes type in SelectedTab.Types)
                {
                    if (type.ToString() == itemStack.ContainedItem.Type.ToString())
                    {
                        TabItems.Add(itemStack);
                    }

                }
            }
            else
            {
                TabItems.Add(itemStack);
            }
        }
    }

    void DrawItem(InventoryItemStack pStack)
    {



        if (pStack != null)
        {
            ItemUIPrefab.GetComponent<Button>().interactable = true;
            ItemUIPrefab.ItemIcon.color = Color.white;

            ItemUIPrefab.ItemIcon.sprite = pStack.ContainedItem.Icon;
            ItemUIPrefab.ItemAmount.text = pStack.Amount.ToString();
        }
        else
        {
            ItemUIPrefab.GetComponent<Button>().interactable = false;
            ItemUIPrefab.ItemIcon.color = new Color(0, 0, 0, 0);
            ItemUIPrefab.ItemAmount.text = "";
        }

        ItemStackUI itemUI = Instantiate(ItemUIPrefab, ItemUIContainer);
        itemUI.Button = itemUI.GetComponent<Button>();
        itemUI.ItemStack = pStack;

        DrawnItems.Add(itemUI);
        itemUI.Button.onClick.AddListener(delegate
        {
            SetSelectedItem(pStack);
        });


        if (TransferUI != null && TransferUI.IsOpen)
        {
            itemUI.Button.onClick.AddListener(delegate
          {
              TransferUI.TransferItem(pStack, this);
          });
        }

        else if (CraftingInputUI != null && CraftingInputUI.IsOpen)
        {
            itemUI.Button.onClick.AddListener(delegate
         {
             CraftingInputUI.AddItem(pStack, this);
         });

        }

    }
    void DrawTab()
    {

        foreach (InventoryItemStack itemStack in TabItems)
        {
            DrawItem(itemStack);
        }
    }

    void DrawEmptyStacks()
    {
        int amt = CurrentStorage.MaxStacks - CurrentStorage.ContainedStacks.Count;
        for (int i = 0; i < amt; i++)
        {
            DrawItem(null);
        }
    }

    protected void Draw()
    {
        if (CurrentStorage != null)
        {
            StorageStacks = CurrentStorage.ContainedStacks;
        }
        foreach (InventoryTabUI tab in Tabs)
        {
            tab.TitleText = tab.GetComponentInChildren<TextMeshProUGUI>();
            tab.TitleText.text = tab.Title;

        }
        Clear();
        GetTabItems();
        DrawTab();
        DrawEmptyStacks();
        Reselect();

        if (DrawnItems.Count > 0)
        {
            TabDescription.text = string.Empty;
        }

        else
        {
            TabDescription.text = "No items in this category";
        }

        if (InventoryWeightText != null)
        {
            InventoryWeightText.text = CurrentStorage.CurrentWeight.ToString("F2") + "/" + CurrentStorage.MaxWeight.ToString("F2");
        }




    }

    public void ChangeTabs(InventoryTabUI Tab)
    {

        SelectedTab = Tab;

        for (int i = 0; i < Tabs.Length; i++)
        {
            if (Tabs[i] == SelectedTab)
            {
                SelectedTabId = i;
                SelectedTab.GetComponent<Image>().sprite = SelectedTabSprite;
            }
            else
            {
                Tabs[i].GetComponent<Image>().sprite = TabSprite;
            }
        }

        if (TabItems.Count > 0)
        {
            SetSelectedItem(TabItems[0]);
        }
        else
        {
            SetSelectedItem(null);
        }
        Draw();
    }
    protected void Reselect()
    {
        //check if stack still exists, set it to that one if so.
        foreach (InventoryItemStack stack in TabItems)
        {
            if (stack.ContainedItem.ID == SelectedItem.ID)
            {
                SetSelectedItem(stack);
                return;
            }
        }

        //if stack does not exist anymore, select first stack if it exists
        if (TabItems.Count > 0)
        {
            SetSelectedItem(TabItems[0]);
        }
        else
        {
            SetSelectedItem(null);
        }

    }

    public void SetSelectedItem(InventoryItemStack pStack)
    {
        if (pStack != null)
        {
            SelectedItem = pStack.ContainedItem;
        }
        else
        {
            SelectedItem = null;
        }

        if (SelectedItem == null)
        {
            SelectedItem = new ItemBase();
            SelectedStack = null;
        }

        if (SelectedItem.ID != -1)
        {
            foreach (ItemStackUI itemUI in DrawnItems)
            {
                if (itemUI.ItemStack == null || itemUI.ItemStack.ContainedItem.ID != SelectedItem.ID)
                {
                    itemUI.FrameImage.enabled = false;
                }
                else
                {
                    SelectedStack = itemUI.ItemStack;
                    itemUI.GetComponent<Button>().Select();
                    itemUI.FrameImage.enabled = true;
                }
            }
        }


        DrawSelectedItemInfo();
    }

    private void DrawSelectedItemInfo()
    {
        if (SelectedItem == null || SelectedItem.ID == -1 || SelectedStack == null)
        {
            if (ItemNameText != null)
            {
                ItemNameText.text = string.Empty;
            }
            if (ItemDescriptionText != null)
            {
                ItemDescriptionText.text = string.Empty;
            }
            if (ItemWeightText != null)
            {
                ItemWeightText.text = string.Empty;
            }
            if (ItemValueText != null)
            {
                ItemValueText.text = string.Empty;
            }
        }
        else
        {
            if (ItemNameText != null)
            {
                ItemNameText.text = SelectedItem.Name;
            }
            if (ItemDescriptionText != null)
            {
                ItemDescriptionText.text = SelectedItem.Description;
            }
            if (ItemWeightText != null)
            {
                ItemWeightText.text = SelectedItem.Weight + " (" + (SelectedItem.Weight * SelectedStack.Amount) + ")";
            }
            if (ItemValueText != null)
            {
                ItemValueText.text = SelectedItem.Value + " (" + (SelectedItem.Value * SelectedStack.Amount) + ")";
            }

        }
    }


}
