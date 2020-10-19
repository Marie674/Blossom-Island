using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using UnityEngine.UI;
using TMPro;
public class CraftingInputUI : MonoBehaviour
{

    public InventoryUI InventoryUI;
    CraftingStation Station;
    public bool IsOpen = false;
    WindowToggle Window;

    List<CraftingItemUI> DrawnItems = new List<CraftingItemUI>();
    public CraftingItemUI ItemUIPrefab;
    public Transform IngredientContainer;

    public TextMeshProUGUI Title;

    public CraftingItemUI OutputUI;

    public Button ConfirmBtn;
    int RecipeAmount = 1;

    public TextMeshProUGUI AmountText;
    public Button CycleLeftButton;
    public Button CycleRightButton;


    void Start()
    {
        Window = GetComponent<WindowToggle>();
    }

    protected void OnDisable()
    {
        if (Station != null)
        {
            Station.OnOutputChanged -= Draw;
            Station = null;
        }


    }

    public void Open(CraftingStation pStation)
    {
        InventoryUI.CurrentStorage = GameManager.Instance.Player.GetComponent<PlayerInventory>();
        IsOpen = true;
        Station = pStation;
        Station.OnOutputChanged += Draw;

        Draw(Station);

        Window.Open();
    }

    public void Draw(CraftingStation pStation)
    {
        Station.AdjustAmounts();
        Title.text = pStation.name;
        ConfirmBtn.interactable = false;
        List<RecipeContainer> recipes = pStation.RecipesInInput;

        if (recipes != null && recipes.Count > 1)
        {
            CycleLeftButton.gameObject.SetActive(true);
            CycleRightButton.gameObject.SetActive(true);
        }
        else
        {
            CycleLeftButton.gameObject.SetActive(false);
            CycleRightButton.gameObject.SetActive(false);
        }

        DrawIngredients();
        DrawOutput(Station.ChosenRecipe);
        DrawAmount(Station.ChosenRecipe);
    }

    public void AddItem(InventoryItemStack pItem, InventoryUI pFrom)
    {
        if (Station != null)
        {
            Station.AddItem(pItem.ContainedItem);
        }
    }


    void DrawIngredients()
    {
        ClearIngredients();
        int i = 0;
        int stackAmt = Station.Slots;

        foreach (StationItem item in Station.ChosenItems)
        {
            DrawIngredient(item);
            i++;
        }
        stackAmt -= i;
        for (int j = 0; j < stackAmt; j++)
        {
            DrawIngredient(null);
        }
    }

    void DrawIngredient(StationItem pItem)
    {
        CraftingItemUI itemUI = Instantiate(ItemUIPrefab, IngredientContainer);

        itemUI.Button.onClick.AddListener(delegate
        {
            Station.RemoveItem(pItem.ContainedItem);
        });

        itemUI.IncreaseButton.onClick.AddListener(delegate
        {
            Station.IncreaseIngredientAmount(pItem);
        });

        itemUI.DecreaseButton.onClick.AddListener(delegate
        {
            Station.DecreaseIngredientAmount(pItem);
        });

        if (itemUI.Button != null)
        {
            itemUI.Button.interactable = false;
        }

        if (pItem != null)
        {
            if (itemUI.Button != null)
            {
                itemUI.Button.interactable = true;
            }

            itemUI.ItemIcon.color = Color.white;
            itemUI.ItemIcon.sprite = pItem.ContainedItem.Icon;

            itemUI.ItemAmount.text = "x" + pItem.Amount.ToString();


            if (itemUI.ItemNameText != null)
            {
                itemUI.ItemNameText.text = pItem.ContainedItem.Name;
            }
        }
        else
        {
            if (itemUI.ItemNameText != null)
            {
                itemUI.ItemNameText.text = "No Recipe Selected";

            }
            itemUI.ItemIcon.color = new Color(0, 0, 0, 0);
            itemUI.ItemAmount.text = "";
        }
        DrawnItems.Add(itemUI);
    }

    void DrawOutput(RecipeContainer pOutput)
    {

        if (OutputUI.Button != null)
        {
            OutputUI.Button.interactable = false;
        }

        if (pOutput != null)
        {
            int amount = pOutput.Recipe.Outputs[0].Amount * pOutput.Amount;


            OutputUI.ItemIcon.color = Color.white;
            OutputUI.ItemIcon.sprite = pOutput.Recipe.Outputs[0].ContainedItem.Icon;

            OutputUI.ItemAmount.text = "x" + amount;
            if (OutputUI.ItemNameText != null)
            {
                OutputUI.ItemNameText.text = pOutput.Recipe.Outputs[0].ContainedItem.Name;
            }
        }
        else
        {
            if (OutputUI.ItemNameText != null)
            {
                OutputUI.ItemNameText.text = "No Recipe Selected";

            }
            OutputUI.ItemIcon.color = new Color(0, 0, 0, 0);
            OutputUI.ItemAmount.text = "";
        }

        if (pOutput == null)
        {
            ConfirmBtn.interactable = false;
        }
        else
        {
            ConfirmBtn.interactable = true;
        }
    }

    void DrawAmount(RecipeContainer pOutput)
    {
        if (pOutput != null)
        {
            AmountText.text = "x" + pOutput.Amount.ToString();
        }
        else
        {
            AmountText.text = string.Empty;
        }
    }
    public void Close()
    {
        IsOpen = false;
        Window.Close();
    }


    //cycle through recipes if the input has several outputs
    public void CycleRecipes(int pAmt)
    {
        if (Station == null)
        {
            return;
        }
        Station.CycleRecipes(pAmt);
    }

    public delegate void ConfirmRecipe();
    public event ConfirmRecipe OnConfirmRecipe;
    public void Confirm()
    {

        Station.AddRecipe();
        Close();
        if (OnConfirmRecipe != null)
        {
            OnConfirmRecipe();
        }
    }


    public void ClearIngredients()
    {
        foreach (CraftingItemUI stack in DrawnItems)
        {
            Destroy(stack.gameObject);
        }
        DrawnItems.Clear();
    }
    public void DecreaseMultiplier()
    {
        Station.DecreaseMultiplier();
    }
    public void IncreaseMultiplier()
    {
        Station.IncreaseMultiplier();
    }

}
