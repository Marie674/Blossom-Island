using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class CraftingManagerSaver : MonoBehaviour
{

    public void OnEnable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        PersistentDataManager.UnregisterPersistentData(this.gameObject);
    }

    public void OnRecordPersistentData()
    {
        DialogueLua.SetVariable("CraftingManager RecipeAmount", CraftingManager.Instance.KnownRecipes.Count);
        for (int i = 0; i < CraftingManager.Instance.KnownRecipes.Count; i++)
        {
            DialogueLua.SetVariable("CraftingManager Recipe" + i, CraftingManager.Instance.KnownRecipes[i].UniqueName);
        }
    }
    public void OnApplyPersistentData()
    {
        int amt = DialogueLua.GetVariable("CraftingManager RecipeAmount").asInt;
        for (int i = 0; i < amt; i++)
        {
            string recipeName = DialogueLua.GetVariable("CraftingManager Recipe" + i).asString;
            CraftingManager.Instance.TeachRecipe(recipeName);
        }
    }
}