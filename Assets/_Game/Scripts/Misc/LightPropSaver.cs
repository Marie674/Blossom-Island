using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class LightPropSaver : MonoBehaviour
{
    LightProp Target;
    private string VariableName = "";


    void Start()
    {
        Target = GetComponent<LightProp>();
        VariableName = "Light" + transform.position.x + transform.position.y;
    }
    void OnEnable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    void OnDisable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);


    }

    public void OnRecordPersistentData()
    {
        print("record");
        Target = GetComponent<LightProp>();
        VariableName = "Light" + transform.position.x + transform.position.y;
        DialogueLua.SetVariable(VariableName + "On", Target.On);
        DialogueLua.SetVariable(VariableName + "Intensity", Target.TargetIntensity);
    }

    public void OnApplyPersistentData()
    {
        print("apply");

        Target = GetComponent<LightProp>();
        VariableName = "Light" + transform.position.x + transform.position.y;
        if (DialogueLua.DoesVariableExist(VariableName + "On"))
        {
            Target.On = DialogueLua.GetVariable(VariableName + "On").asBool;
            Target.TargetIntensity = DialogueLua.GetVariable(VariableName + "Intensity").asFloat;
            if (Target.On)
            {
                Target.SetOn();
            }
            else
            {
                Target.SetOff();
            }
        }





    }

}
