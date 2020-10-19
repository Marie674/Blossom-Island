using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CraftingStationContextualHUD : ContextualHUD
{
    public TextMeshProUGUI PromptText;
    public Slider ProgressBar;
    public Image ProgressHandle;
    public CraftingStation TargetStation;
    void Start()
    {
        TargetStation = GetComponent<CraftingStation>();
    }

    void OnEnable()
    {
        TargetStation = GetComponent<CraftingStation>();

        if (TargetStation != null)
        {
            TargetStation.OnRecipeChanged += RecipeChange;
            TargetStation.OnRecipeOver += RecipeDone;
            TargetStation.OnRecipeProgress += RecipeProgress;

        }
    }

    void OnDisable()
    {
        TargetStation = GetComponent<CraftingStation>();

        if (TargetStation != null)
        {
            TargetStation.OnRecipeChanged -= RecipeChange;
            TargetStation.OnRecipeOver -= RecipeDone;
            TargetStation.OnRecipeProgress -= RecipeProgress;

        }
    }

    public void Load()
    {
        TargetStation = GetComponent<CraftingStation>();
        if (TargetStation.CurrentRecipe != null)
        {
            RecipeChange();
        }
        else
        {
            RecipeDone();
        }

    }
    public void RecipeChange()
    {
        if (TargetStation.CurrentRecipe == null)
        {
            return;
        }
        CraftingRecipe recipe = TargetStation.CurrentRecipe.Recipe;
        ProgressHandle.sprite = recipe.Outputs[0].ContainedItem.Icon;
        RecipeProgress();
        ProgressBar.gameObject.SetActive(true);
        PromptText.text = "Craft: Hold E - Reset: Press E";
    }

    public void RecipeDone()
    {
        ProgressBar.gameObject.SetActive(false);
        PromptText.text = "Press E";
    }
    public void RecipeProgress()
    {
        ProgressBar.value = MapRangeExtension.MapRange(TargetStation.CurrentProgress, 0, TargetStation.TargetProgress, 0, 1);
    }


}
