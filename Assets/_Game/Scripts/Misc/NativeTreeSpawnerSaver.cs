using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class NativeTreeSpawnerSaver : MonoBehaviour
{

    string VariableName;
    NativeTreeSpawner TargetSpawner;

    public void OnEnable()
    {
        PersistentDataManager.RegisterPersistentData(this.gameObject);
    }

    public void OnDisable()
    {
        OnRecordPersistentData();
        PersistentDataManager.UnregisterPersistentData(this.gameObject);
    }


    void OnRecordPersistentData()
    {
        GetVariableName();
        //        print(VariableName);
        if (DialogueLua.DoesVariableExist(VariableName + "Init"))
        {
            //            print("variable set");
            return;
        }
        DialogueLua.SetVariable(VariableName + "Init", true);
    }

    void GetVariableName()
    {
        string name = "NativeTree";
        string x = transform.position.x.ToString("F2");
        string y = transform.position.y.ToString("F2");
        x = x.Replace('.', '_');
        y = y.Replace('.', '_');
        VariableName = name + x + y;
    }
    void OnApplyPersistentData()
    {
        TargetSpawner = GetComponent<NativeTreeSpawner>();
        GetVariableName();
        if (DialogueLua.DoesVariableExist(VariableName + "Init"))
        {
            Destroy(TargetSpawner.gameObject);
            return;
        }
        TargetSpawner.SpawnTree();
        Destroy(gameObject);

    }
}
