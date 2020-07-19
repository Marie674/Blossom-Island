using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ItemSystem;
using System;
public class CraftingRecipesUI : MonoBehaviour
{
    public TMP_Dropdown StationDropDown;
    public TMP_Dropdown ItemTypeDropDown;
    CraftingStation CurrentStation;

    CraftingRecipe CurrentRecipe;
    ItemSystem.ItemType CurrentType;

    List<CraftingStation> ValidStations = new List<CraftingStation>();

    List<RecipeContainer> ValidRecipes = new List<RecipeContainer>();

    List<ItemType> ValidTypes = new List<ItemType>();

    public Transform RecipeContainer;
    public RecipeNamePrefab RecipeUIPrefab;


    public Image RecipeIcon;
    public TextMeshProUGUI RecipeAmountText;
    public TextMeshProUGUI RecipeNameText;

    public Transform ByProductsContainer;
    public CraftingByProductUI ByProductUIPrefab;

    public CraftingIngredientUI IngredientUIPrefab;
    public Transform IngredientContainer;

    public void Open()
    {

        List<CraftingStation> ValidStations = new List<CraftingStation>();

        List<RecipeContainer> ValidRecipes = new List<RecipeContainer>();
        FilterStations();
        DrawStationsDropDown();
    }

    public void Close()
    {
        GetComponent<WindowToggle>().Close();
    }
    void FilterStations()
    {
        ValidStations.Clear();
        foreach (CraftingStation station in CraftingManager.Instance.CraftingStations)
        {
            foreach (RecipeContainer recipe in station.StationRecipes)
            {
                if (CraftingManager.Instance.KnownRecipes.Contains(recipe))
                {
                    if (!ValidStations.Contains(station))
                    {
                        ValidStations.Add(station);
                    }
                }
            }
        }
    }

    void FilterTypes()
    {
        ValidTypes.Clear();
        if (CurrentStation == null)
        {
            return;
        }

        foreach (RecipeContainer recipe in CurrentStation.StationRecipes)
        {
            if (CraftingManager.Instance.KnownRecipes.Contains(recipe))
            {
                ItemType type = recipe.Recipe.Outputs[0].Item.item.itemType;
                if (!ValidTypes.Contains(type))
                {
                    ValidTypes.Add(type);
                }
            }

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
            if (CraftingManager.Instance.KnownRecipes.Contains(recipe))
            {
                ItemType type = recipe.Recipe.Outputs[0].Item.item.itemType;
                if (CurrentType == type)
                {
                    if (!ValidRecipes.Contains(recipe))
                    {
                        ValidRecipes.Add(recipe);
                    }
                }
            }

        }
    }

    void DrawStationsDropDown()
    {
        StationDropDown.onValueChanged.RemoveAllListeners();

        StationDropDown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (CraftingStation station in ValidStations)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = station.Name.ToString();
            options.Add(data);

        }
        StationDropDown.AddOptions(options);
        StationDropDown.onValueChanged.AddListener(delegate
        {
            SelectStation();

        });
        if (StationDropDown.options.Count > 0)
        {
            SelectStation();

        }
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
        foreach (ItemType type in ValidTypes)
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
            newRecipe.NameText.text = recipe.Recipe.Outputs[0].Item.item.itemName;
            newRecipe.Button.onClick.AddListener(delegate ()
            {
                SelectRecipe(recipe);
            });
        }
        SelectRecipe(ValidRecipes[0]);

    }

    void SelectRecipe(RecipeContainer pRecipe)
    {
        CurrentRecipe = pRecipe.Recipe;
        DrawRecipe();

    }


    void ClearByProducts()
    {
        CraftingByProductUI[] drawnProducts = ByProductsContainer.GetComponentsInChildren<CraftingByProductUI>();
        foreach (CraftingByProductUI byProductUI in drawnProducts)
        {
            Destroy(byProductUI.gameObject);
        }
    }

    void ClearIngredients()
    {
        CraftingIngredientUI[] drawnIngredients = IngredientContainer.GetComponentsInChildren<CraftingIngredientUI>();
        foreach (CraftingIngredientUI ingredientUI in drawnIngredients)
        {
            Destroy(ingredientUI.gameObject);
        }
    }

    void DrawRecipe()
    {
        ClearByProducts();
        ClearIngredients();
        if (CurrentRecipe == null)
        {
            return;
        }
        RecipeIcon.sprite = CurrentRecipe.Outputs[0].Item.item.itemIcon;
        RecipeAmountText.text = "x" + CurrentRecipe.Outputs[0].Amount.ToString();
        RecipeNameText.text = CurrentRecipe.Outputs[0].Item.item.itemName;

        PlayerInventory playerInventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();

        List<ItemOutput> byProducts = new List<ItemOutput>();
        foreach (ItemOutput output in CurrentRecipe.Outputs)
        {
            if (output.ByProduct == true)
            {
                byProducts.Add(output);
                CraftingByProductUI productUI = Instantiate(ByProductUIPrefab, ByProductsContainer);
                productUI.ItemAmountText.text = "x" + output.Amount.ToString();
                productUI.ItemIcon.sprite = output.Item.item.itemIcon;
            }
        }

        foreach (CraftingIngredient ingredient in CurrentRecipe.Ingredients)
        {
            CraftingIngredientUI ingredientUI = Instantiate(IngredientUIPrefab, IngredientContainer);
            ingredientUI.ItemIcon.sprite = ingredient.Item.item.itemIcon;
            ingredientUI.ItemNameText.text = ingredient.Item.item.itemName;
            int playerAmt = playerInventory.GetItemAmount(ingredient.Item.item);
            string amt = playerAmt.ToString() + "/" + ingredient.Amount.ToString();
            if (playerAmt >= ingredient.Amount)
            {
                ingredientUI.ItemAmountText.color = new Color(0, 0.5f, 0, 1);
                ingredientUI.FrameImage.color = new Color(0.75f, 1, 0.75f);
            }
            else
            {
                ingredientUI.ItemAmountText.color = Color.red;
                ingredientUI.FrameImage.color = Color.white;
            }

            ingredientUI.ItemAmountText.text = amt;
            if (ingredient.Used == true)
            {
                ingredientUI.ConsumedCheck.enabled = true;
            }
            else
            {
                ingredientUI.ConsumedCheck.enabled = false;

            }
        }

    }


    void SelectStation()
    {

        string val = StationDropDown.options[StationDropDown.value].text;
        //print(val);
        CraftingStation station = ValidStations.Find(x => x.Name == val);
        if (station != null)
        {
            CurrentStation = station;
            FilterTypes();
            DrawItemTypeDropDown();
        }
    }

    void SelectItemType()
    {

        string val = ItemTypeDropDown.options[ItemTypeDropDown.value].text;

        ItemType type = (ItemType)Enum.Parse(typeof(ItemType), val);

        bool foundType = false;
        foreach (ItemType validType in ValidTypes)
        {
            if (type == validType)
            {
                foundType = true;
            }
        }
        ItemType newType = ValidTypes.Find(x => x == type);
        if (foundType)
        {
            CurrentType = newType;
            FilterRecipes();
            DrawRecipes();
        }
    }



}
