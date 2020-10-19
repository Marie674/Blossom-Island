using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Items;
using System;
public class CraftingStationRecipesUI : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    CraftingStation CurrentStation;
    ItemSystem.ItemTypes CurrentType;
    List<RecipeContainer> ValidRecipes = new List<RecipeContainer>();
    List<ItemSystem.ItemTypes> ValidTypes = new List<ItemSystem.ItemTypes>();

    public RecipeNamePrefab RecipeUIPrefab;
    public Transform RecipeContainer;
    public TMP_Dropdown ItemTypeDropDown;

    PlayerInventory Inventory;

    public Image ItemIcon;
    public TextMeshProUGUI ItemAmountText;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI CraftingAmountText;

    RecipeContainer CurrentRecipe;

    public GameObject CraftingButtons;

    public void Open(CraftingStation pStation)
    {
        Inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();
        CurrentStation = pStation;
        ValidTypes.Clear();
        foreach (RecipeContainer recipe in CurrentStation.StationRecipes)
        {
            if (CanCraft(recipe))
            {
                ItemSystem.ItemTypes type = recipe.Recipe.Outputs[0].ContainedItem.Type;
                if (!ValidTypes.Contains(type))
                {
                    ValidTypes.Add(type);
                }
            }
        }
        DrawItemTypeDropDown();
        GetComponent<WindowToggle>().Open();

    }

    public void Close()
    {
        GetComponent<WindowToggle>().Close();
    }


    public void DecreaseMultiplier()
    {
        CurrentStation.ManualDecreaseMultiplier();
        DrawRecipe();
    }
    public void IncreaseMultiplier()
    {
        CurrentStation.ManualIncreaseMultiplier();
        DrawRecipe();
    }

    void DrawRecipe()
    {
        ItemIcon.color = Color.white;
        ItemOutput output = CurrentRecipe.Recipe.Outputs[0];
        ItemIcon.sprite = output.ContainedItem.Icon;
        ItemAmountText.text = "x" + (output.Amount * CurrentRecipe.Amount).ToString();
        ItemNameText.text = output.ContainedItem.Name;
        CraftingAmountText.text = "x" + CurrentRecipe.Amount.ToString();
    }
    public void Confirm()
    {

        CurrentStation.AddRecipe();
        Close();
    }


    void DrawItemTypeDropDown()
    {
        ItemTypeDropDown.onValueChanged.RemoveAllListeners();
        ItemTypeDropDown.ClearOptions();
        if (CurrentStation == null)
        {
            return;
        }
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (ItemSystem.ItemTypes type in ValidTypes)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = type.ToString();
            options.Add(data);

        }
        ItemTypeDropDown.AddOptions(options);
        ItemTypeDropDown.onValueChanged.AddListener(delegate
        {
            SelectItemType();

        });
        if (ItemTypeDropDown.options.Count > 0)
        {
            SelectItemType();

        }
        else
        {
            DrawBlank();
        }

    }

    void DrawBlank()
    {
        ItemIcon.color = new Color(0, 0, 0, 0);
        ItemAmountText.text = string.Empty;
        ItemNameText.text = "No Craftable Items";
        CraftingButtons.SetActive(false);
    }

    void SelectItemType()
    {
        string val = ItemTypeDropDown.options[ItemTypeDropDown.value].text;

        ItemSystem.ItemTypes type = (ItemSystem.ItemTypes)Enum.Parse(typeof(ItemSystem.ItemTypes), val);

        bool foundType = false;

        foreach (ItemSystem.ItemTypes validType in ValidTypes)
        {
            if (type == validType)
            {
                foundType = true;
            }
        }
        ItemSystem.ItemTypes newType = ValidTypes.Find(x => x == type);
        if (foundType)
        {
            CraftingButtons.SetActive(true);

            CurrentType = newType;
            FilterRecipes();
            DrawRecipes();
        }
    }

    void FilterRecipes()
    {
        ValidRecipes.Clear();
        if (CurrentStation == null)
        {
            return;
        }

        foreach (RecipeContainer recipe in CurrentStation.StationRecipes)
        {
            if (CanCraft(recipe))
            {
                ItemSystem.ItemTypes type = recipe.Recipe.Outputs[0].ContainedItem.Type;
                if (CurrentType == type)
                {
                    ValidRecipes.Add(recipe);
                }
            }
        }
    }
    void ClearRecipes()
    {
        RecipeNamePrefab[] drawnRecipes = RecipeContainer.GetComponentsInChildren<RecipeNamePrefab>();
        foreach (RecipeNamePrefab recipeName in drawnRecipes)
        {
            Destroy(recipeName.gameObject);
        }
    }
    void DrawRecipes()
    {
        ClearRecipes();
        foreach (RecipeContainer recipe in ValidRecipes)
        {
            RecipeNamePrefab newRecipe = Instantiate(RecipeUIPrefab, RecipeContainer);
            newRecipe.NameText.text = recipe.Recipe.Outputs[0].ContainedItem.Name;
            newRecipe.Button.onClick.AddListener(delegate ()
            {
                SelectRecipe(recipe);
            });
        }
        SelectRecipe(ValidRecipes[0]);

    }

    void SelectRecipe(RecipeContainer pRecipe)
    {
        CurrentRecipe = pRecipe;
        CurrentRecipe.Amount = 1;
        CurrentStation.RecipeManualSelect(CurrentRecipe);
        DrawRecipe();
    }


    public bool CanCraft(RecipeContainer pRecipe)
    {
        if (Inventory == null)
        {
            return false;
        }
        foreach (CraftingIngredient ingredient in pRecipe.Recipe.Ingredients)
        {
            if (Inventory.GetItemAmount(ingredient.ContainedItem) < ingredient.Amount)
            {
                return false;
            }
        }
        return true;
    }
}
