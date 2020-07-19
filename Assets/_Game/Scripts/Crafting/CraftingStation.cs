using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[System.Serializable]
public class StationItem

{
    [SerializeField]
    public ItemBase Item;
    [SerializeField]
    public int Amount;
}


public class CraftingStation : MonoBehaviour
{
    public string Name;
    public List<RecipeContainer> Queue = new List<RecipeContainer>();
    public List<RecipeContainer> StationRecipes;
    public bool AutomaticProgression = false;
    public SpriteRenderer RecipeSprite;

    public int RecipeIndex = 0;

    public int Slots;

    [SerializeField]
    public List<StationItem> ChosenItems = new List<StationItem>();

    public GameObject Canvas;

    public RecipeContainer CurrentRecipe;

    public float CurrentProgress;

    public float TargetProgress;

    public Transform SpawnPoint;

    public bool CanProgress = true;

    public List<RecipeContainer> RecipesInInput = new List<RecipeContainer>();

    public RecipeContainer ChosenRecipe;

    //When the player presses the interact button, if there is no recipe in queue, open the recipe UI. Otherwise, open the recipe change UI.


    void OnEnable()
    {
        if (AutomaticProgression == true)
        {
            TimeManager.OnMinuteChanged += MinuteChange;

        }

    }
    void OnDisable()
    {
        if (AutomaticProgression == true)
        {
            TimeManager.OnMinuteChanged -= MinuteChange;

        }
    }

    void MinuteChange()
    {
        if (AutomaticProgression == true && CurrentRecipe != null)
        {
            ProgressRecipe();
        }
    }

    public void Interact()
    {
        if (Queue.Count < 1)
        {
            FindObjectOfType<CraftingChoiceUI>().Open(this, Name);

        }
        else
        {
            FindObjectOfType<CraftingResetUI>().Open(this, "Reset Crafting Station?");
        }

    }

    //When player holds interact button, and queue has at least one recipe in it, progress recipe
    public void HoldUse()
    {
        if (Queue.Count > 0 && AutomaticProgression == false)
        {
            ProgressRecipe();
        }
    }

    public void Reset()
    {

        CurrentRecipe = null;
        Color newcolor = Color.white;
        newcolor.a = 0f;
        RecipeSprite.color = newcolor;
        ChosenRecipe = null;

        foreach (RecipeContainer recipe in Queue)
        {
            ReAddRecipeItems(recipe);
        }
        Queue.Clear();

        if (OnRecipeOver != null)
        {
            OnRecipeOver();
        }
    }

    public void Load()
    {
        if (CurrentRecipe != null)
        {
            RecipeSprite.sprite = CurrentRecipe.Recipe.Outputs[0].Item.item.itemIcon;
            Color newcolor = Color.white;
            newcolor.a = MapRangeExtension.MapRange(CurrentProgress, 0f, TargetProgress, 0.1f, 1f);
            RecipeSprite.color = newcolor;

        }
        if (GetComponent<CraftingStationContextualHUD>() != null)
        {
            GetComponent<CraftingStationContextualHUD>().Load();
        }
    }
    public void IncreaseMultiplier()
    {
        foreach (StationItem item in ChosenItems)
        {
            int amt = 1;
            foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
            {
                if (item.Item.itemID == ingredient.Item.item.itemID)
                {
                    amt = ingredient.Amount;
                }
            }

            if (CheckIncreaseIngredientAmount(item, amt) == false)
            {
                return;
            }

        }

        foreach (StationItem item in ChosenItems)
        {
            int amt = 1;
            foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
            {
                if (item.Item.itemID == ingredient.Item.item.itemID)
                {
                    amt = ingredient.Amount;
                }
            }
            for (int i = 0; i < amt; i++)
            {
                item.Amount += 1;
            }

        }
        CheckInput(true);
    }

    public void ManualIncreaseMultiplier()
    {
        int amt = ChosenRecipe.Amount + 1;
        PlayerInventory inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();

        foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
        {
            if (inventory.GetItemAmount(ingredient.Item.item) < ingredient.Amount * amt)
            {
                return;
            }
        }
        ChosenRecipe.Amount += 1;
    }

    public void ManualDecreaseMultiplier()
    {
        if (ChosenRecipe.Amount < 2)
        {
            return;
        }
        ChosenRecipe.Amount -= 1;
    }
    public void DecreaseMultiplier()
    {
        if (ChosenRecipe.Amount < 2)
        {
            return;
        }
        foreach (StationItem item in ChosenItems)
        {
            int amt = 1;
            foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
            {
                if (item.Item.itemID == ingredient.Item.item.itemID)
                {
                    amt = ingredient.Amount;
                }
            }

            if (CheckDecreaseIngredientAmount(item, amt) == false)
            {
                return;
            }
            CheckInput();

        }

        foreach (StationItem item in ChosenItems)
        {
            int amt = 1;
            foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
            {
                if (item.Item.itemID == ingredient.Item.item.itemID)
                {
                    amt = ingredient.Amount;
                }
            }
            for (int i = 0; i < amt; i++)
            {
                item.Amount -= 1;
                if (item.Amount < 1)
                {
                    RemoveItem(item.Item);
                }
            }

        }
        CheckInput(true);
    }


    //Check input items for recipe matches
    public void CheckInput(bool pKeepIndex = false)
    {

        List<RecipeContainer> recipeList = new List<RecipeContainer>();

        //Build a list of ingredients from the input's items
        List<CraftingIngredient> ingredientList = new List<CraftingIngredient>();

        foreach (StationItem stack in ChosenItems)
        {
            CraftingIngredient ingredient = new CraftingIngredient();
            ItemContainer container = new ItemContainer();
            container.item = ItemSystemUtility.GetItemCopy(stack.Item.itemID, stack.Item.itemType);
            ingredient.Item = container;
            ingredient.Amount = stack.Amount;
            if (ingredient.Item.item.itemID != 0)
            {
                ingredientList.Add(ingredient);
            }
        }

        //Check recipes against this station's contained recipes to find matches
        recipeList = CraftingManager.Instance.CheckRecipeMatches(ingredientList, StationRecipes);


        if (recipeList.Count < 1)
        {
            // print("nothing");
            RecipeIndex = 0;
            SetRecipe();
        }

        RecipesInInput = recipeList;
        if (RecipeIndex > RecipesInInput.Count - 1)
        {
            RecipeIndex = 0;
        }
        SetRecipe();

    }


    public void CycleRecipes(int pAmt)
    {
        if (RecipesInInput.Count < 1)
        {
            RecipeIndex = 0;
            return;
        }
        RecipeIndex = (RecipeIndex + pAmt) % (RecipesInInput.Count);
        SetRecipe();
    }




    public delegate void OutputChange(CraftingStation pStation);
    public event OutputChange OnOutputChanged;
    void SetRecipe()
    {
        if (RecipesInInput.Count < 1)
        {
            ChosenRecipe = null;
        }

        else if (RecipesInInput[RecipeIndex] != null)
        {
            ChosenRecipe = RecipesInInput[RecipeIndex];
        }
        else
        {
            ChosenRecipe = null;

        }
        if (OnOutputChanged != null)
        {
            OnOutputChanged(this);
        }
    }

    public void RecipeManualSelect(RecipeContainer pRecipe)
    {
        ChosenRecipe = pRecipe;
        if (OnOutputChanged != null)
        {
            OnOutputChanged(this);
        }
    }

    public void ClearItems()
    {
        ChosenItems.Clear();
    }

    public void AddRecipe()
    {
        PlayerInventory inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();

        if (ChosenRecipe == null)
        {
            // print("no recipes");

            return;
        }
        for (int i = 0; i < ChosenRecipe.Amount; i++)
        {
            Queue.Add(ChosenRecipe);
            foreach (CraftingIngredient ingredient in ChosenRecipe.Recipe.Ingredients)
            {
                inventory.RemoveItem(ingredient.Item.item, (uint)ingredient.Amount);
            }
        }
        ChosenItems.Clear();
        if (Queue.Count > 0)
        {
            StartRecipe();
        }
        ChosenRecipe = null;
    }

    public void ReAddRecipeItems(RecipeContainer pRecipe)
    {
        PlayerInventory inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();

        for (int i = 0; i < pRecipe.Amount; i++)
        {
            foreach (CraftingIngredient ingredient in pRecipe.Recipe.Ingredients)
            {
                inventory.Add(ingredient.Item.item, (uint)ingredient.Amount, true);
            }
        }

    }

    public delegate void RecipeChange();
    public event RecipeChange OnRecipeChanged;
    void StartRecipe()
    {
        CurrentRecipe = Queue[0];
        RecipeSprite.sprite = CurrentRecipe.Recipe.Outputs[0].Item.item.itemIcon;
        Color newcolor = Color.white;
        newcolor.a = 0.1f;
        RecipeSprite.color = newcolor;
        CurrentProgress = 0;
        TargetProgress = CurrentRecipe.Recipe.CraftingTime;
        if (OnRecipeChanged != null)
        {
            OnRecipeChanged();
        }
    }

    public delegate void RecipeProgress();
    public event RecipeProgress OnRecipeProgress;
    void ProgressRecipe()
    {
        if (CanProgress == false)
        {
            print("can't progress");
            return;
        }
        print("progress");
        if (AutomaticProgression)
        {
            CurrentProgress += (Time.deltaTime * CraftingManager.Instance.CraftingSpeed) * 60;

        }
        else
        {
            CurrentProgress += Time.deltaTime * CraftingManager.Instance.CraftingSpeed;

        }
        // print("progress: " + CurrentProgress);
        Color newcolor = Color.white;
        newcolor.a = MapRangeExtension.MapRange(CurrentProgress, 0f, TargetProgress, 0.1f, 1f);
        RecipeSprite.color = newcolor;
        if (OnRecipeProgress != null)
        {
            OnRecipeProgress();
        }
        if (CurrentProgress >= TargetProgress)
        {
            RecipeDone();
        }
    }

    public delegate void RecipeOver();
    public event RecipeOver OnRecipeOver;
    void RecipeDone()
    {
        foreach (ItemOutput output in CurrentRecipe.Recipe.Outputs)
        {
            ItemSpawner.Instance.SpawnItems(output.Item.item, SpawnPoint.transform.position, (uint)output.Amount);

        }
        Queue.Remove(CurrentRecipe);
        CurrentRecipe = null;
        Color newcolor = Color.white;
        newcolor.a = 0f;
        RecipeSprite.color = newcolor;

        if (Queue.Count > 0)
        {
            StartRecipe();
        }
        else
        {
            if (OnRecipeOver != null)
            {
                OnRecipeOver();
            }
        }
    }


    void IngredientsChanged()
    {
        CheckInput();
        SetRecipe();
    }

    public bool HasItem(ItemBase pItem)
    {
        foreach (StationItem item in ChosenItems)
        {
            if (pItem.itemID == item.Item.itemID)
            {
                return true;
            }
        }
        return false;
    }

    public bool AddItem(ItemBase pItem)
    {
        if (HasItem(pItem))
        {
            return false;
        }
        if (ChosenItems.Count >= Slots)
        {
            return false;
        }
        StationItem newItem = new StationItem();
        newItem.Item = pItem;
        newItem.Amount = 1;
        ChosenItems.Add(newItem);
        IngredientsChanged();
        return true;
    }


    public bool RemoveItem(ItemBase pItem)
    {
        foreach (StationItem item in ChosenItems)
        {
            if (pItem.itemID == item.Item.itemID)
            {
                ChosenItems.Remove(item);
                IngredientsChanged();
                return true;
            }
        }
        return false;
    }



    public bool IncreaseIngredientAmount(StationItem pItem)
    {
        if (pItem == null)
        {
            return false;
        }
        InventoryItemStack stack = GameManager.Instance.Player.GetComponent<PlayerInventory>().FindItemStack(pItem.Item);
        if (stack == null)
        {
            return false;
        }
        if (stack.Amount >= pItem.Amount + 1)
        {
            pItem.Amount += 1;
            IngredientsChanged();
            return true;
        }
        return false;
    }


    public bool CheckIncreaseIngredientAmount(StationItem pItem, int pAmount)
    {
        if (pItem == null)
        {
            return false;
        }
        InventoryItemStack stack = GameManager.Instance.Player.GetComponent<PlayerInventory>().FindItemStack(pItem.Item);
        if (stack == null)
        {
            return false;
        }
        if (stack.Amount >= pItem.Amount + pAmount)
        {
            return true;
        }
        return false;
    }

    public bool DecreaseIngredientAmount(StationItem pItem)
    {
        if (pItem == null)
        {
            return false;
        }
        if (pItem.Amount > 0)
        {
            pItem.Amount -= 1;

            if (pItem.Amount <= 0)
            {
                RemoveItem(pItem.Item);
            }
            IngredientsChanged();

            return true;
        }
        return false;
    }

    public bool CheckDecreaseIngredientAmount(StationItem pItem, int pAmount)
    {
        if (pItem == null)
        {
            return false;
        }
        if (pItem.Amount > pAmount)
        {
            return true;
        }
        return false;
    }
    public StationItem GetItem(ItemBase pItem)
    {
        foreach (StationItem item in ChosenItems)
        {
            if (item.Item.itemID == pItem.itemID)
            {
                return item;
            }
        }
        return new StationItem();
    }


    //Check if inventory items have changed since last open
    public void AdjustAmounts()
    {
        foreach (StationItem item in ChosenItems)
        {
            StorageObject Inventory = GameManager.Instance.Player.GetComponent<PlayerInventory>();
            InventoryItemStack stack = Inventory.FindItemStack(item.Item);
            int itemAmount = Inventory.GetItemAmount(item.Item);

            if (itemAmount < 1)
            {
                RemoveItem(item.Item);
                return;
            }
            if (item.Amount > itemAmount)
            {
                int amt = item.Amount - itemAmount;
                amt = Mathf.Abs(amt);
                for (int i = 0; i < amt; i++)
                {
                    DecreaseIngredientAmount(item);
                }
            }
        }
    }



}
