using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GrassPatchSaver : MonoBehaviour
{
    GrassPatch Target;
    private string VariableName = "";
    void Start()
    {
        Target = GetComponent<GrassPatch>();
        VariableName = "Grass" + transform.position.x + transform.position.y;
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
        Target = GetComponent<GrassPatch>();
        VariableName = "Grass" + transform.position.x + transform.position.y;
        DialogueLua.SetVariable(VariableName + "Pregrow", Target.Pregrow);
        GrassTuft[] children = GetComponentsInChildren<GrassTuft>(true);
        int i = 0;
        int awake = 0;
        foreach (GrassTuft child in children)
        {
            if (child.gameObject.activeSelf == true)
            {
                DialogueLua.SetVariable(VariableName + "Tuft" + i, true);
                awake += 1;
            }
            else
            {
                DialogueLua.SetVariable(VariableName + "Tuft" + i, false);

            }
            i++;
        }
        DialogueLua.SetVariable(VariableName + "AwakeChildren", awake);


    }

    public void OnApplyPersistentData()
    {

        Target = GetComponent<GrassPatch>();
        VariableName = "Grass" + transform.position.x + transform.position.y;

        if (DialogueLua.DoesVariableExist(VariableName + "Pregrow") == false)
        {
            return;
        }
        Target.Pregrow = DialogueLua.GetVariable(VariableName + "Pregrow").asBool;
        Target.AwakeChildren = DialogueLua.GetVariable(VariableName + "AwakeChildren").AsInt;
        GrassTuft[] children = GetComponentsInChildren<GrassTuft>(true);
        int i = 0;
        foreach (GrassTuft child in children)
        {
            if (DialogueLua.GetVariable(VariableName + "Tuft" + i).asBool == true)
            {

                child.gameObject.SetActive(true);
            }
            else
            {

                child.gameObject.SetActive(false);

            }
            i++;
        }
    }
}
