using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Game.Items;
public class CraftingStationSaver : MonoBehaviour
{
    CraftingStation TargetStation;
    string VariableName;

    public void OnEnable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        OnRecordPersistentData();

        PersistentDataManager.UnregisterPersistentData(this.gameObject);
    }

    public void OnRecordPersistentData()
    {
        TargetStation = GetComponent<CraftingStation>();

        VariableName = TargetStation.Name + transform.position.x + transform.position.y;

        DialogueLua.SetVariable(VariableName + "CurrentProgress", TargetStation.CurrentProgress);
        DialogueLua.SetVariable(VariableName + "TargetProgress", TargetStation.TargetProgress);

        DialogueLua.SetVariable(VariableName + "RecipeIndex", TargetStation.RecipeIndex);

        DialogueLua.SetVariable(VariableName + "QueueAmount", TargetStation.Queue.Count);

        for (int i = 0; i < TargetStation.Queue.Count; i++)
        {
            DialogueLua.SetVariable(VariableName + "Queue" + i, TargetStation.Queue[i].UniqueName);

        }
        DialogueLua.SetVariable(VariableName + "ChosenItems", TargetStation.ChosenItems.Count);

        for (int i = 0; i < TargetStation.ChosenItems.Count; i++)
        {
            DialogueLua.SetVariable(VariableName + "ChosenID" + i, TargetStation.ChosenItems[i].ContainedItem.ID);
            DialogueLua.SetVariable(VariableName + "ChosenType", TargetStation.ChosenItems[i].ContainedItem.Type.ToString());
            DialogueLua.SetVariable(VariableName + "ChosenAmount" + i, TargetStation.ChosenItems[i].Amount);

        }

        if (TargetStation.CurrentRecipe != null)
        {
            DialogueLua.SetVariable(VariableName + "CurrentRecipe", TargetStation.CurrentRecipe.UniqueName);

        }
        else
        {
            DialogueLua.SetVariable(VariableName + "CurrentRecipe", "Null");
        }

        DialogueLua.SetVariable(VariableName + "CanProgress", TargetStation.CanProgress);


        DialogueLua.SetVariable(VariableName + "RecipesInInput", TargetStation.RecipesInInput.Count);

        for (int i = 0; i < TargetStation.RecipesInInput.Count; i++)
        {
            DialogueLua.SetVariable(VariableName + "InputName" + i, TargetStation.RecipesInInput[i].UniqueName);

        }

        if (TargetStation.ChosenRecipe != null)
        {
            DialogueLua.SetVariable(VariableName + "ChosenRecipe", TargetStation.ChosenRecipe.UniqueName);

        }
        else
        {
            DialogueLua.SetVariable(VariableName + "ChosenRecipe", "Null");

        }

    }
    public void OnApplyPersistentData()
    {


        TargetStation = GetComponent<CraftingStation>();
        VariableName = TargetStation.Name + transform.position.x + transform.position.y;
        if (DialogueLua.DoesVariableExist(VariableName + "CurrentProgress") == false)
        {
            return;
        }

        TargetStation.CurrentProgress = DialogueLua.GetVariable(VariableName + "CurrentProgress").asFloat;
        TargetStation.TargetProgress = DialogueLua.GetVariable(VariableName + "TargetProgress").asFloat;

        TargetStation.RecipeIndex = DialogueLua.GetVariable(VariableName + "RecipeIndex").AsInt;

        int queueAmt = DialogueLua.GetVariable(VariableName + "QueueAmount").AsInt;

        for (int i = 0; i < queueAmt; i++)
        {
            string recipeName = DialogueLua.GetVariable(VariableName + "Queue" + i).AsString;
            TargetStation.Queue.Add(CraftingManager.Instance.GetRecipeByName(recipeName));
        }

        int chosenAmt = DialogueLua.GetVariable(VariableName + "ChosenItems").AsInt;

        for (int i = 0; i < chosenAmt; i++)
        {
            int chosenID = DialogueLua.GetVariable(VariableName + "ChosenID" + i).asInt;
            string typeName = DialogueLua.GetVariable(VariableName + "ChosenType" + i).AsString;
            ItemSystem.ItemTypes type = (ItemSystem.ItemTypes)System.Enum.Parse(typeof(ItemSystem.ItemTypes), typeName);
            int chosenmAmount = DialogueLua.GetVariable(VariableName + "ChosenAmount" + i).AsInt;

            StationItem newItem = new StationItem();
            newItem.ContainedItem = ItemSystem.Instance.GetItemClone(chosenID);
            newItem.Amount = chosenmAmount;

            TargetStation.ChosenItems.Add(newItem);
        }

        string currentRecipe = DialogueLua.GetVariable(VariableName + "CurrentRecipe").AsString;
        if (currentRecipe != "Null")
        {
            TargetStation.CurrentRecipe = CraftingManager.Instance.GetRecipeByName(currentRecipe);
        }
        else
        {
            TargetStation.CurrentRecipe = null;
        }

        TargetStation.CanProgress = DialogueLua.GetVariable(VariableName + "CanProgress").asBool;

        int inputAmount = DialogueLua.GetVariable(VariableName + "RecipesInInput").AsInt;

        for (int i = 0; i < inputAmount; i++)
        {
            string recipeName = DialogueLua.GetVariable(VariableName + "InputName" + i).AsString;
            TargetStation.RecipesInInput.Add(CraftingManager.Instance.GetRecipeByName(recipeName));
        }

        string chosenRecipeName = DialogueLua.GetVariable(VariableName + "ChosenRecipe").AsString;
        if (chosenRecipeName != "Null")
        {
            TargetStation.ChosenRecipe = CraftingManager.Instance.GetRecipeByName(chosenRecipeName);
        }
        else
        {
            TargetStation.ChosenRecipe = null;
        }
        TargetStation.Load();

    }
}
