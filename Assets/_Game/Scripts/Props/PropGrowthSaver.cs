using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;

public class PropGrowthSaver : MonoBehaviour
{
    PropGrowth TargetProp;


    string VariableName;

    void Start()
    // Start is called before the first frame updatepublic override void Start()
    {
        TargetProp = GetComponent<PropGrowth>();
    }

    void OnEnable()
    {
        VariableName = name + transform.position.x.ToString("F2") + transform.position.ToString("F2");
        print(VariableName);
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnDisable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnRecordPersistentData()
    {
        DialogueLua.SetVariable(VariableName + "CurrentGrowth", TargetProp.CurrentGrowth);
    }
    void OnApplyPersistentData()
    {
        VariableName = name + transform.position.x.ToString("F2") + transform.position.ToString("F2");

        print(VariableName);
        TargetProp.CurrentGrowth = DialogueLua.GetVariable(VariableName + "CurrentGrowth").asInt;
        print(TargetProp.CurrentGrowth);
        TargetProp.CheckStage();
    }
}
