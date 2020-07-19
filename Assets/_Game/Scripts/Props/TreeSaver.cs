using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
public class TreeSaver : MonoBehaviour
{
    TreeBase TargetTree;
    string VariableName;
    string LevelName;
    public void OnEnable()
    {
        LevelName = GameManager.Instance.LevelName;
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        OnRecordPersistentData();
        PersistentDataManager.UnregisterPersistentData(this.gameObject);
    }

    void GetVariableName()
    {
        if (GameManager.Instance != null)
        {
            LevelName = GameManager.Instance.LevelName;

        }
        string objName = name.Replace(' ', '_');
        string x = transform.position.x.ToString("F2");
        string y = transform.position.y.ToString("F2");
        x = x.Replace('.', '_');
        y = y.Replace('.', '_');
        VariableName = objName + "_" + LevelName + x + "_" + y;
    }

    void OnRecordPersistentData()
    {
        GetVariableName();
        TargetTree = GetComponent<TreeBase>();

        if (TargetTree.YieldsProduce)
        {
            DialogueLua.SetVariable(VariableName + "SeasonAmount", TargetTree.ProduceSeasons.Count);
            int i = 0;
            foreach (TimeManager.MonthNames season in TargetTree.ProduceSeasons)
            {
                DialogueLua.SetVariable(VariableName + "Season" + i, season.ToString());
                i++;
            }

            DialogueLua.SetVariable(VariableName + "CurrentProduceGrowth", TargetTree.CurrentProduceGrowth);

            DialogueLua.SetVariable(VariableName + "ProduceReady", TargetTree.ProduceReady);
        }
        DialogueLua.SetVariable(VariableName + "DroppedItems", TargetTree.DroppedItems);
        DialogueLua.SetVariable(VariableName + "CurrentGrowth", TargetTree.CurrentGrowth);
        DialogueLua.SetVariable(VariableName + "Native", TargetTree.NativeTree);


    }
    void OnApplyPersistentData()
    {
        GetVariableName();
        TargetTree = GetComponent<TreeBase>();


        if (TargetTree.YieldsProduce && DialogueLua.DoesVariableExist(VariableName + "CurrentProduceGrowth"))
        {
            int seasonAmount = DialogueLua.GetVariable(VariableName + "SeasonAmount").asInt;
            TargetTree.ProduceSeasons = new List<TimeManager.MonthNames>();

            for (int i = 0; i < seasonAmount; i++)
            {
                TargetTree.ProduceSeasons.Add((TimeManager.MonthNames)System.Enum.Parse(typeof(TimeManager.MonthNames), DialogueLua.GetVariable(VariableName + "Season" + i).asString));
            }
            //            print(DialogueLua.GetVariable(VariableName + "CurrentProduceGrowth").asInt);
            TargetTree.CurrentProduceGrowth = DialogueLua.GetVariable(VariableName + "CurrentProduceGrowth").asInt;


            TargetTree.ProduceReady = DialogueLua.GetVariable(VariableName + "ProduceReady").asBool;
            TargetTree.SetProduceSprite();
            if (TargetTree.ProduceSeasons.Contains(TimeManager.Instance.CurrentMonth.Name))
            {
                if (TargetTree.FloweringSprite != null)
                {
                    TargetTree.GetComponent<SpriteRenderer>().sprite = TargetTree.FloweringSprite;
                }
            }
        }
        TargetTree.DroppedItems = DialogueLua.GetVariable(VariableName + "DroppedItems").asInt;
        TargetTree.CurrentGrowth = DialogueLua.GetVariable(VariableName + "CurrentGrowth").asInt;
        TargetTree.NativeTree = DialogueLua.GetVariable(VariableName + "Native").asBool;



    }


}
